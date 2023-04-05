using System.Runtime.InteropServices;

namespace Ujeby.Tools
{
	public class Mem
	{
		public static unsafe void Copy(uint[] source, IntPtr ptrDest, uint length)
		{
			fixed (uint* ptrSource = &source[0])
			{
				RtlMoveMemory(ptrDest, (IntPtr)ptrSource, length);
			}
		}

		[DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = false)]
		static extern void RtlMoveMemory(IntPtr Destination, IntPtr Source, uint Length);
	}
}
