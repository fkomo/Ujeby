namespace Ujeby.Vectors
{
#pragma warning disable IDE1006 // Naming Styles
	public struct v4f
#pragma warning restore IDE1006 // Naming Styles
	{
		public double X;
		public double Y;
		public double Z;
		public double W;

		public double this[int key]
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

		public readonly static v4f Zero = new();

		public v4f(double x, double y, double z, double w) : this()
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		public v4f(double xyz, double w) : this(xyz, xyz, xyz, w)
		{
		}

		public v4f(v4f v) : this(v.X, v.Y, v.Z, v.W)
		{
		}

		public v4f(double value) : this(value, value, value, value)
		{
		}

		public override string ToString() => $"[{X:0.00};{Y:0.00};{Z:0.00};{W:0.00}]";
		public override bool Equals(object obj) => obj is v4f v && this == v;
		public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode() ^ W.GetHashCode();

		public static v4f operator +(v4f a, v4f b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
		public static v4f operator -(v4f a, v4f b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
		public static v4f operator *(v4f a, double k) => new(a.X * k, a.Y * k, a.Z * k, a.W * k);
		public static v4f operator *(v4f a, v4f b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.W * b.W);
		public static v4f operator /(v4f a, double k) => new(a.X / k, a.Y / k, a.Z / k, a.W / k);
		public static v4f operator /(v4f a, v4f b) => new(a.X / b.X, a.Y / b.Y, a.Z / b.Z, a.W / b.W);
		public static v4f operator +(v4f a, double k) => new(a.X + k, a.Y + k, a.Z + k, a.W + k);
		public static v4f operator -(v4f a, double k) => new(a.X - k, a.Y - k, a.Z - k, a.W - k);

		public static bool operator ==(v4f a, v4f b) => a.X.Eq(b.X) && a.Y.Eq(b.Y) && a.Z.Eq(b.Z) && a.W.Eq(b.W);
		public static bool operator !=(v4f a, v4f b) => !(a == b);

		public v4f Abs() => new(Math.Abs(X), Math.Abs(Y), Math.Abs(Z), Math.Abs(W));
		public v4f Inv() => new(-X, -Y, -Z, -W);
		public v4f Min(v4f v) => new(Math.Min(X, v.X), Math.Min(Y, v.Y), Math.Min(Z, v.Z), Math.Min(W, v.W));
		public v4f Max(v4f v) => new(Math.Max(X, v.X), Math.Max(Y, v.Y), Math.Max(Z, v.Z), Math.Max(W, v.W));
		public double[] ToArray() => new[] { X, Y, Z, W };

		public static v4f Min(v4f v1, v4f v2) => new(Math.Min(v1.X, v2.X), Math.Min(v1.Y, v2.Y), Math.Min(v1.Z, v2.Z), Math.Min(v1.W, v2.W));
		public static v4f Max(v4f v1, v4f v2) => new(Math.Max(v1.X, v2.X), Math.Max(v1.Y, v2.Y), Math.Max(v1.Z, v2.Z), Math.Max(v1.W, v2.W));
		public static double Dot(v4f v1, v4f v2) => v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z + v1.W * v2.W;
	}
}
