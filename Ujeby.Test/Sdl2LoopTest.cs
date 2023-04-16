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

		protected override void Init()
		{
			Sdl2Wrapper.ShowCursor(false); 
		}

		protected override void Render()
		{
			Grid.Draw();

			Grid.DrawText(new(0, 0), HorizontalTextAlign.Center, VerticalTextAlign.Center,
				new Text($"[-/-]  [+/-]") { Color = new(.5) },
				Text.EmptyLine,
				Text.EmptyLine,
				new Text($"[-/+]  [+/+]") { Color = new(.5) });

			Sdl2Wrapper.DrawCircle(0, 0, 500, border: new(1, 1, 0, .8)/*, fill: new(.5, .5, 0, .8)*/);

			Grid.DrawCircle(-15, 10, 10, border: new(.5, 0, .5, 1), fill: new(.5, 0, .5, 1));
			Grid.DrawCircle(-10, -20, 5, border: new(.5, 0, .5, 1)/*, fill: new(.5, 0, .5, 1)*/);
			Grid.DrawText(new(-10, -20), HorizontalTextAlign.Center, VerticalTextAlign.Center, new Text($"circle"));

			Grid.DrawCell(10, -20, border: new(1, 0, 0, .8), fill: new(.5, 0, 0, .8));

			Grid.DrawRect(5, 10, 30, 15, border: new(0, 1, 0, .8), fill: new(0, .5, 0, .8));

			Sdl2Wrapper.DrawRect(300, 400, 200, 100, border: new(0, 0, 1, .8), fill: new(0, 0, .5, .8));

			Grid.DrawLine(-10, -20, -15, 10, new(1, 0, 1, 1));

			Sdl2Wrapper.DrawText(new(32, 32), 
				new Text($"{nameof(Fps)}: {(int)Fps}") { Color = new(1, 1, 0, 1) },
				new EmptyLine(),
				new Text($"{nameof(_frameTime)}: {_frameTime}ms"),
				new Text($"{nameof(_frameCount)}: {_frameCount}"),
				new Text($"{nameof(_mouseState)}: {_mouseState}"),
				new Text($"{nameof(_mouseLeft)}: {_mouseLeft}"),
				new Text($"{nameof(_mouseRight)}: {_mouseRight}"),
				new Text($"{nameof(WindowSize)}: {WindowSize}"),
				new Text($"{nameof(Grid.Offset)}: {Grid.Offset}"),
				new Text($"{nameof(Grid.MousePosition)}: {Grid.MousePosition}"),
				new Text($"{nameof(MousePosition)}: {MousePosition}")
				);

			Grid.DrawMouseCursor(
				style: GridCursorStyles.FullRowColumn);
		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}

		protected override void LeftMouseDown()
		{

		}

		protected override void LeftMouseUp()
		{

		}

		internal void Run()
		{
			Run(HandleUserInput);
		}
	}
}
