using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;

namespace TagsCloudVisualization
{
	public static class CloudVisualizator
	{
		private const int ImagePadding = 5;

		public static void Visualize(string fileName, IEnumerable<Rectangle> rectangles=null)
		{
			var layouter = new CircularCloudLayouter();
			var maxSize = new Size(100, 50);
			var minSize = new Size(10, 10);
			var sizes = RectangleSizeGenerator.GenerateSizes(400, maxSize, minSize);
			if (rectangles == null)
				rectangles = sizes.Select(layouter.PutNextRectangle).ToArray();
			var imageSize = CalculateImageSize(rectangles);
			rectangles = rectangles.Select(rect => MoveRectangleToImageCenter(rect, imageSize)).ToArray();

			var image = DrawCenter(imageSize);
			image = DrawRectangles(image, rectangles);
			image.Save(fileName);
			Console.WriteLine($"Image saved to {fileName}");
		}

		private static Bitmap DrawCenter(Size imageSize)
		{
			var image = new Bitmap(imageSize.Width, imageSize.Height);
			var gr = Graphics.FromImage(image);
			gr.DrawLine(Pens.Yellow, 0, 0, image.Width, image.Height);
			gr.DrawLine(Pens.Yellow, 0, image.Height, image.Width, 0);
			return image;
		}

		internal static Rectangle MoveRectangleToImageCenter(Rectangle rectangle, Size imageSize)
		{
			var xOffset = imageSize.Width / 2;
			var yOffset = -2 * rectangle.Y + imageSize.Height / 2;
			rectangle.Offset(xOffset, yOffset);
			return rectangle;
		}

		private static Bitmap DrawRectangles(Bitmap image, IEnumerable<Rectangle> rectangles)
		{
			var graphics = Graphics.FromImage(image);
			foreach (var rectangle in rectangles)
				graphics.DrawRectangle(Pens.DarkRed, rectangle);
			return image;
		}

		private static Size CalculateImageSize(IEnumerable<Rectangle> rectangles)
		{
			int maxX = int.MinValue, maxY = int.MinValue;
			int minX = int.MaxValue, minY = int.MaxValue;
			foreach (var rectangle in rectangles)
			{
				maxX = rectangle.Right > maxX ? rectangle.Right : maxX;
				maxY = rectangle.Top > maxY ? rectangle.Top : maxY;
				minX = rectangle.Left < minX ? rectangle.Left : minX;
				minY = rectangle.GetBottom() < minY ? rectangle.GetBottom() : minY;
			}

			var width = MakeNumberEven(maxX + Math.Abs(minX) + ImagePadding);
			var height = MakeNumberEven(maxY + Math.Abs(minY) + ImagePadding);
			return new Size(width, height);
		}

		private static int MakeNumberEven(int number) => number % 2 == 0 ? number : ++number;
	}

	public static class RectangleSizeGenerator
	{
		public static IEnumerable<Size> GenerateSizes(int count, Size maxSize, Size minSize)
		{
			var randomizer = new Random();
			for (var i = 0; i < count; i++)
			{
				var height = randomizer.Next(maxSize.Height, maxSize.Height);
				var width = randomizer.Next(Math.Max(minSize.Width, height), maxSize.Width);
				yield return new Size(width, height);
			}
		}
	}
}