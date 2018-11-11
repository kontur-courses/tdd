using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
	internal class MathematicalSpiral
	{
		private readonly double width = 0.1;
		private readonly IReadOnlyList<Rectangle> allRectangles;
		private double angle ;
		private Point center;
		private double radius;

		public MathematicalSpiral(Point center, IReadOnlyList<Rectangle> allRectangles)
		{
			this.center = center;
			this.allRectangles = allRectangles;
		}

		public Point GetCoordinateOnSpiral(Size rectangleSize)
		{
			while (true)
			{
				var newCoordinate = GetNextTrialCoordinateOnSpiral();
				var rectangle = CreateRectangleInSpiral(rectangleSize, newCoordinate);
				if (!IsIntersectWithExistingRectangles(rectangle))
				{
					newCoordinate = new Point(newCoordinate.X- rectangleSize.Width / 2, newCoordinate.Y - rectangleSize.Height / 2);
					return newCoordinate;
				}
			}
		}

		private Rectangle CreateRectangleInSpiral(Size rectangleSize, Point pointOnSpiral)
		{
			var x = pointOnSpiral.X - rectangleSize.Width / 2;
			var y = pointOnSpiral.Y - rectangleSize.Height / 2;
			var rectangle = new Rectangle(x, y, rectangleSize.Width, rectangleSize.Height);
			return rectangle;
		}

		private bool IsIntersectWithExistingRectangles(Rectangle rectangle)
		{
			foreach (var existingRectangle in allRectangles)
				if (rectangle.IntersectsWith(existingRectangle))
					return true;
			return false;
		}

		private Point GetNextTrialCoordinateOnSpiral()
		{
			angle += 0.01;
			radius = width * angle;
			var x = (int) (radius * Math.Cos(angle)) + center.X;
			var y = (int) (radius * Math.Sin(angle)) + center.Y;
			return new Point(x, y);
		}
	}
}