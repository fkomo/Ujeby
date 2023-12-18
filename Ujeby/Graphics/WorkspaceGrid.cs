using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.Graphics
{
	public class WorkspaceGrid
	{
		/// <summary>
		/// workspace dragging with right mouse button
		/// </summary>
		public bool DragEnabled = true;

		/// <summary>
		/// size/step of minor grid lines
		/// </summary>
		public int MinorSize = 16;

		/// <summary>
		/// copunt of minor grid lines per major
		/// </summary>
		public int MajorSize = 10;

		/// <summary>
		/// mouse position in grid (from window center)
		/// </summary>
		public v2i MousePosition { get; private set; }

		/// <summary>
		/// MousePosition / MinorSize 
		/// </summary>
		public v2i MousePositionDiscrete => MousePosition / MinorSize;

		/// <summary>
		/// grid offset from window center
		/// </summary>
		public v2i Offset { get; private set; }

		public readonly v4f MinorColor = new(.06, .06, .06, 1);
		public readonly v4f MajorColor = new(.1, .1, .1, 1);
		public readonly v4f AxisColor = new(.5, .5, .5, 1);

		protected bool _drag { get; private set; } = false;
		protected v2i _dragStart { get; private set; }

		public void SetCenter(v2i v)
		{
			Offset = v.Inv();
		}

		public void MoveCenter(v2i v)
		{
			Offset -= v;
		}

		public void DragStart(v2i mousePosition)
		{
			if (DragEnabled)
			{
				_drag = true;
				_dragStart = mousePosition;
			}
		}

		public void UpdateMouse(v2i mousePosition, v2i windowSize)
		{
			if (DragEnabled && _drag)
			{
				Offset += mousePosition - _dragStart;
				_dragStart = mousePosition;
			}

			MousePosition = (mousePosition - windowSize / 2) - Offset;
		}

		public void DragEnd(v2i mousePosition)
		{
			if (DragEnabled)
			{
				_drag = false;
			}
		}

		public void Zoom(int value)
		{
			MinorSize = System.Math.Max(1, MinorSize + value);
		}

		public void DrawCell(int x, int y,
			v4f? border = null, v4f? fill = null)
		{
			DrawRect(
				x,
				y,
				1,
				1,
				border: border,
				fill: fill);
		}

		public void DrawCell(v2i p,
			v4f? border = null, v4f? fill = null)
		{
			DrawCell((int)p.X, (int)p.Y, border: border, fill: fill);
		}

		public void DrawCells(IEnumerable<v2i> cells,
			v4f? border = null, v4f? fill = null)
		{
			if (cells == null)
				return;

			foreach (var v in cells)
				DrawCell(v, border: border, fill: fill);
		}

		public void DrawCellsHeatPath(IEnumerable<v2i> path)
		{
			if (path != null)
			{
				var pa = path.ToArray();
				for (var i = 0; i < pa.Length; i++)
					DrawCell(pa[i], fill: HeatMap.GetColorForValue(i, pa.Length, .8));
			}
		}

		public void DrawRect(int x, int y, int w, int h,
			v4f? border = null, v4f? fill = null)
		{
			Sdl2Wrapper.DrawRect(
				(int)(Sdl2Wrapper.WindowSize.X / 2 + Offset.X + x * MinorSize) - MinorSize / 2,
				(int)(Sdl2Wrapper.WindowSize.Y / 2 + Offset.Y + y * MinorSize) - MinorSize / 2,
				w * MinorSize,
				h * MinorSize,
				border: border,
				fill: fill);
		}

		public void DrawRect(v2i topLeft, v2i size,
			v4f? border = null, v4f? fill = null)
		{
			DrawRect((int)topLeft.X, (int)topLeft.Y, (int)size.X, (int)size.Y, border: border, fill: fill);
		}

		public void DrawCircle(int x, int y, int radius,
			v4f? border = null, v4f? fill = null)
		{
			Sdl2Wrapper.DrawCircle(
				(int)(Sdl2Wrapper.WindowSize.X / 2 + Offset.X + x * MinorSize),
				(int)(Sdl2Wrapper.WindowSize.Y / 2 + Offset.Y + y * MinorSize),
				radius * MinorSize,
				border: border,
				fill: fill);
		}

		public void DrawCircle(v2i center, int radius,
			v4f? border = null, v4f? fill = null)
		{
			DrawCircle((int)center.X, (int)center.Y, radius, border: border, fill: fill);
		}

		public void DrawLine(int x1, int y1, int x2, int y2, v4f color)
		{
			Sdl2Wrapper.DrawLine(
				(int)(Sdl2Wrapper.WindowSize.X / 2 + Offset.X + x1 * MinorSize),
				(int)(Sdl2Wrapper.WindowSize.Y / 2 + Offset.Y + y1 * MinorSize),
				(int)(Sdl2Wrapper.WindowSize.X / 2 + Offset.X + x2 * MinorSize),
				(int)(Sdl2Wrapper.WindowSize.Y / 2 + Offset.Y + y2 * MinorSize),
				color);
		}

		public void DrawLine(v2i a, v2i b, v4f color)
		{
			DrawLine((int)a.X, (int)a.Y, (int)b.X, (int)b.Y, color);
		}

		public void Draw(
			bool showAxis = true, bool showMajor = true, bool showMinor = true)
		{
			var size = MinorSize;
			var origin = Sdl2Wrapper.WindowSize / 2 + Offset;

			if (showMinor)
			{
				for (var ix = 1; origin.X + ix * size < Sdl2Wrapper.WindowSize.X; ix++)
					Sdl2Wrapper.DrawLine((int)origin.X + ix * size, 0, (int)origin.X + ix * size, (int)Sdl2Wrapper.WindowSize.Y, MinorColor);

				for (var ix = 1; origin.X - ix * size >= 0; ix++)
					Sdl2Wrapper.DrawLine((int)origin.X - ix * size, 0, (int)origin.X - ix * size, (int)Sdl2Wrapper.WindowSize.Y, MinorColor);

				for (var iy = 1; origin.Y + iy * size < Sdl2Wrapper.WindowSize.Y; iy++)
					Sdl2Wrapper.DrawLine(0, (int)origin.Y + iy * size, (int)Sdl2Wrapper.WindowSize.X, (int)origin.Y + iy * size, MinorColor);

				for (var iy = 1; origin.Y - iy * size >= 0; iy++)
					Sdl2Wrapper.DrawLine(0, (int)origin.Y - iy * size, (int)Sdl2Wrapper.WindowSize.X, (int)origin.Y - iy * size, MinorColor);

				Sdl2Wrapper.DrawLine((int)origin.X, 0, (int)origin.X, (int)Sdl2Wrapper.WindowSize.Y, MinorColor);
				Sdl2Wrapper.DrawLine(0, (int)origin.Y, (int)Sdl2Wrapper.WindowSize.X, (int)origin.Y, MinorColor);
			}

			if (showMajor)
			{
				for (var ix = 1; origin.X + ix * size < Sdl2Wrapper.WindowSize.X; ix++)
				{
					if (ix % MajorSize != 0)
						continue;

					Sdl2Wrapper.DrawLine((int)origin.X + ix * size, 0, (int)origin.X + ix * size, (int)Sdl2Wrapper.WindowSize.Y, MajorColor);
				}

				for (var ix = 1; origin.X - ix * size >= 0; ix++)
				{
					if (ix % MajorSize != 0)
						continue;

					Sdl2Wrapper.DrawLine((int)origin.X - ix * size, 0, (int)origin.X - ix * size, (int)Sdl2Wrapper.WindowSize.Y, MajorColor);
				}

				for (var iy = 1; origin.Y + iy * size < Sdl2Wrapper.WindowSize.Y; iy++)
				{
					if (iy % MajorSize != 0)
						continue;

					Sdl2Wrapper.DrawLine(0, (int)origin.Y + iy * size, (int)Sdl2Wrapper.WindowSize.X, (int)origin.Y + iy * size, MajorColor);
				}

				for (var iy = 1; origin.Y - iy * size >= 0; iy++)
				{
					if (iy % MajorSize != 0)
						continue;

					Sdl2Wrapper.DrawLine(0, (int)origin.Y - iy * size, (int)Sdl2Wrapper.WindowSize.X, (int)origin.Y - iy * size, MajorColor);
				}
			}

			if (showAxis)
			{
				Sdl2Wrapper.DrawLine((int)origin.X, 0, (int)origin.X, (int)Sdl2Wrapper.WindowSize.Y, AxisColor);
				Sdl2Wrapper.DrawLine(0, (int)origin.Y, (int)Sdl2Wrapper.WindowSize.X, (int)origin.Y, AxisColor);
			}
		}

		public void DrawText(v2i position,
			params TextLine[] lines)
		{
			DrawText(
				position,
				new v2i(0),
				new v2i(2),
				HorizontalTextAlign.Left,
				VerticalTextAlign.Top,
				lines);
		}

		public void DrawText(v2i position, v2i spacing, v2i scale,
			params TextLine[] lines)
		{
			DrawText(
				position,
				spacing,
				scale,
				HorizontalTextAlign.Left,
				VerticalTextAlign.Top,
				lines);
		}

		public void DrawText(v2i position, HorizontalTextAlign alignH, VerticalTextAlign alignV,
			params TextLine[] lines)
		{
			DrawText(
				position,
				new v2i(0),
				new v2i(2),
				alignH,
				alignV,
				lines);
		}

		public void DrawText(v2i position, v2i spacing, v2i scale, HorizontalTextAlign alignH, VerticalTextAlign alignV,
			params TextLine[] lines)
		{
			var gridPosition = Sdl2Wrapper.WindowSize / 2 + Offset + position * MinorSize;

			Sdl2Wrapper.DrawText(
				Sdl2Wrapper.CurrentFont,
				gridPosition,
				spacing,
				scale,
				alignH,
				alignV,
				lines);
		}

		public void DrawMouseCursor(
			GridCursorStyles style = GridCursorStyles.Simple, bool printCoords = true)
		{
			var m = MousePositionDiscrete;

			switch (style)
			{
				case GridCursorStyles.Simple:
					DrawCell((int)m.X, (int)m.Y, border: new v4f(1, 1, 0, 1));
					break;

				case GridCursorStyles.SimpleFill:
					DrawCell((int)m.X, (int)m.Y, fill: new v4f(1, 1, 0, 0.5));
					break;

				case GridCursorStyles.FullRowColumn:
					{
						var cursorColor = new v4f(1, 1, 0, .1);

						Sdl2Wrapper.DrawRect(
							0,
							(int)(Sdl2Wrapper.WindowSize.Y / 2 + Offset.Y + m.Y * MinorSize) - MinorSize / 2,
							(int)Sdl2Wrapper.WindowSize.X,
							MinorSize,
							fill: cursorColor);

						Sdl2Wrapper.DrawRect(
							(int)(Sdl2Wrapper.WindowSize.X / 2 + Offset.X + m.X * MinorSize) - MinorSize / 2,
							0,
							MinorSize,
							(int)Sdl2Wrapper.WindowSize.Y,
							fill: cursorColor);
					}
					break;
			}

			if (printCoords)
			{
				var p = (MousePosition / MinorSize) * MinorSize + (Sdl2Wrapper.WindowSize / 2) + Offset;
				Sdl2Wrapper.DrawText(p + MinorSize * 2, null, new Text($"[{(int)m.X};{(int)m.Y}]"));
			}
		}
	}
}
