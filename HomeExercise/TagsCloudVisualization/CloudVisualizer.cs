using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System;

namespace TagsCloudVisualization
{
	public static class CloudVisualizer
	{
		private const int ImagePadding = 5;
		private const int RectangleBorderWidth = 1;

		private static readonly Color[] _colors = 
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
			rectangles = rectangles.Select(rect => MoveRectangleToImageCenter(rect, imageSize)).ToArray();

			var image = DrawRectangles(imageSize, rectangles);
			return image;
		}
		
		private static Bitmap DrawCenter(Bitmap image)
		{
			var gr = Graphics.FromImage(image);
			var linesColor = Pens.Red;
			
			gr.DrawLine(linesColor, 0, 0, image.Width, image.Height);
			gr.DrawLine(linesColor, 0, image.Height, image.Width, 0);
			return image;
		}

		internal static Rectangle MoveRectangleToImageCenter(Rectangle rectangle, Size imageSize)
		{
			var xOffset = imageSize.Width / 2;
			var yOffset = -2 * rectangle.Y + imageSize.Height / 2;
			rectangle.Offset(xOffset, yOffset);
			return rectangle;
		}

		private static Bitmap DrawRectangles(Size imageSize, IEnumerable<Rectangle> rectangles)
		{
			var image = new Bitmap(imageSize.Width, imageSize.Height);
			image = DrawCenter(image);
			var graphics = Graphics.FromImage(image);
			var random = new Random();
			foreach (var rectangle in rectangles)
				graphics.DrawRectangle(new Pen(GenerateColor(random), RectangleBorderWidth), rectangle);
			return image;
		}

		private static Color GenerateColor(Random random)
		{
			var colorIndex = random.Next(0, _colors.Length);
			return _colors[colorIndex];
		}

		private static Size CalculateImageSize(IEnumerable<Rectangle> rectangles)
		{
			int maxX = int.MinValue, maxY = int.MinValue;
			foreach (var rectangle in rectangles)
			{
				maxX = GetMaxNumber(rectangle.Left, rectangle.Right, maxX);
				maxY = GetMaxNumber(rectangle.Top, rectangle.GetBottom(), maxY);
			}

			var width = maxX * 2 + ImagePadding;
			var height = maxY * 2 + ImagePadding;
			return new Size(width, height);
		}

		private static int GetMaxNumber(int newNumber1, int newNumber2, int oldNumber)
		{
			var maxNumber = Math.Max(Math.Abs(newNumber1), Math.Abs(newNumber2));
			return maxNumber > oldNumber ? maxNumber : oldNumber;
		}
	}
}