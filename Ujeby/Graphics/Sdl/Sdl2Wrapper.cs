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

			CurrentFont = SpriteCache.LoadFont(SpriteCache.LoadSpriteFromResource,
				fontName: $"Ujeby.Content.Fonts.{fontName}.png",
				fontDataName: $"Ujeby.Content.Fonts.{fontName}-data.png"); ;

			SpriteCache.CreateTexture(CurrentFont.SpriteId, out _);
		}

		public static void Destroy()
		{
			SDL.SDL_DestroyRenderer(RendererPtr);
			SDL.SDL_DestroyWindow(WindowPtr);
			SDL.SDL_Quit();
		}

		private static readonly byte[] CurrentKeys = new byte[(int)SDL.SDL_Scancode.SDL_NUM_SCANCODES];
		private static readonly byte[] PreviousKeys = new byte[(int)SDL.SDL_Scancode.SDL_NUM_SCANCODES];

		public static int MouseWheel { get; set; }

		public static bool HandleInput(Func<bool> handleInput)
		{
			while (SDL.SDL_PollEvent(out SDL.SDL_Event e) != 0)
			{
				MouseWheel = 0;

				switch (e.type)
				{
					case SDL.SDL_EventType.SDL_QUIT:
						return false;
				};

				if (e.type == SDL.SDL_EventType.SDL_KEYUP || e.type == SDL.SDL_EventType.SDL_KEYDOWN)
				{
					CurrentKeys.CopyTo(PreviousKeys, 0);
					var keysBuffer = SDL.SDL_GetKeyboardState(out int keysBufferLength);
					System.Runtime.InteropServices.Marshal.Copy(keysBuffer, CurrentKeys, 0, keysBufferLength);

					if (KeyPressed(SDL.SDL_Scancode.SDL_SCANCODE_ESCAPE))
						return false;

					// TODO handle user input

					//if (KeyPressed(SDL2.SDL.SDL_Scancode.SDL_SCANCODE_UP))
					//	player.HandleButton(InputButton.Up, InputButtonState.Pressed);
					//if (KeyReleased(SDL2.SDL.SDL_Scancode.SDL_SCANCODE_UP))
					//	player.HandleButton(InputButton.Up, InputButtonState.Released);

					//if (KeyPressed(SDL2.SDL.SDL_Scancode.SDL_SCANCODE_DOWN))
					//	player.HandleButton(InputButton.Down, InputButtonState.Pressed);
					//if (KeyReleased(SDL2.SDL.SDL_Scancode.SDL_SCANCODE_DOWN))
					//	player.HandleButton(InputButton.Down, InputButtonState.Released);

					//if (KeyPressed(SDL2.SDL.SDL_Scancode.SDL_SCANCODE_LEFT))
					//	player.HandleButton(InputButton.Left, InputButtonState.Pressed);
					//if (KeyReleased(SDL2.SDL.SDL_Scancode.SDL_SCANCODE_LEFT))
					//	player.HandleButton(InputButton.Left, InputButtonState.Released);

					//if (KeyPressed(SDL2.SDL.SDL_Scancode.SDL_SCANCODE_RIGHT))
					//	player.HandleButton(InputButton.Right, InputButtonState.Pressed);
					//if (KeyReleased(SDL2.SDL.SDL_Scancode.SDL_SCANCODE_RIGHT))
					//	player.HandleButton(InputButton.Right, InputButtonState.Released);

					//if (KeyPressed(SDL2.SDL.SDL_Scancode.SDL_SCANCODE_LSHIFT))
					//	player.HandleButton(InputButton.LT, InputButtonState.Pressed);
					//if (KeyReleased(SDL2.SDL.SDL_Scancode.SDL_SCANCODE_LSHIFT))
					//	player.HandleButton(InputButton.LT, InputButtonState.Released);

					//if (KeyPressed(SDL2.SDL.SDL_Scancode.SDL_SCANCODE_RSHIFT))
					//	player.HandleButton(InputButton.RT, InputButtonState.Pressed);
					//if (KeyReleased(SDL2.SDL.SDL_Scancode.SDL_SCANCODE_RSHIFT))
					//	player.HandleButton(InputButton.RT, InputButtonState.Released);

					return handleInput();
				}

				else if (e.type == SDL.SDL_EventType.SDL_MOUSEWHEEL)
				{
					if (e.wheel.y > 0) // scroll up
					{
					}
					else if (e.wheel.y < 0) // scroll down
					{
					}

					MouseWheel = e.wheel.y;
				}
			}

			return true;
		}

		public static bool KeyPressed(SDL.SDL_Scancode scanCode)
		{
			return CurrentKeys[(int)scanCode] == 1 && PreviousKeys[(int)scanCode] == 0;
		}

		public static bool KeyReleased(SDL.SDL_Scancode scanCode)
		{
			return CurrentKeys[(int)scanCode] == 0 && PreviousKeys[(int)scanCode] == 1;
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

		public static v2i GetMouse(out uint mouseState)
		{
			mouseState = SDL.SDL_GetMouseState(out int mouseX, out int mouseY);
			return new(mouseX, mouseY);
		}

		public static void Render()
		{
			SDL.SDL_RenderPresent(RendererPtr);
		}

		public static void Clear(v4f color)
		{
			var bColor = color * 255;
			_ = SDL.SDL_SetRenderDrawColor(Sdl2Wrapper.RendererPtr, (byte)bColor.X, (byte)bColor.Y, (byte)bColor.Z, (byte)bColor.W);
			_ = SDL.SDL_RenderClear(Sdl2Wrapper.RendererPtr);
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

		public static void DrawCircle(int cx, int cy, int radius,
			v4f? border = null, v4f? fill = null)
		{
			// TODO DrawCircle fill

			if (fill.HasValue)
			{
				var fColor = fill.Value * 255;
				_ = SDL.SDL_SetRenderDrawColor(Sdl2Wrapper.RendererPtr, (byte)fColor.X, (byte)fColor.Y, (byte)fColor.Z, (byte)fColor.W);

				for (int w = 0; w < radius * 2; w++)
				{
					for (int h = 0; h < radius * 2; h++)
					{
						int dx = radius - w; // horizontal offset
						int dy = radius - h; // vertical offset
						if ((dx * dx + dy * dy) <= (radius * radius))
						{
							_ = SDL.SDL_RenderDrawPoint(Sdl2Wrapper.RendererPtr, cx + dx, cy + dy);
						}
					}
				}
			}

			if (border.HasValue)
			{
				var bColor = border.Value * 255;
				_ = SDL.SDL_SetRenderDrawColor(Sdl2Wrapper.RendererPtr, (byte)bColor.X, (byte)bColor.Y, (byte)bColor.Z, (byte)bColor.W);
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
				_ = SDL.SDL_RenderDrawPoint(Sdl2Wrapper.RendererPtr, cx + x, cy - y);
				_ = SDL.SDL_RenderDrawPoint(Sdl2Wrapper.RendererPtr, cx + x, cy + y);
				_ = SDL.SDL_RenderDrawPoint(Sdl2Wrapper.RendererPtr, cx - x, cy - y);
				_ = SDL.SDL_RenderDrawPoint(Sdl2Wrapper.RendererPtr, cx - x, cy + y);
				_ = SDL.SDL_RenderDrawPoint(Sdl2Wrapper.RendererPtr, cx + y, cy - x);
				_ = SDL.SDL_RenderDrawPoint(Sdl2Wrapper.RendererPtr, cx + y, cy + x);
				_ = SDL.SDL_RenderDrawPoint(Sdl2Wrapper.RendererPtr, cx - y, cy - x);
				_ = SDL.SDL_RenderDrawPoint(Sdl2Wrapper.RendererPtr, cx - y, cy + x);

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
			params TextLine[] lines)
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
					textPosition.Y += (font.CharSize.Y + font.Spacing.Y + spacing.Y) * scale.Y;
				}
			}
		}

		public static void DrawText(v2i topLeft,
			params TextLine[] lines)
		{
			DrawText(
				topLeft,
				new v2i(0),
				new v2i(2),
				HorizontalTextAlign.Left,
				VerticalTextAlign.Top,
				lines);
		}

		public static void DrawText(v2i topLeft, v2i spacing, v2i scale,
			params TextLine[] lines)
		{
			DrawText(
				topLeft,
				spacing,
				scale,
				HorizontalTextAlign.Left,
				VerticalTextAlign.Top,
				lines);
		}

		public static void DrawText(v2i topLeft, HorizontalTextAlign alignH, VerticalTextAlign alignV,
			params TextLine[] lines)
		{
			DrawText(
				topLeft,
				new v2i(0),
				new v2i(2),
				alignH,
				alignV,
				lines);
		}

		public static void DrawText(v2i position, v2i spacing, v2i scale, HorizontalTextAlign alignH, VerticalTextAlign alignV,
			params TextLine[] lines)
		{
			DrawText(
				Sdl2Wrapper.CurrentFont,
				position,
				spacing,
				scale,
				alignH,
				alignV,
				lines);
		}
	}
}
