namespace Ujeby.Alg
{
	/// <summary>
	/// https://stackoverflow.com/questions/1952153/what-is-the-best-way-to-find-all-combinations-of-items-in-an-array
	/// </summary>
	public class Combinatorics
	{
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static IEnumerable<T>[] PermutationsWithRep<T>(IEnumerable<T> list, int length)
		{
			if (length == 1)
				return list.Select(t => new T[] { t })
					.ToArray();
			
			return PermutationsWithRep(list, length - 1)
				.SelectMany(t => list, (t1, t2) => t1.Concat(new T[] { t2 }))
				.ToArray();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static IEnumerable<T>[] Permutations<T>(IEnumerable<T> list, int length)
		{
			if (length == 1)
				return list.Select(t => new T[] { t })
					.ToArray();

			return Permutations(list, length - 1)
				.SelectMany(t => list.Where(o => !t.Contains(o)), (t1, t2) => t1.Concat(new T[] { t2 }))
				.ToArray();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static IEnumerable<T>[] Combinations<T>(IEnumerable<T> list, int length)
			where T : IComparable
		{
			if (length == 1)
				return list.Select(t => new T[] { t })
					.ToArray();

			return Combinations(list, length - 1)
				.SelectMany(t => list.Where(o => o.CompareTo(t.Last()) > 0), (t1, t2) => t1.Concat(new T[] { t2 }))
				.ToArray();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static IEnumerable<T>[] CombinationsWithRep<T>(IEnumerable<T> list, int length)
			where T : IComparable
		{
			if (length == 1)
				return list.Select(t => new T[] { t })
					.ToArray();

			return CombinationsWithRep(list, length - 1)
				.SelectMany(t => list.Where(o => o.CompareTo(t.Last()) >= 0), (t1, t2) => t1.Concat(new T[] { t2 }))
				.ToArray();
		}
	}
}
