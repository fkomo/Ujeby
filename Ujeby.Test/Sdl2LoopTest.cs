using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.Test
{
	internal class Sdl2LoopTest : Sdl2Loop
	{
		public override string Name => nameof(Sdl2LoopTest);

		public Sdl2LoopTest(v2i windowSize) : base(windowSize)
		{
		}

		public override void Init()
		{
			ShowCursor(false); 
		}

		public override void Update()
		{
		}

		public override void Render()
		{
			DrawGrid();

			DrawGridCell(10, -20, border: new v4f(1, 0, 0, .8), fill: new v4f(.5, 0, 0, .8));

			DrawGridRect(5, 10, 30, 15, border: new v4f(0, 1, 0, .8), fill: new v4f(0, .5, 0, .8));

			DrawRect(300, 400, 200, 100, border: new v4f(0, 0, 1, .8), fill: new v4f(0, 0, .5, .8));

			DrawGridLine(-10, -20, -15, 10, new v4f(1, 0, 1, 1));

			DrawText(new(32, 32), new(0, 2),
				new Text($"{nameof(Fps)}: {(int)Fps}") { Color = new v4f(1, 1, 0, 1) },
				new EmptyLine(),
				new Text($"{nameof(_frameTime)}: {_frameTime}ms"),
				new Text($"{nameof(_frameCount)}: {_frameCount}"),
				new Text($"{nameof(_mouseState)}: {_mouseState}"),
				new Text($"{nameof(_mouseLeft)}: {_mouseLeft}"),
				new Text($"{nameof(_mouseRight)}: {_mouseRight}"),
				new Text($"{nameof(WindowSize)}: {WindowSize}"),
				new Text($"{nameof(GridOffset)}: {GridOffset}"),
				new Text($"{nameof(MouseGridPosition)}: {MouseGridPosition}"),
				new Text($"{nameof(MouseWindowPosition)}: {MouseWindowPosition}")
				);

			DrawGridMouseCursor(
				style: GridCursorStyles.FullRowColumn);
		}

		public override void Destroy()
		{
			ShowCursor();
		}

		protected override void LeftMouseDown()
		{

		}

		protected override void LeftMouseUp()
		{

		}
	}
}
