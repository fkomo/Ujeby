using Ujeby.Vectors;

namespace Ujeby.Graphics.Entities
{
    public abstract class TextLine
    {
    }

    public class EmptyLine : TextLine
    {
    }

    public class Text : TextLine
    {
        public Text(string value)
        {
            Value = value;
        }

        public string Value;
        public v4f Color = Colors.White;
    }

    public class Font
    {
        public string SpriteId;
        public string DataSpriteId;

        public v2i CharSize;
        public v2i Spacing;

        public AABBi[] CharBoxes;

		public v2i GetTextSize(v2i spacing, params TextLine[] lines)
		{
			var scale = new v2i(2, 2);

			var size = v2i.Zero;
			foreach (var line in lines)
			{
				if (line is Text text)
				{
					var lineLength = 0;
					for (var i = 0; i < text.Value.Length; i++)
					{
						var charIndex = (int)text.Value[i] - 32;
						var charAabb = CharBoxes[charIndex];

						lineLength += (int)(charAabb.Size.X + Spacing.X + spacing.X);
					}
					size.X = Math.Max(lineLength, size.X);
					size.Y += CharSize.Y + Spacing.Y + spacing.Y;
				}
				else if (line is EmptyLine)
				{
					size.Y += CharSize.Y + Spacing.Y + spacing.Y;
				}
			}

			return size * scale;
		}
	}

    public enum TextAlign
    {
        Left,
        Center,
        Right
    }
}