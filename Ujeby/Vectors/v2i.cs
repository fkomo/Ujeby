﻿using Ujeby.Tools.StringExtensions;

namespace Ujeby.Vectors
{
#pragma warning disable IDE1006 // Naming Styles
	public struct v2i : IComparable<v2i>
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

		public readonly static v2i[] DownUpLeftRight = new[]
		{
			Down,
			Up,
			Left,
			Right,
		};

		public readonly static v2i[] UpDownLeftRight = new[]
		{
			Up,
			Down,
			Left,
			Right,
		};

		public readonly static v2i[] PlusMinusOne = new[]
		{
			Left + Up,
			Up,
			Right + Up,
			Right,
			Right + Down,
			Down,
			Left + Down,
			Left,
		};

		public readonly static v2i[] Corners = new[]
		{
			Right + Up,
			Left + Up,
			Right + Down,
			Left + Down,
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

		public static v2i Parse(string s) => new(s.ToNumArray());

		public long Length() => (long)System.Math.Sqrt(Length2());
		public long Length2() => System.Math.Abs(X * X) + System.Math.Abs(Y * Y);
		public long Area() => X * Y;
		public v2i Abs() => new(System.Math.Abs(X), System.Math.Abs(Y));
		public v2i Inv() => new(-X, -Y);
		public long[] ToArray() => new[] { X, Y };

		/// <summary>Manhattan length</summary>
		public long ManhLength() => System.Math.Abs(X) + System.Math.Abs(Y);

		/// <summary>Manhattan distance</summary>
		public static long ManhDistance(v2i a, v2i b) => System.Math.Abs(a.X - b.X) + System.Math.Abs(a.Y - b.Y);

		public static v2i Min(v2i v1, v2i v2) => new(System.Math.Min(v1.X, v2.X), System.Math.Min(v1.Y, v2.Y));
		public static v2i Max(v2i v1, v2i v2) => new(System.Math.Max(v1.X, v2.X), System.Math.Max(v1.Y, v2.Y));

		public readonly int CompareTo(v2i other)
		{
			if (other.X < X && other.Y < Y) 
				return 1;
			
			if (other.X > X && other.Y > Y)
				return -1;

			return 0;
		}
	}
}
