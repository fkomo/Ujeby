namespace Ujeby.Vectors
{
#pragma warning disable IDE1006 // Naming Styles
	public struct v4i
#pragma warning restore IDE1006 // Naming Styles
	{
		public long X;
		public long Y;
		public long Z;
		public long W;

		public long this[int key]
		{
			get
			{
				return key switch
				{
					0 => X,
					1 => Y,
					2 => Z,
					3 => W,
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
					case 3: W = value; break;
					default:
						throw new ArgumentOutOfRangeException(nameof(key), $"{nameof(key)}={key}");
				}
			}
		}

		public readonly static v4i Zero = new();

		public v4i(long x, long y, long z, long w) : this()
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		public v4i(v4i v) : this(v.X, v.Y, v.Z, v.W)
		{
		}

		public v4i(long value) : this(value, value, value, value)
		{
		}

		public v4i(v3i xyz, long w) : this(xyz.X, xyz.Y, xyz.Z, w)
		{
		}

		public v4i(v2i xy, long z, long w) : this(xy.X, xy.Y, z, w)
		{
		}

		public override string ToString() => $"[{X};{Y};{Z};{W}]";
		public override bool Equals(object obj) => obj is v4i v && this == v;
		public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode() ^ W.GetHashCode();

		public static v4i operator +(v4i a, v4i b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
		public static v4i operator -(v4i a, v4i b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
		public static v4i operator *(v4i a, v4i b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.W * b.W);
		public static v4i operator *(v4i a, double k) => new((long)(a.X * k), (long)(a.Y * k), (long)(a.Z * k), (long)(a.W * k));
		public static v4i operator /(v4i a, double k) => new((long)(a.X / k), (long)(a.Y / k), (long)(a.Z / k), (long)(a.W / k));
		public static v4i operator /(v4i a, v4i b) => new(a.X / b.X, a.Y / b.Y, a.Z / b.Z, a.W / b.W);
		public static v4i operator +(v4i a, v2i b) => new(a.X + b.X, a.Y + b.Y, a.Z, a.W);
		public static v4i operator +(v4i a, v3i b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W);

		public static implicit operator v2f(v4i v) => new(v.X, v.Y);

		public static implicit operator v2i(v4i v) => new(v.X, v.Y);
		public static implicit operator v3i(v4i v) => new(v.X, v.Y, v.Z);

		public static bool operator ==(v4i a, v4i b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z && a.W == b.W;
		public static bool operator !=(v4i a, v4i b) => !(a == b);

		public v4i Abs() => new(Math.Abs(X), Math.Abs(Y), Math.Abs(Z), Math.Abs(W));
		public v2i ToV2i() => new(X, Y);
		public v3i ToV3i() => new(X, Y, Z);

		public static v4i Min(v4i v1, v4i v2) => new(Math.Min(v1.X, v2.X), Math.Min(v1.Y, v2.Y), Math.Min(v1.Z, v2.Z), Math.Min(v1.W, v2.W));
		public static v4i Max(v4i v1, v4i v2) => new(Math.Max(v1.X, v2.X), Math.Max(v1.Y, v2.Y), Math.Max(v1.Z, v2.Z), Math.Max(v1.W, v2.W));
	}
}
