using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
	public class CircularCloudLayouter
	{
		public readonly List<Rectangle> AllRectangles;
		public readonly Point center;
		private double widthOfSpiral;
		private double angle;
		private double radius;

		public CircularCloudLayouter(Point center)
		{
			this.center = center;
			AllRectangles = new List<Rectangle>();
		}

		public Rectangle PutNextRectangle(Size rectangleSize)
		{
			var x = 0;
			var y = 0;
			var rectangle = new Rectangle();
			if (AllRectangles.Count == 0)
			{
				var firstRectangle = PutFirstRectangle(rectangleSize);
				angle = Math.Atan2(firstRectangle.Y,firstRectangle.X);
				widthOfSpiral = 1;
				return firstRectangle;
			}
			var newCoord = GetNewCoord();
			while (true)
			{
				rectangle = new Rectangle(newCoord.X + rectangleSize.Width / 2, newCoord.Y + rectangleSize.Height / 2, rectangleSize.Width,
					rectangleSize.Height);
				var countNonIntersectRect = 0;
				for (var i = 0; i < AllRectangles.Count; i++)
				{
					if (rectangle.IntersectsWith(AllRectangles[i]))
					{
						break;
					}
					countNonIntersectRect++;
				}
				if (countNonIntersectRect==AllRectangles.Count)
					break;
				angle -= 0.1;
				newCoord = GetNewCoord();
			}
			AllRectangles.Add(rectangle);
			return rectangle;
		}

		private Point GetNewCoord()
		{
			radius = widthOfSpiral * angle;
			var x = (int)(radius * Math.Cos(angle));
			var y = (int)(radius * Math.Sin(angle));
			return new Point(x,y);
		}

		private Rectangle PutFirstRectangle(Size rectangleSize)
		{
			var x = center.X - rectangleSize.Width / 2;
			var y = center.Y - rectangleSize.Height / 2;
			var rectangle = new Rectangle(x, y, rectangleSize.Width, rectangleSize.Height);
			AllRectangles.Add(rectangle);
			return rectangle;
		}
	}
}