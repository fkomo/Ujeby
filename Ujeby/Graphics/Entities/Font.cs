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

		public Text(string value, v4f color) : this(value)
		{
			Color = color;
		}

		public string Value;
        public v4f Color = Colors.White;

		public static EmptyLine EmptyLine => new();
    }

    public class Font
    {
        public string SpriteId;
        public string AABBSpriteId;
		public string OutlineSpriteId;

		public v2i CharSize;
        public v2i Spacing;

        public AABox2i[] CharBoxes;

		public v2i GetTextSize(params TextLine[] lines)
		{
			return GetTextSize(new(), new(2), lines);
		}

		public v2i GetTextSize(v2i spacing, v2i scale, params TextLine[] lines)
		{
			var size = v2i.Zero;
			foreach (var line in lines)
			{
				if (line is Text text)
				{
					var lineLength = 0;
					for (var i = 0; i < text.Value.Length; i++)
					{
						var charIndex = text.Value[i] - ' ';
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

    public enum HorizontalTextAlign
    {
        Left,
        Center,
        Right
    }

	public enum VerticalTextAlign
	{
		Top,
		Center,
		Bottom
	}
}
