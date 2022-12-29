using Ujeby.Vectors;

namespace Ujeby.Graphics.Entities
{
    public class Sprite
    {
        /// <summary>internal sprite id</summary>
        public string Id;

        public string Filename;

        /// <summary>width x height</summary>
        public v2i Size;

        /// <summary>
        /// pixel format: 0xARGB
        /// topLeft -> bottomRight
        /// </summary>
        public uint[] Data;

        /// <summary>sdl2 image pointer</summary>
        public IntPtr ImagePtr = IntPtr.Zero;

        /// <summary>sdl2 texture pointer</summary>
        public IntPtr TexturePtr = IntPtr.Zero;
    }
}
