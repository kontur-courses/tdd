using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
	public class CircularCloudLayouter
	{
		private readonly Point center;
		private readonly Spiral spiral;
		public List<Rectangle> rectangles;
		public int overrideCounter;

		public CircularCloudLayouter(Point center)
		{
			this.center = center;
			spiral = new Spiral(1, 2);
			rectangles = new List<Rectangle>();
		}

		public Rectangle PutNextRectangle(Size size)
		{
			if (size.Width <= 0 || size.Height <= 0)
				throw new ArgumentException();

			spiral.ChangeSpiralParameters(overrideCounter);

			var point = spiral.GetNextPoint(center);
			var nextRect = GetRectangle(size, point);

			while (IsIntersect(nextRect))
			{
				spiral.ChangeSpiralParametersIfIntersect();
				point = spiral.GetNextPoint(center);
				nextRect = GetRectangle(size, point);
				overrideCounter++;
			}

			rectangles.Add(nextRect);

			return nextRect;
		}

		private static Rectangle GetRectangle(Size size, Point point) =>
			new Rectangle(point.X, point.Y, size.Width, size.Height);

		private bool IsIntersect(Rectangle rect) =>
			rectangles.Count(r => r.IntersectsWith(rect)) > 0;
	}
}