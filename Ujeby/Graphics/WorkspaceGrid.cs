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

		protected void SetGridCenter(v2i v)
		{
			Offset = v.Inv();
		}

		protected void MoveGridCenter(v2i v)
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
			MinorSize = Math.Max(2, MinorSize + value);
		}
	}
}
