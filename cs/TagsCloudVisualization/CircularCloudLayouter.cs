using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
	public class CircularCloudLayouter
	{
		private readonly Point center;
		private readonly double widthOfSpiral = 1;
		private readonly List<Rectangle> allRectangles;
		private double angle;
		private double radius;

		public CircularCloudLayouter(Point center)
		{
			this.center = center;
			allRectangles = new List<Rectangle>();
		}

		public IReadOnlyList<Rectangle> Rectangles => allRectangles;

		public Rectangle PutNextRectangle(Size rectangleSize)
		{
			if (allRectangles.Count == 0)
			{
				var firstRectangle = PutFirstRectangle(rectangleSize);
				angle = Math.Atan2(firstRectangle.Y, firstRectangle.X);
				return firstRectangle;
			}
			var rectangle = PutRectangle(rectangleSize);
			allRectangles.Add(rectangle);
			return rectangle;
		}

		private Rectangle PutRectangle(Size rectangleSize)
		{
			while (true)
			{
				var newCoordinate = GetNewCoordinate();
				var x = newCoordinate.X + rectangleSize.Width / 2;
				var y = newCoordinate.Y + rectangleSize.Height / 2;
				var rectangle = new Rectangle(x, y, rectangleSize.Width, rectangleSize.Height);
				if (!IsIntersect(rectangle))
					return rectangle;
				angle += 0.1;
			}
		}

		private bool IsIntersect(Rectangle rectangle)
		{
			foreach (var existingRectangle in allRectangles)
				if (rectangle.IntersectsWith(existingRectangle))
					return true;
			return false;
		}

		private Point GetNewCoordinate()
		{
			radius = widthOfSpiral * angle;
			var x = (int) (radius * Math.Cos(angle));
			var y = (int) (radius * Math.Sin(angle));
			return new Point(x, y);
		}

		private Rectangle PutFirstRectangle(Size rectangleSize)
		{
			var x = center.X - rectangleSize.Width / 2;
			var y = center.Y - rectangleSize.Height / 2;
			var rectangle = new Rectangle(x, y, rectangleSize.Width, rectangleSize.Height);
			allRectangles.Add(rectangle);
			return rectangle;
		}
	}
}