using System.Text;
using System.Text.RegularExpressions;

namespace Ujeby.Tools.StringExtensions
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
			var result = new List<long>();
			for (var i = 0; i < s.Length; i++)
			{
				if (char.IsDigit(s[i]) || s[i] == '-')
				{
					sb.Append(s[i]);
					for (++i; i < s.Length && char.IsDigit(s[i]); i++)
						sb.Append(s[i]);
				}

				if (long.TryParse(sb.ToString(), out long l))
					result.Add(l);

				sb.Clear();
			}

			return result.ToArray();
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

		/// <summary>
		/// returns index of corresponding opening bracket (with nesting in mind)
		/// </summary>
		/// <param name="s"></param>
		/// <param name="closingBracketIndex"></param>
		/// <param name="openingBrackets"></param>
		/// <param name="closingBrackets"></param>
		/// <returns></returns>
		public static int IndexOfOpeningBracket(this string s, int closingBracketIndex, 
			string openingBrackets = "{([", string closingBrackets = "})]")
		{
			var i = closingBracketIndex;
			var nest = new int[openingBrackets.Length];

			do
			{
				var nCloseId = closingBrackets.IndexOf(s[i]);
				if (nCloseId != -1)
					nest[nCloseId]++;

				else
				{
					var nOpenId = openingBrackets.IndexOf(s[i]);
					if (nOpenId != -1)
						nest[nOpenId]--;
				}

				i--;
			}
			while (nest.Any(n => n > 0));

			return i + 1;
		}

		/// <summary>
		/// returns index of corresponding closing bracket (with nesting in mind)
		/// </summary>
		/// <param name="s"></param>
		/// <param name="openingBracketIndex"></param>
		/// <param name="openingBrackets"></param>
		/// <param name="closingBrackets"></param>
		/// <returns></returns>
		public static int IndexOfClosingBracket(this string s,
			int openingBracketIndex = 0, string openingBrackets = "{([", string closingBrackets = "})]")
		{
			var i = openingBracketIndex;
			var nest = new int[openingBrackets.Length];
			do
			{
				var nOpenId = openingBrackets.IndexOf(s[i]);
				if (nOpenId != -1)
					nest[nOpenId]++;
				else
				{
					var nCloseId = closingBrackets.IndexOf(s[i]);
					if (nCloseId != -1)
						nest[nCloseId]--;
				}

				i++;
			}
			while (nest.Any(n => n > 0));

			return i - 1;
		}

		/// <summary>
		/// returns tag defined by start/end
		/// </summary>
		/// <param name="s"></param>
		/// <param name="startTag"></param>
		/// <param name="endTag"></param>
		/// <returns></returns>
		public static string GetTag(this string s, string startTag, string endTag)
		{
			var regexp = $"{startTag}(.*?){endTag}";
			var match = Regex.Match(s, regexp);

			return match.Success ? match.Value : null;
		}

		/// <summary>
		/// returns only letters and digits from string
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string LettersOrDigitsOnly(this string s)
			=> s == null ? s : new(s.Where(c => char.IsLetterOrDigit(c)).ToArray());

		/// <summary>
		/// returns only letters from string
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string LettersOnly(this string s)
			=> s == null ? s : new(s.Where(c => char.IsLetter(c)).ToArray());

		/// <summary>
		/// remove tags from string (defined by start/end tag)
		/// </summary>
		/// <param name="s"></param>
		/// <param name="tags"></param>
		/// <returns></returns>
		public static string StripTags(this string s, params (string Start, string End)[] tags)
		{
			if (string.IsNullOrEmpty(s))
				return null;

			foreach (var tag in tags)
				s = Regex.Replace(s, $"{tag.Start}.*?{tag.End}", string.Empty);

			return s;
		}
	}
}
