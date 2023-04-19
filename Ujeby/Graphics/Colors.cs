using Ujeby.Vectors;

namespace Ujeby.Graphics
{
    public static class Colors
    {
        public static v4f[] All => new[]
        {
            White,
            Red,
            Yellow,
            Green,
            Cyan,
            Blue,
            Purple
        };

        public static v4f White = new(1);
        public static v4f Black = new(0, 0, 0, .7f);
		public static v4f DarkGray = new(.33f, .7f);
		public static v4f LightGray = new(.66f, .7f);

		public static v4f Red = new(1, 0, 0, .7f);
        public static v4f Green = new(0, 1, 0, .7f);
        public static v4f Blue = new(0, 0, 1, .7f);

        public static v4f Yellow = new(1, 1, 0, .7f);
        public static v4f Cyan = new(0, 1, 1, .7f);
        public static v4f Purple = new(1, 0, 1, .7f);
    }
}
