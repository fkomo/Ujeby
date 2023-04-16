using SDL2;
using Ujeby.Vectors;

namespace Ujeby.Game.Sdl
{
	public static class Sdl2InputWrapper
	{
		private static readonly byte[] _currentKeys = new byte[(int)SDL.SDL_Scancode.SDL_NUM_SCANCODES];
		private static readonly byte[] _previousKeys = new byte[(int)SDL.SDL_Scancode.SDL_NUM_SCANCODES];

		public static int MouseWheel { get; set; }

		public static readonly Dictionary<InputButton, SDL.SDL_Scancode> _defaultKeyMappings = new()
		{
			{ InputButton.DPadUp, SDL.SDL_Scancode.SDL_SCANCODE_W },
			{ InputButton.DPadDown, SDL.SDL_Scancode.SDL_SCANCODE_S },
			{ InputButton.DPadLeft, SDL.SDL_Scancode.SDL_SCANCODE_A },
			{ InputButton.DPadRight, SDL.SDL_Scancode.SDL_SCANCODE_D },
			{ InputButton.LT, SDL.SDL_Scancode.SDL_SCANCODE_LSHIFT },
			{ InputButton.RT, SDL.SDL_Scancode.SDL_SCANCODE_RSHIFT },
			{ InputButton.LB, SDL.SDL_Scancode.SDL_SCANCODE_U },
			{ InputButton.RB, SDL.SDL_Scancode.SDL_SCANCODE_I },
			{ InputButton.X, SDL.SDL_Scancode.SDL_SCANCODE_J },
			{ InputButton.Y, SDL.SDL_Scancode.SDL_SCANCODE_K },
			{ InputButton.A, SDL.SDL_Scancode.SDL_SCANCODE_N },
			{ InputButton.B, SDL.SDL_Scancode.SDL_SCANCODE_M },
			{ InputButton.Menu, SDL.SDL_Scancode.SDL_SCANCODE_ESCAPE },
		};

		public static bool HandleInput(Func<InputButton, InputButtonState, bool> handleInput, 
			Dictionary<InputButton, SDL.SDL_Scancode> keyMappings = null)
		{
			keyMappings ??= _defaultKeyMappings;

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
					_currentKeys.CopyTo(_previousKeys, 0);
					var keysBuffer = SDL.SDL_GetKeyboardState(out int keysBufferLength);
					System.Runtime.InteropServices.Marshal.Copy(keysBuffer, _currentKeys, 0, keysBufferLength);

					// handle user input
					foreach (var btn in keyMappings)
					{
						var result = true;
						if (KeyPressed(btn.Value))
							result = handleInput(btn.Key, InputButtonState.Pressed);

						if (KeyReleased(btn.Value))
							result = handleInput(btn.Key, InputButtonState.Released);

						if (!result)
							return false;
					}
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
			=> _currentKeys[(int)scanCode] == 1 && _previousKeys[(int)scanCode] == 0;

		public static bool KeyReleased(SDL.SDL_Scancode scanCode)
			=> _currentKeys[(int)scanCode] == 0 && _previousKeys[(int)scanCode] == 1;

		public static v2i GetMouse(out uint mouseState)
		{
			mouseState = SDL.SDL_GetMouseState(out int mouseX, out int mouseY);
			return new(mouseX, mouseY);
		}
	}
}
