using SDL2;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl.Interfaces;
using Ujeby.Vectors;

namespace Ujeby.Graphics.Sdl
{
	public abstract class Sdl2Loop : IRunnable
	{
		/// <summary>
		/// workspace dragging with right mouse button
		/// </summary>
		public bool DragEnabled = true;

		/// <summary>
		/// size/step of minor grid lines
		/// </summary>
		public int MinorGridSize = 16;

		/// <summary>
		/// copunt of minor grid lines per major
		/// </summary>
		public int MajorGridSize = 10;

		/// <summary>
		/// mouse position in window (from top-left)
		/// </summary>
		protected v2i MouseWindowPosition;

		/// <summary>
		/// mouse position in grid (from window center)
		/// </summary>
		protected v2i MouseGridPosition;

		/// <summary>
		/// MouseGridPosition / GridSize 
		/// </summary>
		protected v2i MouseGridPositionDiscrete => MouseGridPosition / MinorGridSize;

		/// <summary>
		/// grid offset from window center
		/// </summary>
		protected v2i GridOffset;

		protected readonly v2i WindowSize;

		protected uint _mouseState;
		protected bool _mouseLeft;
		protected bool _mouseRight;

		protected string Title;

		protected bool _terminate;

		protected Sdl2Loop(v2i windowSize)
		{
			WindowSize = windowSize;
		}

		protected bool _drag = false;
		protected v2i _dragStart;

		public void Run(Func<bool> handleInput)
		{
			Init();

			while (Sdl2Wrapper.HandleInput(handleInput) && !_terminate)
			{
				MouseWindowPosition = Sdl2Wrapper.GetMouse(out _mouseState);

				// mouse right
				var right = (_mouseState & 4) == 4;
				if (DragEnabled)
				{
					if (right && !_mouseRight)
					{
						_drag = true;
						_dragStart = MouseWindowPosition;
					}
					else if (!right && _mouseRight)
					{
						_drag = false;
					}
					else if (_drag)
					{
						GridOffset += MouseWindowPosition - _dragStart;
						_dragStart = MouseWindowPosition;
					}
				}
				_mouseRight = right;

				MouseGridPosition = (MouseWindowPosition - WindowSize / 2) - GridOffset;

				// mouse left
				var left = (_mouseState & 1) == 1;
				if (left)
					LeftMouseDown();
				else if (!left && _mouseLeft)
					LeftMouseUp();
				_mouseLeft = left;

				MinorGridSize = Math.Max(2, MinorGridSize + Sdl2Wrapper.MouseWheel);
				Sdl2Wrapper.MouseWheel = 0;

				Update();

				// clear backbuffer
				Sdl2Renderer.Clear(_bgColor);

				Render();

				if (Title != null)
					Sdl2Wrapper.SetWindowTitle(Title);

				Sdl2Wrapper.Render();
			}

			Destroy();
		}

		protected virtual void LeftMouseDown()
		{
		}

		protected virtual void LeftMouseUp()
		{
		}

		protected abstract void Init();
		protected abstract void Update();
		protected abstract void Render();
		protected abstract void Destroy();

		protected void DrawRect(int x, int y, int w, int h,
			v4f? border = null, v4f? fill = null)
		{
			Sdl2Renderer.DrawRect(
				x,
				y,
				w,
				h,
				border: border,
				fill: fill);
		}

		protected void DrawGridCell(int x, int y, 
			v4f? border = null, v4f? fill = null)
		{
			DrawGridRect(
				x, 
				y, 
				1, 
				1,
				border: border,
				fill: fill);
		}

		protected void DrawGridRect(int x, int y, int w, int h,
			v4f? border = null, v4f? fill = null)
		{
			DrawRect(
				(int)(WindowSize.X / 2 + GridOffset.X + x * MinorGridSize) - MinorGridSize / 2,
				(int)(WindowSize.Y / 2 + GridOffset.Y + y * MinorGridSize) - MinorGridSize / 2,
				w * MinorGridSize,
				h * MinorGridSize,
				border: border,
				fill: fill);
		}

		private v4f _bgColor = new(0.05, 0.05, 0.05, 1);
		private v4f _minorColor = new(0.06, 0.06, 0.06, 1);
		private v4f _majorColor = new(0.1, 0.1, 0.1, 1);
		private v4f _axisColor = new(0.5, 0.5, 0.5, 1);

