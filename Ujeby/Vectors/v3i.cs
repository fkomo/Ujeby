namespace Ujeby.Vectors
{
#pragma warning disable IDE1006 // Naming Styles
	public struct v3i
#pragma warning restore IDE1006 // Naming Styles
	{
		public long X;
		public long Y;
		public long Z;

		public long this[int key]
		{
			get
			{
				return key switch
				{
					0 => X,
					1 => Y,
					2 => Z,
					_ => throw new ArgumentOutOfRangeException(nameof(key), $"{nameof(key)}={key}"),
				};
			}
			set
			{
				switch (key)
				{
					case 0: X = value; break;
					case 1: Y = value; break;
					case 2: Z = value; break;
					default:
						throw new ArgumentOutOfRangeException(nameof(key), $"{nameof(key)}={key}");
				}
			}
		}

		public readonly static v3i Zero = new();

		public v3i(long x, long y, long z) : this()
		{
			X = x;
			Y = y;
			Z = z;
		}

		public v3i(v3i v) : this(v.X, v.Y, v.Z)
		{
		}

		public v3i(long value) : this(value, value, value)
		{
		}

		public v3i(v2i v2, long z) : this(v2.X, v2.Y, z)
		{
		}

		public override string ToString() => $"[{X};{Y};{Z}]";
		public override bool Equals(object obj) => obj is v3i v && this == v;
		public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();

		public static v3i operator +(v3i a, v3i b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
		public static v3i operator -(v3i a, v3i b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
		public static v3i operator *(v3i a, v3i b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
		public static v3i operator *(v3i a, double k) => new((long)(a.X * k), (long)(a.Y * k), (long)(a.Z * k));
		public static v3i operator /(v3i a, double k) => new((long)(a.X / k), (long)(a.Y / k), (long)(a.Z / k));
		public static v3i operator /(v3i a, v3i b) => new(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
		public static v3i operator +(v3i a, v2i b) => new(a.X + b.X, a.Y + b.Y, a.Z);

		public static bool operator ==(v3i a, v3i b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z;
		public static bool operator !=(v3i a, v3i b) => !(a == b);

		public long Volume() => X * Y * Z;
		public v3i Abs() => new(Math.Abs(X), Math.Abs(Y), Math.Abs(Z));
		public v2i ToV2i() => new(X, Y);

		public static v3i Min(v3i v1, v3i v2) => new(Math.Min(v1.X, v2.X), Math.Min(v1.Y, v2.Y), Math.Min(v1.Z, v2.Z));
		public static v3i Max(v3i v1, v3i v2) => new(Math.Max(v1.X, v2.X), Math.Max(v1.Y, v2.Y), Math.Max(v1.Z, v2.Z));
	}
}
