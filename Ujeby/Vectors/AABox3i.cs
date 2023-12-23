namespace Ujeby.Vectors
{
	public struct AABox3i
	{
		public v3i Min;
		public v3i Max;
		public v3i Size => Max - Min;

		public static AABox3i Empty => new(new(long.MaxValue), new(long.MinValue));

		public AABox3i(v3i[] corners) : this(v3i.Min(corners[0], corners[1]), v3i.Max(corners[0], corners[1]))
		{
		}

		public AABox3i(v3i min, v3i max)
		{
			Min = min;
			Max = max;
		}

		public override string ToString() => $"{Min}..{Max}";
		public override bool Equals(object obj) => obj is AABox3i aab && this == aab;
		public override int GetHashCode() => Min.GetHashCode() ^ Max.GetHashCode();

		public static bool operator ==(AABox3i a, AABox3i b) => a.Min == b.Min && a.Max == b.Max;
		public static bool operator !=(AABox3i a, AABox3i b) => !(a == b);

		public bool Intersect(AABox3i aab)
			=> !(aab.Min.X > Max.X || aab.Min.Y > Max.Y || aab.Min.Z > Max.Z ||
				aab.Max.X < Min.X || aab.Max.Y < Min.Y || aab.Max.Z < Min.Z);

		public bool And(AABox3i aab, out AABox3i result)
		{
			result = default;
			if (!Intersect(aab))
				return false;

			var x = new long[] { Min.X, Max.X, aab.Min.X, aab.Max.X }.OrderBy(i => i).ToArray();
			var y = new long[] { Min.Y, Max.Y, aab.Min.Y, aab.Max.Y }.OrderBy(i => i).ToArray();
			var z = new long[] { Min.Z, Max.Z, aab.Min.Z, aab.Max.Z }.OrderBy(i => i).ToArray();

			result = new AABox3i(new(x[1], y[1], z[1]), new(x[2], y[2], z[2]));

			return true;
		}
	}
}
