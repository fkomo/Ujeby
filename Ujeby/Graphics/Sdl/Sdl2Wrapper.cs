using SDL2;
using Ujeby.Graphics.Entities;
using Ujeby.Vectors;

namespace Ujeby.Graphics.Sdl
{
	public static class Sdl2Wrapper
	{
		public static IntPtr WindowPtr;
		public static IntPtr RendererPtr;

		public static Font CurrentFont;

		public static v2i WindowSize { get; private set; }

		public static void CreateWindow(string title, v2i windowSize)
		{
			WindowSize = windowSize;

			SpriteCache.Initialize();

			if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
				throw new Exception($"Failed to initialize SDL2 library. SDL2Error({SDL.SDL_GetError()})");

			WindowPtr = SDL.SDL_CreateWindow(title,
				SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED,
				(int)windowSize.X, (int)windowSize.Y,
				SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL);
			if (WindowPtr == IntPtr.Zero)
				throw new Exception($"Failed to create window. SDL2Error({SDL.SDL_GetError()})");

			RendererPtr = SDL.SDL_CreateRenderer(WindowPtr, -1,
				SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED
				);
			if (RendererPtr == IntPtr.Zero)
				throw new Exception($"Failed to create renderer. SDL2Error({SDL.SDL_GetError()})");

			_ = SDL.SDL_SetRenderDrawBlendMode(RendererPtr, SDL.SDL_BlendMode.SDL_BLENDMODE_ADD);
		}

		public static IntPtr CreateScreen(v2i screenSize)
		{
			_ = SDL.SDL_RenderSetLogicalSize(RendererPtr, (int)screenSize.X, (int)screenSize.Y);
			_ = SDL.SDL_RenderSetIntegerScale(RendererPtr, SDL.SDL_bool.SDL_TRUE);

			return SDL.SDL_CreateTexture(RendererPtr,
				SDL.SDL_PIXELFORMAT_RGBA8888, (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_STREAMING,
				(int)screenSize.X, (int)screenSize.Y);
		}

		public static void CreateFont(string fontName = "font-5x7")
		{
			//CurrentFont = SpriteCache.LoadFont(SpriteCache.LoadSpriteFromFile,
			//	fontName: Path.Combine(SpriteCache.ContentDirectory, "Fonts", $"{fontName}.png"),
			//	fontDataName: Path.Combine(SpriteCache.ContentDirectory, "Fonts", $"{fontName}-data.png");

			CurrentFont = SpriteCache.LoadFont(SpriteCache.LoadSpriteFromResource, fontName);

			SpriteCache.CreateTexture(CurrentFont.SpriteId, out _);
		}

		public static void Destroy()
		{
			SDL.SDL_DestroyRenderer(RendererPtr);
			SDL.SDL_DestroyWindow(WindowPtr);
			SDL.SDL_Quit();
		}

		public static void SetWindowTitle(string title)
		{
			SDL.SDL_SetWindowTitle(WindowPtr, title);
		}

		public static void ShowCursor(
			bool show = true)
		{
			_ = SDL.SDL_ShowCursor(show ? 1 : 0);
		}

		public static void Render()
		{
			SDL.SDL_RenderPresent(RendererPtr);
		}

		public static void Clear(v4f color)
		{
			var bColor = color * 255;
			_ = SDL.SDL_SetRenderDrawColor(RendererPtr, (byte)bColor.X, (byte)bColor.Y, (byte)bColor.Z, (byte)bColor.W);
			_ = SDL.SDL_RenderClear(RendererPtr);
		}

		public static void DrawPixel(int x, int y, v4f color)
		{
			var bColor = color * 255;
			_ = SDL.SDL_SetRenderDrawColor(RendererPtr, (byte)bColor.X, (byte)bColor.Y, (byte)bColor.Z, (byte)bColor.W);
			_ = SDL.SDL_RenderDrawPoint(RendererPtr, x, y);
		}

		public static void DrawPixel(v2i p, v4f color)
		{
			DrawPixel((int)p.X, (int)p.Y, color);
		}

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
				_ = SDL.SDL_SetRenderDrawColor(RendererPtr, (byte)fColor.X, (byte)fColor.Y, (byte)fColor.Z, (byte)fColor.W);
				_ = SDL.SDL_RenderFillRect(RendererPtr, ref rect);
			}

