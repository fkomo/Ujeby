using FFMpegCore;
using System.Drawing;

namespace Ujeby.Graphics
{
	public static class VideoOutput
	{
		public static void SaveMapToImage(byte[][] map, int step, string fileNamePrefix, string outputDir)
		{
#pragma warning disable CA1416 // Validate platform compatibility
			var size = 32;
			int width = map[0].Length * size;
			int height = map.Length * size;

			using Bitmap bmp = new(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			using System.Drawing.Graphics gfx = System.Drawing.Graphics.FromImage(bmp);
			gfx.Clear(Color.Black);

			for (var y = 0; y < map.Length; y++)
				for (var x = 0; x < map[0].Length; x++)
				{
					var c = Math.Clamp(map[y][x] * 10, 0, 0xff);

					Point pt = new(x * size, y * size);
					Size sz = new(size, size);
					Rectangle rect = new(pt, sz);
					gfx.FillRectangle(
						new SolidBrush(Color.FromArgb(c, c, c)),
						rect);
				}

			bmp.Save(Path.Combine(outputDir, $"{fileNamePrefix}.{step.ToString("D3")}.png"));
#pragma warning restore CA1416 // Validate platform compatibility
		}

		public static void CreateVideoFromImages(string output, string inputDir, string inputImagePrefix)
		{
			var outputFiles = Directory.EnumerateFiles(inputDir, $"{inputImagePrefix}.*.png");
			FFMpeg.JoinImageSequence(output, frameRate: 60,
				outputFiles.Select(f => ImageInfo.FromPath(f)).ToArray());
			foreach (var outputFile in outputFiles)
				File.Delete(outputFile);
		}
	}
}
