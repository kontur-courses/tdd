using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
	public static class RectanglesGenerator
	{
		public static Rectangle[] GenerateRectangles(int rectanglesCount, Size maxSize, Size minSize)
		{
			var sizes = GenerateSizes(rectanglesCount, maxSize, minSize);
			var layouter = new CircularCloudLayouter();
			var timer = new Stopwatch();
			timer.Start();
			var rectangles = sizes.Select(layouter.PutNextRectangle).ToArray();
			timer.Stop();
			Console.WriteLine(timer.Elapsed.TotalSeconds);
			return rectangles;
		}

		private static IEnumerable<Size> GenerateSizes(int count, Size maxSize, Size minSize)
		{
			var random = new Random();
			for (var i = 0; i < count; i++)
			{
				var height = random.Next(maxSize.Height, maxSize.Height);
				var width = random.Next(Math.Max(minSize.Width, height), maxSize.Width);
				yield return new Size(width, height);
			}
		}
	}
}