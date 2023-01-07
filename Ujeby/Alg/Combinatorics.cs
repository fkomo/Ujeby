namespace Ujeby.Alg
{
	/// <summary>
	/// https://stackoverflow.com/questions/1952153/what-is-the-best-way-to-find-all-combinations-of-items-in-an-array
	/// </summary>
	public class Combinatorics
	{
		public static IEnumerable<IEnumerable<T>> PermutationsWithRep<T>(IEnumerable<T> list, int length)
		{
			if (length == 1)
				return list.Select(t => new T[] { t });
			
			return PermutationsWithRep(list, length - 1)
				.SelectMany(t => list, (t1, t2) => t1.Concat(new T[] { t2 }));
		}

		public static IEnumerable<IEnumerable<T>> Permutations<T>(IEnumerable<T> list, int length)
		{
			if (length == 1)
				return list.Select(t => new T[] { t });
			
			return Permutations(list, length - 1)
				.SelectMany(t => list.Where(o => !t.Contains(o)), (t1, t2) => t1.Concat(new T[] { t2 }));
		}

		public static IEnumerable<IEnumerable<T>> KCombinations<T>(IEnumerable<T> list, int length)
			where T : IComparable
		{
			if (length == 1)
				return list.Select(t => new T[] { t });

			return KCombinations(list, length - 1)
				.SelectMany(t => list.Where(o => o.CompareTo(t.Last()) > 0), (t1, t2) => t1.Concat(new T[] { t2 }));
		}

		public static IEnumerable<IEnumerable<T>> KCombinationsWithRep<T>(IEnumerable<T> list, int length)
			where T : IComparable
		{
			if (length == 1)
				return list.Select(t => new T[] { t });
			
			return KCombinationsWithRep(list, length - 1)
				.SelectMany(t => list.Where(o => o.CompareTo(t.Last()) >= 0), (t1, t2) => t1.Concat(new T[] { t2 }));
		}
	}
}
