using SDL2;
using System.Reflection;
using System.Runtime.InteropServices;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.Graphics
{
	public class SpriteCache
	{
		public static string ContentDirectory => Path.Combine(Environment.CurrentDirectory, "Content");
		public static Dictionary<string, string> LibraryFileMap { get; private set; }

		public static void Initialize(
			Dictionary<string, string> library = null)
		{
			LibraryFileMap = library;
		}

		/// <summary>
		/// spriteId vs Sprite
		/// </summary>
		private static readonly Dictionary<string, Sprite> Library = new();

		public static Sprite Get(string id)
		{
			if (string.IsNullOrEmpty(id))
				return null;

			if (!Library.TryGetValue(id, out Sprite sprite))
			{
				if (!LibraryFileMap.TryGetValue(id, out string filename))
					return null;

				sprite = LoadSpriteFromFile(filename, id);
				if (CreateTexture(sprite.Id, out Sprite spriteWithTexture))
					sprite = spriteWithTexture;
			}

			return sprite;
		}

		public static Font LoadFont(Func<string, string, Sprite> loadSpriteFunc, string fontName)
		{
			var fontBaseFile = $"Ujeby.Content.Fonts.{fontName}.png";

			var sizeString = fontBaseFile
				.Split(".").SkipLast(1).Last()
				.Split("-").Last()
				.Split("x");

			var font = new Font
			{
				SpriteId = loadSpriteFunc(fontBaseFile, null)?.Id,
				CharSize = new v2i(long.Parse(sizeString[0]), long.Parse(sizeString[1])),
				Spacing = new v2i(1, 1),
			};

			// create aabb's for each character
			var aabbSprite = loadSpriteFunc($"Ujeby.Content.Fonts.{fontName}-aabb.png", null);
			if (aabbSprite != null)
			{
				font.AABBSpriteId = aabbSprite.Id;

				font.CharBoxes = new AABox2i[(int)(aabbSprite.Size.X / font.CharSize.X)];
				for (var ci = 0; ci < aabbSprite.Size.X; ci += (int)font.CharSize.X)
				{
					var min = new v2i(font.CharSize.X, font.CharSize.Y);
					var max = v2i.Zero;

					for (var y = 0; y < font.CharSize.Y; y++)
					{
						for (var x = 0; x < font.CharSize.X; x++)
						{
							var index = (int)(y * aabbSprite.Size.X + x + ci);
							if (aabbSprite.Data[index] != 0)
							{
								min.X = Math.Min(min.X, x);
								min.Y = Math.Min(min.Y, y);
								max.X = Math.Max(max.X, x + 1);
								max.Y = Math.Max(max.Y, y + 1);
							}
						}
					}

					font.CharBoxes[(int)(ci / font.CharSize.X)] = new AABox2i(min, max);
				}
			}

			var outlineSprite = loadSpriteFunc($"Ujeby.Content.Fonts.{fontName}-outline.png", null);
			if (outlineSprite != null)
				font.OutlineSpriteId = outlineSprite.Id;

			return font;
		}

		public static void CreateTextures()
		{
			var count = 0;
			for (var i = 0; i < Library.Keys.Count; i++)
			{
				if (CreateTexture(Library.Keys.ElementAt(i), out _))
					count++;
			}
		}

		public static bool CreateTexture(string spriteId, out Sprite sprite)
		{
			sprite = null;
			if (string.IsNullOrEmpty(spriteId))
				return false;

			if (!Library.TryGetValue(spriteId, out sprite))
				return false;

			if (sprite.ImagePtr == IntPtr.Zero)
				return false;

			if (sprite.TexturePtr == IntPtr.Zero)
			{
				sprite.TexturePtr = SDL2.SDL.SDL_CreateTextureFromSurface(Sdl2Wrapper.RendererPtr, sprite.ImagePtr);
				Library[spriteId] = sprite;
			}

			return true;
		}

		public static void Destroy()
		{
			// free sdl images/textures
			foreach (var sprite in Library.Values)
			{
				if (sprite.ImagePtr != IntPtr.Zero)
					SDL2.SDL.SDL_FreeSurface(sprite.ImagePtr);
	
				if (sprite.TexturePtr != IntPtr.Zero)
					SDL2.SDL.SDL_DestroyTexture(sprite.TexturePtr);
			}

			Library.Clear();
		}

		public static Sprite LoadSpriteFromFile(string filename, 
			string id = null)
		{
			var sprite = Library.Values.SingleOrDefault(s => s.Filename == filename);
			if (sprite == null)
			{
				sprite = new Sprite
				{
					Filename = filename,
					Id = id ?? Guid.NewGuid().ToString("N"),
				};
				if (!LoadImageFromFile(filename, out sprite.ImagePtr, out sprite.Size, out sprite.Data))
					return null;

				Library.Add(sprite.Id, sprite);
			}
			return sprite;
		}

		public static Sprite LoadSpriteFromResource(string filename, string id = null)
		{
			var sprite = Library.Values.SingleOrDefault(s => s.Filename == filename);
			if (sprite == null)
			{
				sprite = new Sprite
				{
					Filename = filename,
					Id = id ?? Guid.NewGuid().ToString("N"),
				};
				if (!LoadImageFromResource(filename, out sprite.ImagePtr, out sprite.Size, out sprite.Data))
					return null;

				Library.Add(sprite.Id, sprite);
			}
			return sprite;
		}

		private static bool LoadImageFromResource(string name, out IntPtr imagePtr, out v2i size, out uint[] data)
		{
			var imageData = Array.Empty<byte>();

			var assembly = Assembly.GetExecutingAssembly();
			using (var resourceStream = assembly.GetManifestResourceStream(name))
			{
				if (resourceStream == null)
				{
					imagePtr = IntPtr.Zero;
					data = null;
					size = new v2i();

					return false;
				}

				imageData = new byte[resourceStream.Length];
				resourceStream.Read(imageData, 0, imageData.Length);
			}

			var imageDataPtr = Marshal.AllocHGlobal(imageData.Length);
			Marshal.Copy(imageData, 0, imageDataPtr, imageData.Length);

			var stream = SDL.SDL_RWFromMem(imageDataPtr, imageData.Length);

			imagePtr = SDL_image.IMG_Load_RW(stream, 0);
			var surface = Marshal.PtrToStructure<SDL2.SDL.SDL_Surface>(imagePtr);

			Marshal.FreeHGlobal(imageDataPtr);

			return LoadImage(surface, out size, out data);
		}

		private static bool LoadImageFromFile(string filename, out IntPtr imagePtr, out v2i size, out uint[] data)
		{
			imagePtr = SDL_image.IMG_Load(filename);
			var surface = Marshal.PtrToStructure<SDL2.SDL.SDL_Surface>(imagePtr);

			return LoadImage(surface, out size, out data);
		}

		private static bool LoadImage(SDL.SDL_Surface surface, out v2i size, out uint[] data)
		{
			size = new v2i(surface.w, surface.h);

			var tmpData = new byte[surface.w * surface.h * 4];
			Marshal.Copy(surface.pixels, tmpData, 0, tmpData.Length);

			var i2 = 0;
			data = new uint[surface.w * surface.h];
			for (var y = surface.h - 1; y >= 0; y--)
				for (var x = 0; x < surface.w; x++, i2++)
				{
					var i = y * surface.w + x;
					data[i2] =
						((uint)tmpData[i * 4 + 0]) +
						((uint)tmpData[i * 4 + 1] << 8) +
						((uint)tmpData[i * 4 + 2] << 16) +
						((uint)tmpData[i * 4 + 3] << 24);
				}

			return true;
		}
	}
}
