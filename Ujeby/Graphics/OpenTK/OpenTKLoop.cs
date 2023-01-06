using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using Ujeby.Vectors;

namespace Ujeby.Graphics.OpenTK
{
	public abstract class OpenTKLoop : GameWindow
	{
		public abstract string Name { get; }

		/// <summary>
		/// mouse position in window (from top-left)
		/// </summary>
		protected new v2i MousePosition;

		protected readonly v2i WindowSize;

		protected bool _terminate;

		protected WorkspaceGrid Grid;

		private readonly v4f _bgColor = new(0.05, 0.05, 0.05, 1);

		protected bool _drag { get; private set; } = false;
		protected v2i _dragStart { get; private set; }

		private Stopwatch _frameSw = new();
		protected double _frameTime { get; private set; }
		protected long _frameCount { get; private set; }

		protected const long _fpsUpdate = 60;

		protected double Fps { get; private set; }

		protected OpenTKLoop(v2i windowSize, string title) : base(GameWindowSettings.Default, 
			new NativeWindowSettings
			{
				Title = title,
				Size = new Vector2i((int)windowSize.X, (int)windowSize.Y),
				WindowBorder = WindowBorder.Hidden,
			})
		{
			WindowSize = windowSize;

			Grid = new WorkspaceGrid()
			{
				DragEnabled = true,
			};
		}

		protected override void OnLoad()
		{
			base.OnLoad();

			GL.ClearColor((float)_bgColor.X, (float)_bgColor.Y, (float)_bgColor.Z, (float)_bgColor.W);

			Init();
		}

		protected override void OnUpdateFrame(FrameEventArgs args)
		{
			base.OnUpdateFrame(args);

			var input = KeyboardState;

			if (input.IsKeyDown(Keys.Escape) || _terminate)
			{
				Close();
			}

			MousePosition = new v2i((long)MouseState.Position.X, (long)MouseState.Position.Y);

			// mouse right
			var right = MouseState.IsButtonDown(MouseButton.Right);
			if (right && !MouseState.WasButtonDown(MouseButton.Right))
				Grid.DragStart(MousePosition);

			else if (!right && MouseState.WasButtonDown(MouseButton.Right))
				Grid.DragEnd(MousePosition);

			Grid.UpdateMouse(MousePosition, WindowSize);

			// mouse left
			var left = MouseState.IsButtonDown(MouseButton.Left);
			if (left)
				LeftMouseDown();
			else if (!left && MouseState.WasButtonDown(MouseButton.Left))
				LeftMouseUp();

			Update();
		}

		protected override void OnRenderFrame(FrameEventArgs args)
		{
			base.OnRenderFrame(args);

			// clear backbuffer
			GL.Clear(ClearBufferMask.ColorBufferBit);

			Render();

			SwapBuffers();

			_frameTime = _frameSw.ElapsedMilliseconds;
			if (_frameCount % _fpsUpdate == 0)
				Fps = 1000 / _frameTime;

			_frameCount++;
			_frameSw.Restart();
		}

		protected override void OnResize(ResizeEventArgs e)
		{
			base.OnResize(e);

			GL.Viewport(0, 0, Size.X, Size.Y);
		}

		protected override void OnUnload()
		{
			base.OnUnload();

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
	}
}
