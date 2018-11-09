using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
	public class CircularCloudLayouter
	{
		private readonly List<Rectangle> allRectangles;
		private double angle;
		private double radius;
		private readonly double width = 0.1;

		public CircularCloudLayouter(Point center)
		{
			this.center = center;
			allRectangles = new List<Rectangle>();
		}

		public Point center { get; }
		public IReadOnlyList<Rectangle> Rectangles => allRectangles;

		public Rectangle PutNextRectangle(Size rectangleSize)
		{
			if (SizeHaveZeroOrNegativeValue(rectangleSize))
				throw new ArgumentException("Size cannot be zero or negative");
			if (allRectangles.Count == 0)
			{
				var firstRectangle = CreateRectangle(rectangleSize, center);
				allRectangles.Add(firstRectangle);
				angle = Math.Atan2(firstRectangle.Y, firstRectangle.X);
				return firstRectangle;
			}
			var rectangle = PutRectangle(rectangleSize);
			allRectangles.Add(rectangle);
			return rectangle;
		}

		private bool SizeHaveZeroOrNegativeValue(Size size)
		{
			return size.Width <= 0 || size.Height <= 0;
		}

		private Rectangle PutRectangle(Size rectangleSize)
		{
			while (true)
			{
				var newCoordinate = GetNewCoordinate();
				var rectangle = CreateRectangle(rectangleSize, newCoordinate);
				if (!IsIntersectWithExistingRectangles(rectangle))
					return rectangle;
			}
		}

		private bool IsIntersectWithExistingRectangles(Rectangle rectangle)
		{
			foreach (var existingRectangle in allRectangles)
				if (rectangle.IntersectsWith(existingRectangle))
					return true;
			return false;
		}

		private Point GetNewCoordinate()
		{
			angle += 0.01;
			radius = width * angle;
			var x = (int) (radius * Math.Cos(angle)) + center.X;
			var y = (int) (radius * Math.Sin(angle)) + center.Y;
			return new Point(x, y);
		}

		private Rectangle CreateRectangle(Size rectangleSize, Point coordinate)
		{
			var x = coordinate.X - rectangleSize.Width / 2;
			var y = coordinate.Y - rectangleSize.Height / 2;
			var rectangle = new Rectangle(x, y, rectangleSize.Width, rectangleSize.Height);
			return rectangle;
		}
	}
}