using SDL2;
using System.Numerics;
using Ujeby.Graphics.Entities;
using Ujeby.Vectors;

namespace Ujeby.Graphics.Sdl
{
	public static class Sdl2Wrapper
	{
		internal static IntPtr WindowPtr;
		internal static IntPtr RendererPtr;
		
		public static Font CurrentFont;

		public static void Init(string title, v2i windowSize,
			string fontName = "font-5x7")
		{
			SpriteCache.Initialize();

			if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
				throw new Exception($"Failed to initialize SDL2 library. SDL2Error({SDL.SDL_GetError()})");

			WindowPtr = SDL.SDL_CreateWindow(title,
				SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED,
				(int)windowSize.X, (int)windowSize.Y,
				SDL.SDL_WindowFlags.SDL_WINDOW_VULKAN);
			if (WindowPtr == IntPtr.Zero)
				throw new Exception($"Failed to create window. SDL2Error({SDL.SDL_GetError()})");

			RendererPtr = SDL.SDL_CreateRenderer(WindowPtr, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);
			if (RendererPtr == IntPtr.Zero)
				throw new Exception($"Failed to create renderer. SDL2Error({SDL.SDL_GetError()})");

			_ = SDL.SDL_SetRenderDrawBlendMode(RendererPtr, SDL.SDL_BlendMode.SDL_BLENDMODE_ADD);

			CurrentFont = SpriteCache.LoadFont(fontName);
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

		internal static int MouseWheel { get; set; }

		internal static bool HandleInput(Func<bool> handleInput)
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

		internal static bool KeyPressed(SDL.SDL_Scancode scanCode)
		{
			return CurrentKeys[(int)scanCode] == 1 && PreviousKeys[(int)scanCode] == 0;
		}

		internal static bool KeyReleased(SDL.SDL_Scancode scanCode)
		{
			return CurrentKeys[(int)scanCode] == 0 && PreviousKeys[(int)scanCode] == 1;
		}

		public static void SetWindowTitle(string title)
		{
			SDL.SDL_SetWindowTitle(WindowPtr, title);
		}

		internal static void ShowCursor(
			bool show = true)
		{
			_ = SDL.SDL_ShowCursor(show ? 1 : 0);
		}

		internal static v2i GetMouse(out uint mouseState)
		{
			mouseState = SDL.SDL_GetMouseState(out int mouseX, out int mouseY);
			return new(mouseX, mouseY);
		}

		/// <summary>
		/// display backbuffer
		/// </summary>
		internal static void Render()
		{
			SDL.SDL_RenderPresent(RendererPtr);
		}
	}
}
