namespace Ujeby.Vectors
{
	public struct AABox3i
	{
		public v3i Min;
		public v3i Max;
		public v3i Size => Max - Min;

		public static AABox3i Empty => new(new(long.MaxValue), new(long.MinValue));

		public AABox3i(v3i min, v3i max)
		{
			Min = min;
			Max = max;
		}

		public override string ToString() => $"{Min}..{Max}";
		public override bool Equals(object obj) => obj is AABox3i aab && this == aab;
		public override int GetHashCode() => Min.GetHashCode() ^ Max.GetHashCode();

		public bool Intersect(AABox3i aab)
			=> !(aab.Min.X > Max.X || aab.Min.Y > Max.Y || aab.Min.Z > Max.Z ||
				aab.Max.X < Min.X || aab.Max.Y < Min.Y || aab.Max.Z < Min.Z);

		public static bool operator ==(AABox3i a, AABox3i b) => a.Min == b.Min && a.Max == b.Max;
		public static bool operator !=(AABox3i a, AABox3i b) => !(a == b);
	}
}
