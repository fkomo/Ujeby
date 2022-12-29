using Ujeby.Vectors;

namespace Ujeby.Graphics
{
	public static class HeatMap
	{
		private static readonly v4f[] _colors = new[]
		{
			//new v4f(0, 0, 0, 1),

			new v4f(0, 0, 1, 1),
			new v4f(0, 1, 1, 1),
			new v4f(0, 1, 0, 1),
			new v4f(1, 1, 0, 1),
			new v4f(1, 0, 0, 1),

			//new v4f(1, 1, 1, 1),
		};

		public static v4f GetColorForValue(double value, double maxValue, 
			double alpha = 1)
		{
			var valuePercentage = value / maxValue;

			// % of each block of color. the last is the "100% Color"
			var colorPercentage = 1d / (_colors.Length - 1);

			// the integer part repersents how many block to skip
			var blockOfColor = valuePercentage / colorPercentage;
			var blockIdx = (int)Math.Truncate(blockOfColor);

			// remove the part represented of block 
			var valPercResidual = valuePercentage - (blockIdx * colorPercentage);

			// % of color of this block that will be filled
			var percOfColor = valPercResidual / colorPercentage;

			var cTarget = _colors[blockIdx];
			var cNext = _colors[blockIdx + 1];

			var delta = cNext - cTarget;

			var result = cTarget + (delta * percOfColor);
			result.W = alpha;

			return result;
		}
	}
}
