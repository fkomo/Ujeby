namespace Ujeby.Vectors
{
#pragma warning disable IDE1006 // Naming Styles
	public struct v2f : IComparable<v2f>
#pragma warning restore IDE1006 // Naming Styles
	{
		public double X;
		public double Y;

		public double this[int key]
		{
			get
			{
				return key switch
				{
					0 => X,
					1 => Y,
					_ => throw new ArgumentOutOfRangeException(nameof(key), $"{nameof(key)}={key}"),
				};
			}
			set
			{
				switch (key)
				{
					case 0: X = value; break;
					case 1: Y = value; break;
					default:
						throw new ArgumentOutOfRangeException(nameof(key), $"{nameof(key)}={key}");
				}
			}
		}

		public readonly static v2f Zero = new();
		public readonly static v2f Up = new(0, 1);
		public readonly static v2f Down = new(0, -1);
		public readonly static v2f Left = new(-1, 0);
		public readonly static v2f Right = new(1, 0);

		public v2f(double x, double y) : this()
		{
			X = x;
			Y = y;
		}

		public v2f(v2f v) : this(v.X, v.Y)
		{
		}

		public v2f(double d) : this(d, d)
		{
		}

		public override string ToString() => $"[{X:0.00};{Y:0.00}]";
		public override bool Equals(object obj) => obj is v2f v && this == v;
		public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode();

		public static v2f operator +(v2f a, v2f b) => new(a.X + b.X, a.Y + b.Y);
		public static v2f operator +(v2f a, v2i b) => new(a.X + b.X, a.Y + b.Y);
		public static v2f operator -(v2f a, v2f b) => new(a.X - b.X, a.Y - b.Y);
		public static v2f operator -(v2f a, v2i b) => new(a.X - b.X, a.Y - b.Y);
		public static v2f operator *(v2f a, double k) => new(a.X * k, a.Y * k);
		public static v2f operator *(v2f a, v2f b) => new(a.X * b.X, a.Y * b.Y);
		public static v2f operator /(v2f a, double k) => new(a.X / k, a.Y / k);
		public static v2f operator /(v2f a, v2f b) => new(a.X / b.X, a.Y / b.Y);
		public static v2f operator +(v2f a, double k) => new(a.X + k, a.Y + k);
		public static v2f operator -(v2f a, double k) => new(a.X - k, a.Y - k);

		public static bool operator ==(v2f a, v2f b) => a.X.Eq(b.X) && a.Y.Eq(b.Y);
		public static bool operator !=(v2f a, v2f b) => !(a == b);

		public double Length() => System.Math.Sqrt(X * X + Y * Y);
		public v2f Normalize() => this * 1.0 / Length();
		public v2f Abs() => new(System.Math.Abs(X), System.Math.Abs(Y));
		public v2f Inv() => new(-X, -Y);
		public v2f Min(v2f v) => new(System.Math.Min(X, v.X), System.Math.Min(Y, v.Y));
		public v2f Max(v2f v) => new(System.Math.Max(X, v.X), System.Math.Max(Y, v.Y));
		public v2i Trunc() => new((long)X, (long)Y);
		public v2i Round() => new((long)System.Math.Round(X), (long)System.Math.Round(Y));
		public v2i ToV2i() => new((long)X, (long)Y);
		public double[] ToArray() => new[] { X, Y };

		public static double Dot(v2f v1, v2f v2) => v1.X * v2.X + v1.Y * v2.Y;
		public static v2f Min(v2f v1, v2f v2) => new(System.Math.Min(v1.X, v2.X), System.Math.Min(v1.Y, v2.Y));
		public static v2f Max(v2f v1, v2f v2) => new(System.Math.Max(v1.X, v2.X), System.Math.Max(v1.Y, v2.Y));

		public int CompareTo(v2f other)
		{
			if (other.X < X && other.Y < Y)
				return 1;

			if (other.X > X && other.Y > Y)
				return -1;

			return 0;
		}
	}
}
