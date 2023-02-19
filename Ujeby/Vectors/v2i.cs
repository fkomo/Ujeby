namespace Ujeby.Vectors
{
#pragma warning disable IDE1006 // Naming Styles
	public struct v2i
#pragma warning restore IDE1006 // Naming Styles
	{
		public long X;
		public long Y;

		public long this[int key]
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

		public readonly static v2i Zero = new(0);
		public readonly static v2i Up = new(0, 1);
		public readonly static v2i Down = new(0, -1);
		public readonly static v2i Left = new(-1, 0);
		public readonly static v2i Right = new(1, 0);

		public readonly static v2i[] RightDownLeftUp = new[]
		{
			Right,
			Down,
			Left,
			Up,
		};

		public v2i(long x, long y) : this()
		{
			X = x;
			Y = y;
		}

		public v2i(long[] xy) : this(xy[0], xy[1])
		{
		}

		public v2i(v2i v) : this(v.X, v.Y)
		{
		}

		public v2i(long value) : this(value, value)
		{
		}

		public override string ToString() => $"[{X};{Y}]";
		public override bool Equals(object obj) => obj is v2i v && this == v;
		public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode();

		public static v2i operator +(v2i a, v2i b) => new(a.X + b.X, a.Y + b.Y);
		public static v2i operator -(v2i a, v2i b) => new(a.X - b.X, a.Y - b.Y);
		public static v2i operator *(v2i a, v2i b) => new(a.X * b.X, a.Y * b.Y);
		public static v2i operator %(v2i a, v2i b) => new(a.X % b.X, a.Y % b.Y);
		public static v2i operator +(v2i a, long b) => new(a.X + b, a.Y + b);
		public static v2i operator -(v2i a, long b) => new(a.X - b, a.Y - b);
		public static v2i operator *(v2i a, double k) => new((long)(a.X * k), (long)(a.Y * k));
		public static v2i operator /(v2i a, double k) => new((long)(a.X / k), (long)(a.Y / k));
		public static v2i operator /(v2i a, v2i b) => new(a.X / b.X, a.Y / b.Y);
		public static bool operator <(v2i a, v2i b) => a.X < b.X && a.Y < b.Y;
		public static bool operator >(v2i a, v2i b) => a.X > b.X && a.Y > b.Y;
		public static bool operator <=(v2i a, v2i b) => a.X <= b.X && a.Y <= b.Y;
		public static bool operator >=(v2i a, v2i b) => a.X >= b.X && a.Y >= b.Y;


		public static implicit operator v2f(v2i v) => new(v.X, v.Y);

		public static bool operator ==(v2i a, v2i b) => a.X == b.X && a.Y == b.Y;
		public static bool operator !=(v2i a, v2i b) => !(a == b);

		public long Length() => (long)Math.Sqrt(Length2());
		public long Length2() => Math.Abs(X * X) + Math.Abs(Y * Y);
		public long Area() => X * Y;
		public v2i Abs() => new(Math.Abs(X), Math.Abs(Y));
		public v2i Inv() => new(-X, -Y);
		public long[] ToArray() => new[] { X, Y };

		/// <summary>Manhattan length</summary>
		public long ManhLength() => X + Y;

		/// <summary>Manhattan distance</summary>
		public static long ManhDistance(v2i a, v2i b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

		public static v2i Min(v2i v1, v2i v2) => new(Math.Min(v1.X, v2.X), Math.Min(v1.Y, v2.Y));
		public static v2i Max(v2i v1, v2i v2) => new(Math.Max(v1.X, v2.X), Math.Max(v1.Y, v2.Y));
	}
}
