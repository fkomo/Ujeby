using System.Diagnostics;
using Ujeby.Game.Sdl;
using Ujeby.Graphics.Interfaces;
using Ujeby.Vectors;

namespace Ujeby.Graphics.Sdl
{
	public abstract class Sdl2Loop : IRunnable
	{
		public abstract string Name { get; }

		/// <summary>
		/// mouse position in window (from top-left)
		/// </summary>
		protected v2i MousePosition;

		protected readonly v2i WindowSize;

		protected uint _mouseState;
		protected bool _mouseLeft;
		protected bool _mouseRight;

		protected bool _terminate;

		protected string Title;

		private readonly v4f _bgColor = new(.05, .05, .05, 1);

		protected WorkspaceGrid Grid;

		private Stopwatch _frameSw = new();
		protected double _frameTime { get; private set; }
		protected long _frameCount { get; private set; }

		protected const long _fpsUpdate = 60;

		protected double Fps { get; private set; }

		protected Sdl2Loop(v2i windowSize)
		{
			WindowSize = windowSize;

			Grid = new WorkspaceGrid()
			{
				DragEnabled = true,
			};
		}

		public void Run(Action<InputButton, InputButtonState> handleInput)
		{
			Init();

			while (Sdl2InputWrapper.HandleInput(handleInput) && !_terminate)
			{
				_frameSw.Restart();

				MousePosition = Sdl2InputWrapper.GetMouse(out _mouseState);

				// mouse right
				var right = (_mouseState & 4) == 4;
				if (right && !_mouseRight)
					Grid.DragStart(MousePosition);

				else if (!right && _mouseRight)
					Grid.DragEnd(MousePosition);

				Grid.UpdateMouse(MousePosition, WindowSize);

				_mouseRight = right;

				// mouse left
				var left = (_mouseState & 1) == 1;
				if (left)
					LeftMouseDown();
				else if (!left && _mouseLeft)
					LeftMouseUp();
				_mouseLeft = left;

				if (Sdl2InputWrapper.MouseWheel != 0)
				{
					Grid.Zoom(Sdl2InputWrapper.MouseWheel);
					Sdl2InputWrapper.MouseWheel = 0;
				}

				Update();

				// clear backbuffer
				Sdl2Wrapper.Clear(_bgColor);

				Render();

				if (Title != null)
					Sdl2Wrapper.SetWindowTitle(Title);

				Sdl2Wrapper.Render();

				_frameSw.Stop();
				_frameTime = _frameSw.ElapsedMilliseconds;
				if (_frameCount % _fpsUpdate == 0)
					Fps = 1000 / _frameTime;

				_frameCount++;
			}

			Destroy();
		}

		protected virtual void LeftMouseDown()
		{
		}

		protected virtual void LeftMouseUp()
		{
		}

		protected virtual void Init()
		{
		}

		protected virtual void Update()
		{
		}

		protected virtual void Render()
		{
		}

		protected virtual void Destroy()
		{
		}

		protected virtual void HandleUserInput(InputButton btn, InputButtonState btnState)
		{
			if (btn == InputButton.Start && btnState == InputButtonState.Pressed)
				_terminate = true;
		}
	}
}
