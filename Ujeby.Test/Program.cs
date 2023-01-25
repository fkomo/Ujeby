using BenchmarkDotNet.Running;
using System;
using Ujeby.Graphics.Sdl;
using Ujeby.Test.Benchmarks;
using Ujeby.Tools;
using Ujeby.Vectors;

namespace Ujeby.Test
{
	internal class Program
	{
		static void Main()
		{
			try
			{
				// benchmarks
				//BenchmarkRunner.Run<IntVsString>();

				v2i windowSize = new(1920, 1080);

				Sdl2Wrapper.Init("AoC.Vis", windowSize);
				new Sdl2LoopTest(windowSize).Run(HandleInput);

				//new OpenTKLoopTest(windowSize, "AoC.Vis").Run();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			finally
			{
				Sdl2Wrapper.Destroy();
			}
		}

		private static bool HandleInput()
		{
			return true;
		}
	}
}
