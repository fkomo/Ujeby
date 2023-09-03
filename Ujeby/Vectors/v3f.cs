namespace Ujeby.Vectors
{
#pragma warning disable IDE1006 // Naming Styles
	public struct v3f
#pragma warning restore IDE1006 // Naming Styles
	{
		public double X;
		public double Y;
		public double Z;

		public double this[int key]
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

		public readonly static v3f Zero = new();

		public v3f(double x, double y, double z) : this()
		{
			X = x;
			Y = y;
			Z = z;
		}

		public v3f(double xy, double z) : this(xy, xy, z)
		{
		}

		public v3f(v3f v) : this(v.X, v.Y, v.Z)
		{
		}

		public v3f(double value) : this(value, value, value)
		{
		}

		public override string ToString() => $"[{X:0.00};{Y:0.00};{Z:0.00}]";
		public override bool Equals(object obj) => obj is v3f v && this == v;
		public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();

		public static v3f operator +(v3f a, v3f b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
		public static v3f operator -(v3f a, v3f b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
		public static v3f operator *(v3f a, double k) => new(a.X * k, a.Y * k, a.Z * k);
		public static v3f operator *(v3f a, v3f b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
		public static v3f operator /(v3f a, double k) => new(a.X / k, a.Y / k, a.Z / k);
		public static v3f operator /(v3f a, v3f b) => new(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
		public static v3f operator +(v3f a, double k) => new(a.X + k, a.Y + k, a.Z + k);
		public static v3f operator -(v3f a, double k) => new(a.X - k, a.Y - k, a.Z - k);

		public static bool operator ==(v3f a, v3f b) => a.X.Eq(b.X) && a.Y.Eq(b.Y) && a.Z.Eq(b.Z);
		public static bool operator !=(v3f a, v3f b) => !(a == b);

		public v3f Abs() => new(System.Math.Abs(X), System.Math.Abs(Y), System.Math.Abs(Z));
		public v3f Inv() => new(-X, -Y, -Z);
		public v3f Min(v3f v) => new(System.Math.Min(X, v.X), System.Math.Min(Y, v.Y), System.Math.Min(Z, v.Z));
		public v3f Max(v3f v) => new(System.Math.Max(X, v.X), System.Math.Max(Y, v.Y), System.Math.Max(Z, v.Z));
		public v3i ToV3i() => new((long)X, (long)Y, (long)Z);
		public double[] ToArray() => new[] { X, Y, Z };

		public static v3f Min(v3f v1, v3f v2) => new(System.Math.Min(v1.X, v2.X), System.Math.Min(v1.Y, v2.Y), System.Math.Min(v1.Z, v2.Z));
		public static v3f Max(v3f v1, v3f v2) => new(System.Math.Max(v1.X, v2.X), System.Math.Max(v1.Y, v2.Y), System.Math.Max(v1.Z, v2.Z));
		public static double Dot(v3f v1, v3f v2) => v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
	}
}
