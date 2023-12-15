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

		/// <summary>
		/// finds repeating patter
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="data"></param>
		/// <param name="start"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static bool FindRepeatingPattern<T>(this T[] data, out int start, out int length)
			where T : IComparable
		{
			start = length = -1;

			var startHashSetCount = 0;
			var hashSet = new HashSet<T>();
			for (var i = 0; i < data.Length; i++)
			{
				if (!hashSet.Add(data[i]))
				{
					if (startHashSetCount != hashSet.Count)
					{
						startHashSetCount = hashSet.Count;
						start = i;
					}
				}
			}

			if (start == -1)
				return false;

			for (var i = start + 2; i < data.Length; i++)
			{
				if (data[i].CompareTo(data[start]) == 0)
				{
					length = i - start;
					var validPattern = true;
					for (var i2 = data.Length - 1; i2 > start; i2--)
						if (data[i2].CompareTo(data[i2 - length]) != 0)
						{
							length = -1;
							validPattern = false;
							break;
						}

					if (validPattern)
						return true;
				}
			}

			return false;
		}
	}
}
