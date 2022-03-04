using System.Globalization;

namespace Ujeby.Common.Types
{
	public class V2
	{
		public double X = 0.0;
		public double Y = 0.0;

		public V2()
		{

		}

		public V2(V2 v) : this(v.X, v.Y)
		{

		}

		public V2(double x, double y)
		{
			X = x;
			Y = y;
		}

		public V2(double d) : this(d, d)
		{

		}

		public override string ToString()
		{
			return $"{ X.ToString("F4", CultureInfo.InvariantCulture) }, { Y.ToString("F4", CultureInfo.InvariantCulture) }";
		}
	}

	public class V3 : V2
	{
		public double Z = 0.0;

		public V3()
		{

		}

		public V3(V3 v) : this(v.X, v.Y, v.Z)
		{

		}

		public V3(double x, double y, double z) : base(x, y)
		{
			Z = z;
		}

		public V3(double d) : this(d, d, d)
		{

		}

		public V3(V2 v) : base(v.X, v.Y)
		{

		}

		public override string ToString()
		{
			return $"{ base.ToString() }, { Z.ToString("F4", CultureInfo.InvariantCulture) }";
		}
	}

	public class V4 : V3
	{
		public double W = 0.0;

		public V4()
		{

		}

		public V4(V4 v) : this(v.X, v.Y, v.Z, v.Z)
		{

		}

		public V4(double x, double y, double z, double w) : base(x, y, z)
		{
			W = w;
		}

		public V4(double d) : this(d, d, d, d)
		{

		}

		public V4(V2 v) : base(v)
		{

		}

		public V4(V3 v) : base(v.X, v.Y, v.Z)
		{

		}

		public override string ToString()
		{
			return $"{ base.ToString() }, { W.ToString("F4", CultureInfo.InvariantCulture) }";
		}
	}
}
