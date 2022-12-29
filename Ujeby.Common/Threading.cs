using System.Threading;

namespace Ujeby.Common
{
	public class Threading
	{
		/// <summary>
		/// thread body delegate
		/// </summary>
		public delegate void ThreadBody();

		/// <summary>
		/// create and start new instance of Thread/ThreadStart with given body
		/// </summary>
		/// <param name="threadBody"></param>
		public static void ThreadAndForget(ThreadBody threadBody)
		{
			new Thread(
				new ThreadStart(() =>
				{
					try
					{
						threadBody();
					}
					catch (System.Threading.Tasks.TaskCanceledException)
					{

					}
				})).Start();
		}
	}
}
