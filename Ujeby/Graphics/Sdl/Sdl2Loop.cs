using System.Diagnostics;
using Ujeby.Graphics.Entities;
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

		public void Run(Func<bool> handleInput)
		{
			Init();

			while (Sdl2Wrapper.HandleInput(handleInput) && !_terminate)
			{
				_frameSw.Restart();

				MousePosition = Sdl2Wrapper.GetMouse(out _mouseState);

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

				if (Sdl2Wrapper.MouseWheel != 0)
				{
					Grid.Zoom(Sdl2Wrapper.MouseWheel);
					Sdl2Wrapper.MouseWheel = 0;
				}

				Update();

				// clear backbuffer
				Sdl2Renderer.Clear(_bgColor);

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
				(int)(WindowSize.X / 2 + Grid.Offset.X + x * Grid.MinorSize) - Grid.MinorSize / 2,
				(int)(WindowSize.Y / 2 + Grid.Offset.Y + y * Grid.MinorSize) - Grid.MinorSize / 2,
				w * Grid.MinorSize,
				h * Grid.MinorSize,
				border: border,
				fill: fill);
		}

		protected void DrawGridLine(int x1, int y1, int x2, int y2, v4f color)
		{
			Sdl2Renderer.DrawLine(
				(int)(WindowSize.X / 2 + Grid.Offset.X + x1 * Grid.MinorSize),
				(int)(WindowSize.Y / 2 + Grid.Offset.Y + y1 * Grid.MinorSize),
				(int)(WindowSize.X / 2 + Grid.Offset.X + x2 * Grid.MinorSize),
				(int)(WindowSize.Y / 2 + Grid.Offset.Y + y2 * Grid.MinorSize),
				color);
		}

		protected void DrawGrid(
			bool showAxis = true, bool showMajor = true, bool showMinor = true)
		{
			var size = Grid.MinorSize;
			var origin = WindowSize / 2 + Grid.Offset;

			if (showMinor)
			{
				for (var ix = 1; origin.X + ix * size < WindowSize.X; ix++)
					Sdl2Renderer.DrawLine((int)origin.X + ix * size, 0, (int)origin.X + ix * size, (int)WindowSize.Y, Grid.MinorColor);

				for (var ix = 1; origin.X - ix * size >= 0; ix++)
					Sdl2Renderer.DrawLine((int)origin.X - ix * size, 0, (int)origin.X - ix * size, (int)WindowSize.Y, Grid.MinorColor);

				for (var iy = 1; origin.Y + iy * size < WindowSize.Y; iy++)
					Sdl2Renderer.DrawLine(0, (int)origin.Y + iy * size, (int)WindowSize.X, (int)origin.Y + iy * size, Grid.MinorColor);

				for (var iy = 1; origin.Y - iy * size >= 0; iy++)
					Sdl2Renderer.DrawLine(0, (int)origin.Y - iy * size, (int)WindowSize.X, (int)origin.Y - iy * size, Grid.MinorColor);

				Sdl2Renderer.DrawLine((int)origin.X, 0, (int)origin.X, (int)WindowSize.Y, Grid.MinorColor);
				Sdl2Renderer.DrawLine(0, (int)origin.Y, (int)WindowSize.X, (int)origin.Y, Grid.MinorColor);
			}

			if (showMajor)
			{
				for (var ix = 1; origin.X + ix * size < WindowSize.X; ix++)
				{
					if (ix % Grid.MajorSize != 0)
						continue;

					Sdl2Renderer.DrawLine((int)origin.X + ix * size, 0, (int)origin.X + ix * size, (int)WindowSize.Y, Grid.MajorColor);
				}

				for (var ix = 1; origin.X - ix * size >= 0; ix++)
				{
					if (ix % Grid.MajorSize != 0)
						continue;

					Sdl2Renderer.DrawLine((int)origin.X - ix * size, 0, (int)origin.X - ix * size, (int)WindowSize.Y, Grid.MajorColor);
				}

				for (var iy = 1; origin.Y + iy * size < WindowSize.Y; iy++)
				{
					if (iy % Grid.MajorSize != 0)
						continue;

					Sdl2Renderer.DrawLine(0, (int)origin.Y + iy * size, (int)WindowSize.X, (int)origin.Y + iy * size, Grid.MajorColor);
				}

				for (var iy = 1; origin.Y - iy * size >= 0; iy++)
				{
					if (iy % Grid.MajorSize != 0)
						continue;

					Sdl2Renderer.DrawLine(0, (int)origin.Y - iy * size, (int)WindowSize.X, (int)origin.Y - iy * size, Grid.MajorColor);
				}
			}

			if (showAxis)
			{
				Sdl2Renderer.DrawLine((int)origin.X, 0, (int)origin.X, (int)WindowSize.Y, Grid.AxisColor);
				Sdl2Renderer.DrawLine(0, (int)origin.Y, (int)WindowSize.X, (int)origin.Y, Grid.AxisColor);
			}
		}

		protected void DrawGridMouseCursor(
			GridCursorStyles style = GridCursorStyles.Simple, bool printCoords = true)
		{
			var m = Grid.MousePositionDiscrete;

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
						var cursorColor = new v4f(1, 1, 0, .1);

						DrawRect(
							0,
							(int)(WindowSize.Y / 2 + Grid.Offset.Y + m.Y * Grid.MinorSize) - Grid.MinorSize / 2,
							(int)WindowSize.X,
							Grid.MinorSize,
							fill: cursorColor);

						DrawRect(
							(int)(WindowSize.X / 2 + Grid.Offset.X + m.X * Grid.MinorSize) - Grid.MinorSize / 2,
							0,
							Grid.MinorSize,
							(int)WindowSize.Y,
							fill: cursorColor);
					}
					break;
			}

			if (printCoords)
			{
				var p = (MousePosition / Grid.MinorSize) * Grid.MinorSize;
				DrawText(p + Grid.MinorSize * 2, v2i.Zero, new Text($"[{(int)m.X};{(int)m.Y}]"));
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
	}
}
