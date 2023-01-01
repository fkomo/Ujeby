using Ujeby.Vectors;

namespace Ujeby.Graphics.Entities
{
	public struct AABBi
	{
		public v2i Min { get; private set; }
		public v2i Max { get; private set; }

		public v2i HalfSize { get; private set; }
		public v2i Center { get; private set; }
		public v2i Size => Max - Min;

		public long Top => Max.Y;
		public long Bottom => Min.Y;
		public long Left => Min.X;
		public long Right => Max.X;

		public override string ToString() => $"{ Min }-{ Max }";
		public static AABBi operator +(AABBi bb, v2i v) => new(bb.Min + v, bb.Max + v);

		public AABBi(v2i min, v2i max)
		{
			Min = min;
			Max = max;
			Center = (min + max) / 2;
			HalfSize = (max - min) / 2;
		}

		public bool Contains(v2i p) => (Min <= p && p <= Max);
	}
}