		protected void DrawGrid(
			bool showAxis = true, bool showMajor = true, bool showMinor = true)
		{
			var size = MinorGridSize;
			var origin = WindowSize / 2 + GridOffset;

			if (showMinor)
			{
				for (var ix = 1; origin.X + ix * size < WindowSize.X; ix++)
					Sdl2Renderer.DrawLine((int)origin.X + ix * size, 0, (int)origin.X + ix * size, (int)WindowSize.Y, _minorColor);

				for (var ix = 1; origin.X - ix * size >= 0; ix++)
					Sdl2Renderer.DrawLine((int)origin.X - ix * size, 0, (int)origin.X - ix * size, (int)WindowSize.Y, _minorColor);

				for (var iy = 1; origin.Y + iy * size < WindowSize.Y; iy++)
					Sdl2Renderer.DrawLine(0, (int)origin.Y + iy * size, (int)WindowSize.X, (int)origin.Y + iy * size, _minorColor);

				for (var iy = 1; origin.Y - iy * size >= 0; iy++)
					Sdl2Renderer.DrawLine(0, (int)origin.Y - iy * size, (int)WindowSize.X, (int)origin.Y - iy * size, _minorColor);

				Sdl2Renderer.DrawLine((int)origin.X, 0, (int)origin.X, (int)WindowSize.Y, _minorColor);
				Sdl2Renderer.DrawLine(0, (int)origin.Y, (int)WindowSize.X, (int)origin.Y, _minorColor);
			}

			if (showMajor)
			{
				for (var ix = 1; origin.X + ix * size < WindowSize.X; ix++)
				{
					if (ix % MajorGridSize != 0)
						continue;

					Sdl2Renderer.DrawLine((int)origin.X + ix * size, 0, (int)origin.X + ix * size, (int)WindowSize.Y, _majorColor);
				}

				for (var ix = 1; origin.X - ix * size >= 0; ix++)
				{
					if (ix % MajorGridSize != 0)
						continue;

					Sdl2Renderer.DrawLine((int)origin.X - ix * size, 0, (int)origin.X - ix * size, (int)WindowSize.Y, _majorColor);
				}

				for (var iy = 1; origin.Y + iy * size < WindowSize.Y; iy++)
				{
					if (iy % MajorGridSize != 0)
						continue;

					Sdl2Renderer.DrawLine(0, (int)origin.Y + iy * size, (int)WindowSize.X, (int)origin.Y + iy * size, _majorColor);
				}

				for (var iy = 1; origin.Y - iy * size >= 0; iy++)
				{
					if (iy % MajorGridSize != 0)
						continue;

					Sdl2Renderer.DrawLine(0, (int)origin.Y - iy * size, (int)WindowSize.X, (int)origin.Y - iy * size, _majorColor);
				}
			}

			if (showAxis)
			{
				Sdl2Renderer.DrawLine((int)origin.X, 0, (int)origin.X, (int)WindowSize.Y, _axisColor);
				Sdl2Renderer.DrawLine(0, (int)origin.Y, (int)WindowSize.X, (int)origin.Y, _axisColor);
			}
		}

		protected void DrawGridMouseCursor(
			GridCursorStyles style = GridCursorStyles.Simple, bool printCoords = true)
		{
			var m = MouseGridPositionDiscrete;

			switch (style)
			{
				case GridCursorStyles.Simple:
					DrawGridCell((int)m.X, (int)m.Y, border: new v4f(1, 1, 0, 1));
					break;

				case GridCursorStyles.SimpleFill:
					DrawGridCell((int)m.X, (int)m.Y, fill: new v4f(1, 1, 0, 0.5));
					break;

				case GridCursorStyles.FullRowColumn:
					{
						var cursorColor = new v4f(1, 1, 0, 0.1);

						DrawRect(
							0,
							(int)(WindowSize.Y / 2 + GridOffset.Y + m.Y * MinorGridSize) - MinorGridSize / 2,
							(int)WindowSize.X,
							MinorGridSize,
							fill: cursorColor);

						DrawRect(
							(int)(WindowSize.X / 2 + GridOffset.X + m.X * MinorGridSize) - MinorGridSize / 2,
							0,
							MinorGridSize,
							(int)WindowSize.Y,
							fill: cursorColor);
					}
					break;
			}

			if (printCoords)
			{
				var p = (MouseWindowPosition / MinorGridSize) * MinorGridSize;
				DrawText(p + MinorGridSize * 2, v2i.Zero, new Text($"[{(int)m.X};{(int)m.Y}]"));
			}
		}

		protected void DrawText(v2i position, v2i spacing, params TextLine[] lines)
		{
			Sdl2Renderer.DrawText(
				Sdl2Wrapper.CurrentFont,
				position,
				spacing,
				new v2i(2),
				lines);
		}

		protected void ShowCursor(
			bool show = true)
		{
			Sdl2Wrapper.ShowCursor(show);
		}

		protected void SetGridCenter(v2i v)
		{
			GridOffset = v.Inv();
		}

		protected void MoveGridCenter(v2i v)
		{
			GridOffset -= v;
		}
	}
}
