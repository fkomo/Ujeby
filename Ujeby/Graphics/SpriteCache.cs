using SDL2;
using System.Numerics;
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

		public static Sprite LoadSprite(string filename, string id = null)
		{
			var sprite = Library.Values.SingleOrDefault(s => s.Filename == filename);
			if (sprite == null)
			{
				sprite = new Sprite
				{
					Filename = filename,
					Id = id ?? Guid.NewGuid().ToString("N"),
				};
				if (!LoadImage(filename, out sprite.ImagePtr, out sprite.Size, out sprite.Data))
					return null;

				Library.Add(sprite.Id, sprite);
			}
			return sprite;
		}

		public static Sprite Get(string id)
		{
			if (string.IsNullOrEmpty(id))
				return null;

			if (!Library.TryGetValue(id, out Sprite sprite))
			{
				if (!LibraryFileMap.TryGetValue(id, out string filename))
					return null;

				sprite = LoadSprite(filename, id);
				if (CreateTexture(sprite.Id, out Sprite spriteWithTexture))
					sprite = spriteWithTexture;
			}

			return sprite;
		}

		public static Font LoadFont(string name)
		{
			var file = Path.Combine(ContentDirectory, "Fonts", $"{ name }.png");

			var fileInfo = new FileInfo(file);
			var sizeString = fileInfo.Name
				.Replace(fileInfo.Extension, string.Empty)
				.Split("-").Last()
				.Split("x");

			var font = new Font
			{
				SpriteId = LoadSprite(file)?.Id,
				CharSize = new v2i(Convert.ToInt32(sizeString[0]), Convert.ToInt32(sizeString[1])),
				Spacing = new v2i(1, 1),
			};

			// create aabb's for each character
			var dataFile = Path.Combine(ContentDirectory, "Fonts", $"{ name }-data.png");
			if (File.Exists(dataFile))
			{
				var dataSprite = LoadSprite(dataFile);
				font.DataSpriteId = dataSprite.Id;

				font.CharBoxes = new AABBi[(int)(dataSprite.Size.X / font.CharSize.X)];
				for (var ci = 0; ci < dataSprite.Size.X; ci += (int)font.CharSize.X)
				{
					var min = new v2i(font.CharSize.X, font.CharSize.Y);
					var max = v2i.Zero;

					for (var y = 0; y < font.CharSize.Y; y++)
					{
						for (var x = 0; x < font.CharSize.X; x++)
						{
							var index = (int)(y * dataSprite.Size.X + x + ci);
							if (dataSprite.Data[index] != 0)
							{
								min.X = Math.Min(min.X, x);
								min.Y = Math.Min(min.Y, y);
								max.X = Math.Max(max.X, x + 1);
								max.Y = Math.Max(max.Y, y + 1);
							}
						}
					}

					font.CharBoxes[(int)(ci / font.CharSize.X)] = new AABBi(min, max);
				}
			}

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
			sprite = Library[spriteId];
			if (sprite.ImagePtr != IntPtr.Zero && sprite.TexturePtr == IntPtr.Zero)
			{
				sprite.TexturePtr = SDL2.SDL.SDL_CreateTextureFromSurface(Sdl2Wrapper.RendererPtr, sprite.ImagePtr);
				Library[spriteId] = sprite;

				return true;
			}

			return false;
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

		private static bool LoadImage(string filename, out IntPtr imagePtr, out v2i size, out uint[] data)
		{
			imagePtr = IntPtr.Zero;
			size = v2i.Zero;
			data = null;

			imagePtr = SDL_image.IMG_Load(filename);
			var surface = Marshal.PtrToStructure<SDL2.SDL.SDL_Surface>(imagePtr);

			size = new v2i(surface.w, surface.h);

			//var bitmap = new Bitmap(fileName);
			//var data = bitmap.LockBits(
			//	new Rectangle(Point.Empty, bitmap.Size), 
			//	System.Drawing.Imaging.ImageLockMode.ReadWrite,
			//	bitmap.PixelFormat);

			//sprite.Data = new byte[data.Height * data.Stride];
			//Marshal.Copy(data.Scan0, sprite.Data, 0, sprite.Data.Length);

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
