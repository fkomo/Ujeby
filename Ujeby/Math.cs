namespace Ujeby
{
    public static class Math
    {
        /// <summary>
        /// decimal to any base number
        /// binary (baseString=01) - default
        /// hex (baseString=0123456789abcdef)
        /// ...
        /// </summary>
        /// <param name="dec"></param>
        /// <param name="baseString"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static string DecToBase(long dec, string baseString = "01", int offset = 0)
        {
            var result = string.Empty;

            do
            {
                dec -= offset;
                result = $"{baseString.Substring((int)(dec % baseString.Length), 1)}{result}";
                dec /= baseString.Length;
            }
            while (dec > 0);

            return result;
        }

        /// <summary>
        /// number in any base format to decimal
        /// binary (baseString=01) - default
        /// hex (baseString=0123456789abcdef)
        /// ...
        /// </summary>
        /// <param name="value"></param>
        /// <param name="baseString"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static long BaseToDec(string value, string baseString = "01", int offset = 0)
        {
            if (baseString.Length == 2 && offset == 0)
                // faster
                return Convert.ToInt32(value.Replace(baseString[0], '0').Replace(baseString[1], '1'), 2);

            long pow = 1;
            long result = 0;
            foreach (var c in value.Reverse())
            {
                result += pow * (baseString.IndexOf(c) + offset);
                pow *= baseString.Length;
            }

            return result;
        }

        /// <summary>
        /// factorial
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static long Factorial(long f) => f == 0 ? 1 : f * Factorial(f - 1);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static long GreatestCommonDivisor(long a, long b)
		{
			while (b != 0)
			{
				var temp = b;
				b = a % b;
				a = temp;
			}
			return a;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static long LeastCommonMultiple(long a, long b)
            => a / GreatestCommonDivisor(a, b) * b;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
		public static long LeastCommonMultiple(this IEnumerable<long> values)
	        => values.Aggregate(LeastCommonMultiple);
	}
}
