namespace Ujeby.Tools
{
	public static class StringExtensions
	{
		/// <summary>
		/// splits array of nested objects, but only top level. for example:
		/// [1,2,[3,4]] -> 3 elements: "1", "2", "[3,4]"
		/// [[1,2],[3,4,5]] -> 2 elements: "[1,2]", "[3,4,5]"
		/// ...
		/// </summary>
		/// <param name="s"></param>
		/// <param name="separator"></param>
		/// <param name="openBracket"></param>
		/// <param name="closeBracket"></param>
		/// <returns></returns>
		public static string[] SplitNestedArrays(this string s, 
			char separator = ',', char openBracket = '[', char closeBracket = ']')
		{
			if (!s.Any(c => c == openBracket || c == closeBracket))
				return s.Split(separator);

			var result = new List<string>();

			string item = null;
			for (var i = 0; i < s.Length; i++)
			{
				if (s[i] == openBracket)
				{
					var c = 1;
					var i2 = i + 1;
					while (c > 0)
					{
						if (s[i2] == openBracket)
							c++;
						else if (s[i2] == closeBracket)
							c--;

						i2++;
					}

					result.Add(s[i..i2]);
					i = i2 - 1;
				}
				else
				{
					if (s[i] == separator)
					{
						if (item != null)
							result.Add(item);

						item = null;
					}
					else
						item += s[i];
				}
			}

			if (item != null)
				result.Add(item);

			return result.ToArray();
		}
	}

}
