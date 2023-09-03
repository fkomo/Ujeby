namespace Ujeby.Tools
{
	public static class Strings
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="duration">duration in ms</param>
		/// <returns></returns>
		public static string DurationToStringSimple(double duration)
		{
			if (duration < 1)
				return $"{(int)(duration * 1000)}us";

			else if (duration < 1000)
				return $"{(int)duration}ms";

			else if (duration < 60 * 1000)
				return $">{(int)(duration / 1000)}s";

			else if (duration < 60 * 60 * 1000)
				return $">{(int)(duration / (60 * 1000))}min";

			return $">1h";
		}

		public static string DurationString(TimeSpan span)
		{
			if (span.TotalSeconds < 1)
				return null;

			if (span.TotalSeconds < 60)
				return $"{NumToCountableString((int)span.TotalSeconds, "second", "seconds")}";

			else if (span.TotalMinutes < 60)
				return $"{NumToCountableString((int)span.TotalMinutes, "minute", "minutes")}";

			return $"{NumToCountableString((int)span.TotalHours, "hour", "hours")}" + (span.Minutes > 0 ? $" and {NumToCountableString((int)span.Minutes, "minute", "minutes")}" : null);
		}

		public static string TimeStringSince(DateTime value)
		{
			var d = DateTime.Now.Subtract(value);

			if (d.TotalSeconds < 60)
				return $"{NumToCountableString((int)d.TotalSeconds, "second", "seconds")} ago";

			else if (d.TotalMinutes < 60)
				return $"{NumToCountableString((int)d.TotalMinutes, "minute", "minutes")} ago";

			else if (d.TotalHours < 24)
				return $"{NumToCountableString((int)d.TotalHours, "hour", "hours")} ago";

			else if (d.TotalDays < 365)
				return $"{NumToCountableString((int)d.TotalDays, "day", "days")} ago";

			return $"more than {NumToCountableString((int)(d.TotalDays / 365), "year", "years")} ago";
		}

		public static string[] MemUnits { get; private set; } = new string[]
		{
			"B",
			"KB",
			"MB",
			"GB",
			"TB",
			"PB"
		};

		public static string SizeString(long length)
		{
			var memUnitIndex = 0;
			var floatLength = (float)length;

			while (floatLength > 1024)
			{
				floatLength /= 1024;
				memUnitIndex++;
			}

			return $"~{(int)floatLength}{MemUnits[memUnitIndex]}";
		}

		public static string NumToCountableString(int num, string one, string zeroOrMore)
		{
			if (num == 1)
				return $"{num} {one}";

			return $"{num} {zeroOrMore}";
		}

		public static string NumToCountableString(int num)
		{
			switch (num)
			{
				case 1: return "once";
				case 2: return "twice";
				default:
					return $"{num} times";
			}
		}

		/// <summary>
		/// https://gist.github.com/wickedshimmy/449595/cb33c2d0369551d1aa5b6ff5e6a802e21ba4ad5c
		/// </summary>
		/// <param name="original"></param>
		/// <param name="modified"></param>
		/// <returns></returns>
		public static int DamerauLevenshteinEditDistance(string original, string modified)
		{
			var len_orig = original.Length;
			var len_diff = modified.Length;

			var matrix = new int[len_orig + 1, len_diff + 1];
			for (int i = 0; i <= len_orig; i++)
				matrix[i, 0] = i;
			for (int j = 0; j <= len_diff; j++)
				matrix[0, j] = j;

			for (int i = 1; i <= len_orig; i++)
			{
				for (int j = 1; j <= len_diff; j++)
				{
					int cost = modified[j - 1] == original[i - 1] ? 0 : 1;
					var vals = new int[] {
						matrix[i - 1, j] + 1,
						matrix[i, j - 1] + 1,
						matrix[i - 1, j - 1] + cost
					};
					matrix[i, j] = vals.Min();
					if (i > 1 && j > 1 && original[i - 1] == modified[j - 2] && original[i - 2] == modified[j - 1])
						matrix[i, j] = System.Math.Min(matrix[i, j], matrix[i - 2, j - 2] + cost);
				}
			}

			return matrix[len_orig, len_diff];
		}
	}
}
