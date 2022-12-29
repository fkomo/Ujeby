using SDL2;
using Ujeby.Graphics.Entities;
using Ujeby.Vectors;

namespace Ujeby.Graphics.Sdl
{
	public static class Sdl2Renderer
	{
		public static void DrawRect(int x, int y, int w, int h,
			v4f? border = null, v4f? fill = null)
		{
			var rect = new SDL.SDL_Rect
			{
				x = x,
				y = y,
				w = w,
				h = h,
			};

			if (fill.HasValue)
			{
				var fColor = fill.Value * 255;
				_ = SDL.SDL_SetRenderDrawColor(Sdl2Wrapper.RendererPtr, (byte)fColor.X, (byte)fColor.Y, (byte)fColor.Z, (byte)fColor.W);
				_ = SDL.SDL_RenderFillRect(Sdl2Wrapper.RendererPtr, ref rect);
			}

			if (border.HasValue)
			{
				var bColor = border.Value * 255;
				_ = SDL.SDL_SetRenderDrawColor(Sdl2Wrapper.RendererPtr, (byte)bColor.X, (byte)bColor.Y, (byte)bColor.Z, (byte)bColor.W);
				_ = SDL.SDL_RenderDrawRect(Sdl2Wrapper.RendererPtr, ref rect);
			}
		}

		public static void DrawLine(int x1, int y1, int x2, int y2, v4f color)
		{
			var bColor = color * 255;

			_ = SDL.SDL_SetRenderDrawColor(Sdl2Wrapper.RendererPtr, (byte)bColor.X, (byte)bColor.Y, (byte)bColor.Z, (byte)bColor.W);
			_ = SDL.SDL_RenderDrawLine(Sdl2Wrapper.RendererPtr, x1, y1, x2, y2);
		}

		public static void DrawText(Font font, v2i position, v2i spacing, v2i scale, 
			params TextLine[] lines)
		{
			var fontSprite = SpriteCache.Get(Sdl2Wrapper.CurrentFont.SpriteId);

			var sourceRect = new SDL.SDL_Rect();
			var destinationRect = new SDL.SDL_Rect();

			var textPosition = position;
			foreach (var line in lines)
			{
				if (line is Text text)
				{
					var color = text.Color * 255;
					_ = SDL.SDL_SetTextureColorMod(fontSprite.TexturePtr, (byte)color.X, (byte)color.Y, (byte)color.Z);

					for (var i = 0; i < text.Value.Length; i++)
					{
						var charIndex = (int)text.Value[i] - 32;
						var charAabb = font.CharBoxes[charIndex];

						sourceRect.x = (int)(font.CharSize.X * charIndex + charAabb.Min.X);
						sourceRect.y = (int)charAabb.Min.Y;
						sourceRect.w = (int)charAabb.Size.X;
						sourceRect.h = (int)charAabb.Size.Y;

						destinationRect.x = (int)(textPosition.X);
						destinationRect.y = (int)(textPosition.Y);
						destinationRect.w = (int)(charAabb.Size.X * scale.X);
						destinationRect.h = (int)(charAabb.Size.Y * scale.Y);

						_ = SDL.SDL_RenderCopy(Sdl2Wrapper.RendererPtr, fontSprite.TexturePtr, ref sourceRect, ref destinationRect);
						textPosition.X += (charAabb.Size.X + font.Spacing.X + spacing.X) * scale.X;
					}

					textPosition.Y += (font.CharSize.Y + font.Spacing.Y + spacing.Y) * scale.Y;
					textPosition.X = position.X;
				}
				else if (line is EmptyLine)
				{
					textPosition.Y += font.CharSize.Y + font.Spacing.Y + spacing.Y;
				}
			}
		}

		internal static void Clear(v4f color)
		{
			var bColor = color * 255;
			_ = SDL.SDL_SetRenderDrawColor(Sdl2Wrapper.RendererPtr, (byte)bColor.X, (byte)bColor.Y, (byte)bColor.Z, (byte)bColor.W);
			_ = SDL.SDL_RenderClear(Sdl2Wrapper.RendererPtr);
		}
	}
}
