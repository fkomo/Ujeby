namespace Ujeby.Tools.ArrayExtensions
{
	public static class ArrayExtensions
	{
		/// <summary>
		/// splits array of elements into subarrays based on specific delimiting element
		/// </summary>
		/// <param name="a"></param>
		/// <param name="separator"></param>
		/// <returns></returns>
		public static T[][] Split<T>(this T[] a, T separator)
			where T : IComparable
		{
			var result = new List<T[]>();

			for (var i = 0; i < a.Length; i++)
			{
				var group = new List<T>();

				for (; i < a.Length; i++)
				{
					if (a[i].CompareTo(separator) == 0)
						break;

					group.Add(a[i]);
				}

				result.Add(group.ToArray());
			}

			return result.ToArray();
		}

		/// <summary>
		/// returns array of pascal-case strings
		/// </summary>
		/// <param name="a"></param>
		/// <returns></returns>
		public static string[] ToPascalCase(this string[] a)
		{
			if (a == null)
				return a;

			for (var i = 0; i < a.Length; i++)
				if (char.IsLower(a[i][0]))
					a[i] = char.ToUpper(a[i][0]) + a[i][1..];

			return a;
		}
	}
}
