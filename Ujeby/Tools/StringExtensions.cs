using System.Text;
using System.Text.RegularExpressions;

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

		/// <summary>
		/// returns array of numbers in order they appeared in input string
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static long[] ToNumArray(this string s)
		{
			var sb = new StringBuilder();
			for (var i = 0; i < s.Length; i++)
				if (char.IsDigit(s[i]))
					sb.Append(s[i]);
				else
					sb.Append(' ');

			return sb.ToString()
				.Split(' ', StringSplitOptions.RemoveEmptyEntries)
				.Select(i => long.Parse(i))
				.ToArray();
		}

		/// <summary>
		/// split pascal/camel case string with
		/// </summary>
		/// <param name="s"></param>
		/// <param name="delimiter"></param>
		/// <returns></returns>
		public static string SplitCase(this string s, 
			char delimiter = ' ')
		{
			var result = string.Empty;

			for (var i = 0; i < s.Length; i++)
			{
				result += s[i];
				if (i < s.Length - 1 && char.IsLower(s[i]) && char.IsUpper(s[i + 1]))
					result += delimiter;
			}

			return result;
		}

		/// <summary>
		/// removes substring defined by 2 strings (start - end)
		/// </summary>
		/// <param name="s"></param>
		/// <param name="substringStart"></param>
		/// <param name="substringEnd"></param>
		/// <returns></returns>
		public static string RemoveFromTo(this string s, string substringStart, string substringEnd)
		{
			if (string.IsNullOrEmpty(s))
				return null;

			var startIndex = s.ToLower().IndexOf(substringStart.ToLower());
			if (startIndex < 0)
				return s;

			var endIndex = s.ToLower().LastIndexOf(substringEnd.ToLower());
			if (endIndex < 0)
				return s;

			return s.Remove(startIndex, endIndex - startIndex + substringEnd.Length);
		}

		private const string HTML_TAG_PATTERN = "<.*?>";

		/// <summary>
		/// https://stackoverflow.com/questions/4878452/remove-html-tags-in-string
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string StripHTML(this string s)
		{
			if (string.IsNullOrEmpty(s))
				return null;

			return Regex.Replace(s, HTML_TAG_PATTERN, string.Empty).Trim();
		}

		public static string Normalize(this string s)
		{
			//var after = before.ToLower()
			//	.Replace("’s ", "s ").Replace("’n ", "n ").Replace("’t ", "t ")
			//	.Replace(" ’", " ").Replace("’ ", " ").Replace("’", "")
			//	.Replace("'s ", "s ").Replace("'n ", "n ").Replace("'t ", "t ")
			//	.Replace(" '", " ").Replace("' ", " ").Replace("'", "")
			//	.Replace("²", " ").Replace("®", " ").Replace("™", " ")
			//	.Replace("\"", " ").Replace("“", " ").Replace("”", " ")
			//	.Replace("/", " ").Replace("\\", " ")
			//	.Replace("=", " ").Replace("?", " ").Replace("!", " ").Replace("&", " ").Replace(":", " ").Replace(" and ", " ").Replace("-", " ").Replace(".", " ").Replace(",", "").Replace("_", " ").Replace("(", " ").Replace(")", " ").Replace("[", " ").Replace("]", " ")
			//	.Replace("    ", " ").Replace("   ", " ").Replace("  ", " ")
			//	.Replace(" ", "")
			//	.Trim();

			var after = string.Empty;
			foreach (var ch in s.ToLower().Replace("&", string.Empty).Replace(" and ", string.Empty))
				if (char.IsLetterOrDigit(ch))
					after += ch;

			after = after.Replace('ü', 'u');

			return after;
		}
	}
}
