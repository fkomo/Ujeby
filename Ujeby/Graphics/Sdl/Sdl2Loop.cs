using SDL2;
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
				Clear(_bgColor);

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

		protected void Clear(v4f color)
		{
			var bColor = color * 255;
			_ = SDL.SDL_SetRenderDrawColor(Sdl2Wrapper.RendererPtr, (byte)bColor.X, (byte)bColor.Y, (byte)bColor.Z, (byte)bColor.W);
			_ = SDL.SDL_RenderClear(Sdl2Wrapper.RendererPtr);
		}

		protected void DrawRect(int x, int y, int w, int h,
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

		protected void DrawLine(int x1, int y1, int x2, int y2, v4f color)
		{
			var bColor = color * 255;
			_ = SDL.SDL_SetRenderDrawColor(Sdl2Wrapper.RendererPtr, (byte)bColor.X, (byte)bColor.Y, (byte)bColor.Z, (byte)bColor.W);
			_ = SDL.SDL_RenderDrawLine(Sdl2Wrapper.RendererPtr, x1, y1, x2, y2);
		}

		protected void DrawCircle(int cx, int cy, int radius, 
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

		protected void DrawText(Font font, v2i position, v2i spacing, v2i scale, HorizontalTextAlign alignH, VerticalTextAlign alignV,
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

		protected void DrawText(v2i topLeft,
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

		protected void DrawText(v2i topLeft, v2i spacing, v2i scale,
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

		protected void DrawText(v2i topLeft, HorizontalTextAlign alignH, VerticalTextAlign alignV,
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

		protected void DrawText(v2i position, v2i spacing, v2i scale, HorizontalTextAlign alignH, VerticalTextAlign alignV, 
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

		protected void DrawGridCell(v2i p,
			v4f? border = null, v4f? fill = null)
		{
			DrawGridCell((int)p.X, (int)p.Y, border: border, fill: fill);
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

		protected void DrawGridRect(v2i topLeft, v2i size,
			v4f? border = null, v4f? fill = null)
		{
			DrawGridRect((int)topLeft.X, (int)topLeft.Y, (int)size.X, (int)size.Y, border: border, fill: fill);
		}

		protected void DrawGridCircle(int x, int y, int radius,
			v4f? border = null, v4f? fill = null)
		{
			DrawCircle(
				(int)(WindowSize.X / 2 + Grid.Offset.X + x * Grid.MinorSize),
				(int)(WindowSize.Y / 2 + Grid.Offset.Y + y * Grid.MinorSize),
				radius * Grid.MinorSize, 
				border: border,
				fill: fill);
		}

		protected void DrawGridCircle(v2i center, int radius, 
			v4f? border = null, v4f? fill = null)
		{
			DrawGridCircle((int)center.X, (int)center.Y, radius, border: border, fill: fill);
		}

		protected void DrawGridLine(int x1, int y1, int x2, int y2, v4f color)
		{
			DrawLine(
				(int)(WindowSize.X / 2 + Grid.Offset.X + x1 * Grid.MinorSize),
				(int)(WindowSize.Y / 2 + Grid.Offset.Y + y1 * Grid.MinorSize),
				(int)(WindowSize.X / 2 + Grid.Offset.X + x2 * Grid.MinorSize),
				(int)(WindowSize.Y / 2 + Grid.Offset.Y + y2 * Grid.MinorSize),
				color);
		}

		protected void DrawGridLine(v2i a, v2i b, v4f color)
		{
			DrawLine((int)a.X, (int)a.Y, (int)b.X, (int)b.Y, color);
		}

		protected void DrawGrid(
			bool showAxis = true, bool showMajor = true, bool showMinor = true)
		{
			var size = Grid.MinorSize;
			var origin = WindowSize / 2 + Grid.Offset;

			if (showMinor)
			{
				for (var ix = 1; origin.X + ix * size < WindowSize.X; ix++)
					DrawLine((int)origin.X + ix * size, 0, (int)origin.X + ix * size, (int)WindowSize.Y, Grid.MinorColor);

				for (var ix = 1; origin.X - ix * size >= 0; ix++)
					DrawLine((int)origin.X - ix * size, 0, (int)origin.X - ix * size, (int)WindowSize.Y, Grid.MinorColor);

				for (var iy = 1; origin.Y + iy * size < WindowSize.Y; iy++)
					DrawLine(0, (int)origin.Y + iy * size, (int)WindowSize.X, (int)origin.Y + iy * size, Grid.MinorColor);

				for (var iy = 1; origin.Y - iy * size >= 0; iy++)
					DrawLine(0, (int)origin.Y - iy * size, (int)WindowSize.X, (int)origin.Y - iy * size, Grid.MinorColor);

				DrawLine((int)origin.X, 0, (int)origin.X, (int)WindowSize.Y, Grid.MinorColor);
				DrawLine(0, (int)origin.Y, (int)WindowSize.X, (int)origin.Y, Grid.MinorColor);
			}

			if (showMajor)
			{
				for (var ix = 1; origin.X + ix * size < WindowSize.X; ix++)
				{
					if (ix % Grid.MajorSize != 0)
						continue;

					DrawLine((int)origin.X + ix * size, 0, (int)origin.X + ix * size, (int)WindowSize.Y, Grid.MajorColor);
				}

				for (var ix = 1; origin.X - ix * size >= 0; ix++)
				{
					if (ix % Grid.MajorSize != 0)
						continue;

					DrawLine((int)origin.X - ix * size, 0, (int)origin.X - ix * size, (int)WindowSize.Y, Grid.MajorColor);
				}

				for (var iy = 1; origin.Y + iy * size < WindowSize.Y; iy++)
				{
					if (iy % Grid.MajorSize != 0)
						continue;

					DrawLine(0, (int)origin.Y + iy * size, (int)WindowSize.X, (int)origin.Y + iy * size, Grid.MajorColor);
				}

				for (var iy = 1; origin.Y - iy * size >= 0; iy++)
				{
					if (iy % Grid.MajorSize != 0)
						continue;

					DrawLine(0, (int)origin.Y - iy * size, (int)WindowSize.X, (int)origin.Y - iy * size, Grid.MajorColor);
				}
			}

			if (showAxis)
			{
				DrawLine((int)origin.X, 0, (int)origin.X, (int)WindowSize.Y, Grid.AxisColor);
				DrawLine(0, (int)origin.Y, (int)WindowSize.X, (int)origin.Y, Grid.AxisColor);
			}
		}

		protected void DrawGridText(v2i position,
			params TextLine[] lines)
		{
			DrawGridText(
				position,
				new v2i(0),
				new v2i(2),
				HorizontalTextAlign.Left,
				VerticalTextAlign.Top,
				lines);
		}

		protected void DrawGridText(v2i position, v2i spacing, v2i scale,
			params TextLine[] lines)
		{
			DrawGridText(
				position,
				spacing,
				scale,
				HorizontalTextAlign.Left,
				VerticalTextAlign.Top,
				lines);
		}

		protected void DrawGridText(v2i position, HorizontalTextAlign alignH, VerticalTextAlign alignV,
			params TextLine[] lines)
		{
			DrawGridText(
				position,
				new v2i(0),
				new v2i(2),
				alignH,
				alignV,
				lines);
		}

		protected void DrawGridText(v2i position, v2i spacing, v2i scale, HorizontalTextAlign alignH, VerticalTextAlign alignV, 
			params TextLine[] lines)
		{
			var gridPosition = WindowSize / 2 + Grid.Offset + position * Grid.MinorSize;

			DrawText(
				Sdl2Wrapper.CurrentFont,
				gridPosition,
				spacing,
				scale,
				alignH,
				alignV,
				lines);
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
				DrawText(p + Grid.MinorSize * 2, new Text($"[{(int)m.X};{(int)m.Y}]"));
			}
		}

		protected void ShowCursor(
			bool show = true)
		{
			Sdl2Wrapper.ShowCursor(show);
		}
	}
}
