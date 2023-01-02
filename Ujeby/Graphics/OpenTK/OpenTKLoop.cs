using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using Ujeby.Graphics.Interfaces;
using Ujeby.Vectors;

namespace Ujeby.Graphics.OpenTK
{
	public abstract class OpenTKLoop : GameWindow, ILoop
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

		protected bool _terminate;

		private readonly v4f _bgColor = new(0.05, 0.05, 0.05, 1);
		private readonly v4f _minorColor = new(0.06, 0.06, 0.06, 1);
		private readonly v4f _majorColor = new(0.1, 0.1, 0.1, 1);
		private readonly v4f _axisColor = new(0.5, 0.5, 0.5, 1);

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

			MouseWindowPosition = new v2i((long)MouseState.Position.X, (long)MouseState.Position.Y);

			// mouse right
			var right = MouseState.IsButtonDown(MouseButton.Right);
			if (DragEnabled)
			{
				if (right && !MouseState.WasButtonDown(MouseButton.Right))
				{
					_drag = true;
					_dragStart = MouseWindowPosition;
				}
				else if (!right && MouseState.WasButtonDown(MouseButton.Right))
				{
					_drag = false;
				}
				else if (_drag)
				{
					GridOffset += MouseWindowPosition - new v2i((long)MouseState.PreviousPosition.X, (long)MouseState.PreviousPosition.Y);
					_dragStart = MouseWindowPosition;
				}
			}

			MouseGridPosition = (MouseWindowPosition - WindowSize / 2) - GridOffset;

			// mouse left
			var left = MouseState.IsButtonDown(MouseButton.Left);
			if (left)
				LeftMouseDown();
			else if (!left && MouseState.WasButtonDown(MouseButton.Left))
				LeftMouseUp();

			MinorGridSize = Math.Max(2, MinorGridSize);

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

		public abstract void Init();
		public abstract void Update();
		public abstract void Render();
		public abstract void Destroy();
		public abstract string Name { get; }

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
