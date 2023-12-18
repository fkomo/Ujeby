using Ujeby.Tools.StringExtensions;

namespace Ujeby.Vectors
{
#pragma warning disable IDE1006 // Naming Styles
	public struct v3i : IComparable
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

		public readonly static v3i Up = new(0, 1, 0);
		public readonly static v3i Down = new(0, -1, 0);
		public readonly static v3i Right = new(1, 0, 0);
		public readonly static v3i Left = new(-1, 0, 0);
		public readonly static v3i Front = new(0, 0, 1);
		public readonly static v3i Back = new(0, 0, -1);

		public readonly static v3i[] Cube = new v3i[]
		{
			Up + Right + Front,
			Up + Right + Back,
			Up + Left + Back,
			Up + Left + Front,

			Down + Right + Front,
			Down + Right + Back,
			Down + Left + Back,
			Down + Left + Front,
		};

		public readonly static v3i[] UpDownLeftRightBackFront = new[]
{
			Up,
			Down,
			Left,
			Right,
			Back,
			Front
		};

		public v3i(long x, long y, long z) : this()
		{
			X = x;
			Y = y;
			Z = z;
		}

		public v3i(long[] xyz) : this(xyz[0], xyz[1], xyz[2])
		{
		}

		public v3i(v3i v) : this(v.X, v.Y, v.Z)
		{
		}

		public v3i(long value) : this(value, value, value)
		{
		}

		public v3i(string rgbColorHash)
		{
			if (rgbColorHash.StartsWith('#'))
				rgbColorHash = rgbColorHash[1..];

			var value = int.Parse(rgbColorHash, System.Globalization.NumberStyles.HexNumber);
			X = (value & 0xff0000) >> 16;
			Y = (value & 0x00ff00) >> 8;
			Z = (value & 0x0000ff) >> 0;
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
		public static v3i operator +(v3i a, long b) => new(a.X + b, a.Y + b, a.Z + b);
		public static v3i operator -(v3i a, long b) => a + (-b);

		public static bool operator ==(v3i a, v3i b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z;
		public static bool operator !=(v3i a, v3i b) => !(a == b);

		public static v3i Parse(string s) => new(s.ToNumArray());

		public readonly long Volume() => X * Y * Z;
		public readonly long Surface() => 2 * (X * Y + X * Z + Y * Z);
		public readonly long Length() => (long)System.Math.Sqrt(Length2());
		public readonly long Length2() => System.Math.Abs(X * X) + System.Math.Abs(Y * Y) + System.Math.Abs(Z * Z);
		public readonly v3i Abs() => new(System.Math.Abs(X), System.Math.Abs(Y), System.Math.Abs(Z));
		public readonly v3i Inv() => new(-X, -Y, -Z);
		public readonly v2i ToV2i() => new(X, Y);
		public readonly long[] ToArray() => new[] { X, Y, Z };

		/// <summary>Manhattan length</summary>
		public readonly long ManhLength() => System.Math.Abs(X) + System.Math.Abs(Y) + System.Math.Abs(Z);

		/// <summary>Manhattan distance</summary>
		public static long ManhDistance(v3i a, v3i b) => System.Math.Abs(a.X - b.X) + System.Math.Abs(a.Y - b.Y) + System.Math.Abs(a.Z - b.Z);

		public static v3i Min(v3i v1, v3i v2) => new(System.Math.Min(v1.X, v2.X), System.Math.Min(v1.Y, v2.Y), System.Math.Min(v1.Z, v2.Z));
		public static v3i Max(v3i v1, v3i v2) => new(System.Math.Max(v1.X, v2.X), System.Math.Max(v1.Y, v2.Y), System.Math.Max(v1.Z, v2.Z));

		public static v3i Clamp(v3i v, v3i min, v3i max)
			=> new(System.Math.Clamp(v.X, min.X, max.X), System.Math.Clamp(v.Y, min.Y, max.Y), System.Math.Clamp(v.Z, min.Z, max.Z));

		public readonly v3i RotateCWX() => new(X, -Z, Y);
		public readonly v3i RotateCWY() => new(Z, Y, -X);
		public readonly v3i RotateCWZ() => new(Y, -X, Z);

		public readonly v3i RotateCCWX() => RotateCWX().RotateCWX().RotateCWX();
		public readonly v3i RotateCCWY() => RotateCWY().RotateCWY().RotateCWY();
		public readonly v3i RotateCCWZ() => RotateCWZ().RotateCWZ().RotateCWZ();

		public int CompareTo(object obj)
		{
			if (obj is v3i v2)
			{
				var l1 = Length2();
				var l2 = v2.Length2();

				if (l1 == l2)
					return 0;

				return l1 < l2 ? -1 : 1;
			}

			throw new Exception($"CompareTo: {nameof(v3i)} vs {obj?.GetType().Name}");
		}
	}
}