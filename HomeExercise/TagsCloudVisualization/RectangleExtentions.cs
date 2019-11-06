using System;
using System.Drawing;

namespace TagsCloudVisualization
{
	public static class RectangleExtensions
	{
		public static double GetDiagonalLength(this Size rectangleSize) => 
			Math.Sqrt(Math.Pow(rectangleSize.Width, 2) + Math.Pow(rectangleSize.Height, 2));

		public static Point GetCenter(this Rectangle rectangle)
		{
			var centerX = rectangle.X + rectangle.Width / 2;
			var centerY = rectangle.Y - rectangle.Height / 2;
			return new Point(centerX, centerY);
		}
	}
}