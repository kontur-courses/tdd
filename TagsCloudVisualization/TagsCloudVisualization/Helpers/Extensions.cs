using System.Drawing;

namespace TagsCloudVisualization
{
	public static class PointExtensions
	{
		public static Point Plus(this Point point, Point otherPoint)
		{
			return new Point(point.X + otherPoint.X, point.Y + otherPoint.Y);
		}

		public static Point Minus(this Point point, Point otherPoint)
		{
			return new Point(point.X - otherPoint.X, point.Y - otherPoint.Y);
		}
	}

	public static class RectangleExtensions
	{
		public static Point Center(this Rectangle rectangle)
		{
			var x = rectangle.X + rectangle.Width / 2;
			var y = rectangle.Y + rectangle.Height / 2;
			return new Point(x, y);
		}
	}
}
