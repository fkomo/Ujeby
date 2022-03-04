
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ujeby.Common.Extensions
{
	public static class EnumerableExtensions
	{
		public static TResult[] Join<TResult>(Func<TResult> delimiterFunc, TResult[] array)
		{
			var result = new List<TResult>();

			if (array.Length < 1)
				return result.ToArray();

			result.Add(array.First());
			foreach (var t in array.Skip(1))
			{
				result.Add(delimiterFunc());
				result.Add(t);
			}

			return result.ToArray();
		}

		public static TResult[] Pad<TResult>(Func<TResult> padFunc, TResult[] array)
		{
			var result = new List<TResult> { padFunc() };
			result.AddRange(array);
			result.Add(padFunc());

			return result.ToArray();
		}

		public static TResult[] RepeatNew<TResult>(Func<TResult> func, int count)
		{
			var result = new List<TResult>();
			for (var i = 0; i < count; i++)
				result.Add(func());

			return result.ToArray();
		}
	}
}