			if (border.HasValue)
			{
				var bColor = border.Value * 255;
				_ = SDL.SDL_SetRenderDrawColor(RendererPtr, (byte)bColor.X, (byte)bColor.Y, (byte)bColor.Z, (byte)bColor.W);
				_ = SDL.SDL_RenderDrawRect(RendererPtr, ref rect);
			}
		}

		public static void DrawLine(int x1, int y1, int x2, int y2, v4f color)
		{
			var bColor = color * 255;
			_ = SDL.SDL_SetRenderDrawColor(RendererPtr, (byte)bColor.X, (byte)bColor.Y, (byte)bColor.Z, (byte)bColor.W);
			_ = SDL.SDL_RenderDrawLine(RendererPtr, x1, y1, x2, y2);
		}

		public static void DrawCircle(int cx, int cy, int radius,
			v4f? border = null, v4f? fill = null)
		{
			// TODO DrawCircle fill

			if (fill.HasValue)
			{
				var fColor = fill.Value * 255;
				_ = SDL.SDL_SetRenderDrawColor(RendererPtr, (byte)fColor.X, (byte)fColor.Y, (byte)fColor.Z, (byte)fColor.W);

				for (int w = 0; w < radius * 2; w++)
				{
					for (int h = 0; h < radius * 2; h++)
					{
						int dx = radius - w; // horizontal offset
						int dy = radius - h; // vertical offset
						if ((dx * dx + dy * dy) <= (radius * radius))
						{
							_ = SDL.SDL_RenderDrawPoint(RendererPtr, cx + dx, cy + dy);
						}
					}
				}
			}

			if (border.HasValue)
			{
				var bColor = border.Value * 255;
				_ = SDL.SDL_SetRenderDrawColor(RendererPtr, (byte)bColor.X, (byte)bColor.Y, (byte)bColor.Z, (byte)bColor.W);
			}

			var diameter = radius + radius;
			var x = radius - 1;
			var y = 0;
			var tx = 1;
			var ty = 1;
			var error = tx - diameter;
			while (x >= y)
			{
				//  Each of the following renders an octant of the circle
				_ = SDL.SDL_RenderDrawPoint(RendererPtr, cx + x, cy - y);
				_ = SDL.SDL_RenderDrawPoint(RendererPtr, cx + x, cy + y);
				_ = SDL.SDL_RenderDrawPoint(RendererPtr, cx - x, cy - y);
				_ = SDL.SDL_RenderDrawPoint(RendererPtr, cx - x, cy + y);
				_ = SDL.SDL_RenderDrawPoint(RendererPtr, cx + y, cy - x);
				_ = SDL.SDL_RenderDrawPoint(RendererPtr, cx + y, cy + x);
				_ = SDL.SDL_RenderDrawPoint(RendererPtr, cx - y, cy - x);
				_ = SDL.SDL_RenderDrawPoint(RendererPtr, cx - y, cy + x);

				if (error <= 0)
				{
					++y;
					error += ty;
					ty += 2;
				}

				if (error > 0)
				{
					--x;
					tx += 2;
					error += (tx - diameter);
				}
			}
		}

