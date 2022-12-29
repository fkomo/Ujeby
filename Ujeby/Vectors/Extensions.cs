namespace Ujeby.Vectors
{
    public static class DoubleExtensions
    {
        public const double Precision = 0.001;

        public static bool Eq(this double left, double right) => Math.Abs(left - right) < Precision;
        public static bool GrEq(this double left, double right) => left > right || Math.Abs(left - right) < Precision;
        public static bool LeEq(this double left, double right) => left < right || Math.Abs(left - right) < Precision;
    }
}
