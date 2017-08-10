using System;
using System.Collections.Generic;

namespace TagsCloudVisualization
{
	static class RectangleExtensions
	{
		public static double GetSquare(this Rectangle rectangle)
		{
			if (rectangle.Width < 0 || rectangle.Height < 0)
				throw new InvalidOperationException();
			return rectangle.Width * rectangle.Height;
		}

		public static Rectangle? IntersectWith(this Rectangle thisRectangle, Rectangle thatRectangle)
		{
			var left = Math.Max(thisRectangle.X, thatRectangle.X);
			var right = Math.Min(thisRectangle.X + thisRectangle.Width, thatRectangle.X + thatRectangle.Width);
			if (left > right)
				return null;

			var top = Math.Max(thisRectangle.Y, thatRectangle.Y);
			var bottom = Math.Min(thisRectangle.Y + thisRectangle.Height, thatRectangle.Y + thatRectangle.Height);
			if (top > bottom)
				return null;

			return new Rectangle(left, top, right - left, bottom - top);
		}

		public static bool IsIntersectedWith(this Rectangle thisRectangle, Rectangle thatRectangle)
		{
			var intersection = thisRectangle.IntersectWith(thatRectangle);
			return intersection.HasValue
			       && intersection.Value.Width > 0
			       && intersection.Value.Height > 0;
		}

		public static IList<Point> GetPoints(this Rectangle rectangle)
		{
			return new[]
			{
				new Point(rectangle.X, rectangle.Y),
				new Point(rectangle.X + rectangle.Width, rectangle.Y),
				new Point(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height),
				new Point(rectangle.X, rectangle.Y + rectangle.Height),
			};
		}
	}
}