		public static void DrawText(Font font, v2i position, v2i spacing, v2i scale, HorizontalTextAlign alignH, VerticalTextAlign alignV,
			v4f? outlineColor, params TextLine[] lines)
		{
			var textSize = font.GetTextSize(spacing, scale, lines);
			var align = new v2i();
			switch (alignH)
			{
				case HorizontalTextAlign.Center: align.X = textSize.X / 2; break;
				case HorizontalTextAlign.Right: align.X = textSize.X; break;
				case HorizontalTextAlign.Left:
				default:
					break;
			}
			switch (alignV)
			{
				case VerticalTextAlign.Center: align.Y = textSize.Y / 2; break;
				case VerticalTextAlign.Bottom: /*align.Y = -textSize.Y; break;*/
				case VerticalTextAlign.Top: /*align.Y = textSize.Y; break;*/
				default:
					break;
			}
			position -= align;

			var fontSprite = SpriteCache.Get(font.SpriteId);

			var sourceRect = new SDL.SDL_Rect();
			var destinationRect = new SDL.SDL_Rect();

			var oColor = new v4f();
			var outlineSprite = SpriteCache.Get(font.OutlineSpriteId);
			if (outlineColor.HasValue && outlineSprite != null)
				oColor = outlineColor.Value * 255;

			var textPosition = position;
			foreach (var line in lines)
			{
				if (line is Text text)
				{
					var color = text.Color * 255;

					for (var i = 0; i < text.Value.Length; i++)
					{
						var charIndex = text.Value[i] - 32;
						var charAabb = font.CharBoxes[charIndex];

						sourceRect.x = (int)(font.CharSize.X * charIndex + charAabb.Min.X);
						sourceRect.y = (int)charAabb.Min.Y;
						sourceRect.w = (int)charAabb.Size.X;
						sourceRect.h = (int)charAabb.Size.Y;

						destinationRect.x = (int)textPosition.X;
						destinationRect.y = (int)textPosition.Y;
						destinationRect.w = (int)(charAabb.Size.X * scale.X);
						destinationRect.h = (int)(charAabb.Size.Y * scale.Y);

						if (outlineColor.HasValue && outlineSprite != null)
						{
							var outlineSourceRect = sourceRect;
							outlineSourceRect.w += 2;
							outlineSourceRect.x -= 1;

							var outlineDestRect = destinationRect;
							outlineDestRect.x -= (int)scale.X;
							outlineDestRect.w += 2 * (int)scale.X;

							_ = SDL.SDL_SetTextureColorMod(outlineSprite.TexturePtr,
								(byte)oColor.X, (byte)oColor.Y, (byte)oColor.Z);
							_ = SDL.SDL_RenderCopy(Sdl2Wrapper.RendererPtr, outlineSprite.TexturePtr,
								ref outlineSourceRect, ref outlineDestRect);
						}

						_ = SDL.SDL_SetTextureColorMod(fontSprite.TexturePtr, (byte)color.X, (byte)color.Y, (byte)color.Z);
						_ = SDL.SDL_RenderCopy(Sdl2Wrapper.RendererPtr, fontSprite.TexturePtr, ref sourceRect, ref destinationRect);
						textPosition.X += (charAabb.Size.X + font.Spacing.X + spacing.X) * scale.X;
					}

					textPosition.Y += (font.CharSize.Y + font.Spacing.Y + spacing.Y) * scale.Y;
					textPosition.X = position.X;
				}
				else if (line is EmptyLine)
				{
					textPosition.Y += (font.CharSize.Y + font.Spacing.Y + spacing.Y) * scale.Y;
				}
			}
		}

		public static void DrawText(v2i topLeft, v4f? outlineColor,
			params TextLine[] lines)
		{
			DrawText(
				topLeft,
				new v2i(0),
				new v2i(2),
				HorizontalTextAlign.Left,
				VerticalTextAlign.Top,
				outlineColor,
				lines);
		}

		public static void DrawText(v2i topLeft, v2i spacing, v2i scale, v4f? outlineColor,
			params TextLine[] lines)
		{
			DrawText(
				topLeft,
				spacing,
				scale,
				HorizontalTextAlign.Left,
				VerticalTextAlign.Top,
				outlineColor,
				lines);
		}

		public static void DrawText(v2i topLeft, HorizontalTextAlign alignH, VerticalTextAlign alignV, v4f? outlineColor,
			params TextLine[] lines)
		{
			DrawText(
				topLeft,
				new v2i(0),
				new v2i(2),
				alignH,
				alignV,
				outlineColor,
				lines);
		}

		public static void DrawText(v2i position, v2i spacing, v2i scale, HorizontalTextAlign alignH, VerticalTextAlign alignV,
			v4f? outlineColor, 
			params TextLine[] lines)
		{
			DrawText(
				Sdl2Wrapper.CurrentFont,
				position,
				spacing,
				scale,
				alignH,
				alignV,
				outlineColor,
				lines);
		}

		/// <summary>
		/// draw sprite to screen
		/// </summary>
		/// <param name="sprite">width == height. if not, sprite is considered as animation strip</param>
		/// <param name="spritePosition"></param>
		/// <param name="frame"></param>
		public static void DrawSprite(Sprite sprite, v2i spritePosition, 
			int frame = 0)
		{
			if (sprite == null)
				return;

			var spriteSize = (int)sprite.Size.Y;

			var sourceRect = new SDL.SDL_Rect
			{
				x = spriteSize * frame,
				y = 0,
				w = spriteSize,
				h = spriteSize,
			};

			var destinationRect = new SDL.SDL_Rect
			{
				x = (int)spritePosition.X,
				y = (int)spritePosition.Y,
				w = spriteSize,
				h = spriteSize,
			};
			_ = SDL.SDL_RenderCopy(RendererPtr, sprite.TexturePtr, ref sourceRect, ref destinationRect);
		}
	}
}
