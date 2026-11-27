using System.Drawing;

namespace TagsCloudVisualization.Geometry
{
	public static class RectangleExtensions
	{
		public static Rectangle CenteredAt(this Size size, Point center)
		{
			return new Rectangle(center.X - size.Width / 2, center.Y - size.Height / 2, size.Width, size.Height);
		}

		public static Point Center(this Rectangle rectangle)
		{
			return new Point(rectangle.Left + rectangle.Width / 2, rectangle.Top + rectangle.Height / 2);
		}

		public static bool IntersectsWithAny(this Rectangle rectangle, IEnumerable<Rectangle> others)
		{ 
			return others.Any(rectangle.IntersectsWith);
		}
	}
}
