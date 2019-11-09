using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System;

namespace TagsCloudVisualization
{
	public static class CloudVisualizer
	{
		internal const int ImagePadding = 5;
		private const int RectangleBorderWidth = 1;

		private static readonly Color[] _possibleRectangleColors = 
		{
			Color.DarkBlue,
			Color.DarkGreen,
			Color.Purple,
			Color.DarkRed,
			Color.DarkMagenta,
			Color.DarkTurquoise,
			Color.Red,
			Color.DarkOrange,
		};

		public static Image Visualize(Rectangle[] rectangles)
		{
			var imageSize = CalculateImageSize(rectangles);
			var movedRectangles = rectangles.Select(r => MoveToImageCenter(r, imageSize));
			
			var image = new Bitmap(imageSize.Width, imageSize.Height);
			image = DrawDiagonals(image);
			image = DrawRectangles(image, movedRectangles);
			return image;
		}

		internal static Size CalculateImageSize(IEnumerable<Rectangle> rectangles)
		{
			int maxX = int.MinValue, maxY = int.MinValue;
			foreach (var rectangle in rectangles)
			{
				maxX = GetAbsoluteMax(rectangle.Left, rectangle.Right, maxX);
				maxY = GetAbsoluteMax(rectangle.Top, rectangle.GetBottom(), maxY);
			}

			var width = maxX * 2 + ImagePadding;
			var height = maxY * 2 + ImagePadding;
			return new Size(width, height);
		}
		
		internal static Rectangle MoveToImageCenter(Rectangle rectangle, Size imageSize)
		{
			var xOffset = imageSize.Width / 2;
			var yOffset = -2 * rectangle.Y + imageSize.Height / 2;
			rectangle.Offset(xOffset, yOffset);
			return rectangle;
		}

		private static Bitmap DrawDiagonals(Bitmap image)
		{
			var graphics = Graphics.FromImage(image);
			var lineColor = Pens.Red;
			
			graphics.DrawLine(lineColor, 0, 0, image.Width, image.Height);
			graphics.DrawLine(lineColor, 0, image.Height, image.Width, 0);
			return image;
		}

		private static Bitmap DrawRectangles(Bitmap image, IEnumerable<Rectangle> rectangles)
		{
			var graphics = Graphics.FromImage(image);
			var random = new Random();
			foreach (var rectangle in rectangles)
				graphics.DrawRectangle(new Pen(GenerateColor(random), RectangleBorderWidth), rectangle);
			return image;
		}

		private static Color GenerateColor(Random random)
		{
			var colorIndex = random.Next(0, _possibleRectangleColors.Length);
			return _possibleRectangleColors[colorIndex];
		}

		internal static int GetAbsoluteMax(int number1, int number2, int previousMax) =>
			Math.Max(Math.Max(Math.Abs(number1), Math.Abs(number2)), previousMax);
	}
}