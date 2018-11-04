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
		private readonly List<Rectangle> rectangles;

		public CircularCloudLayouter(Point center)
		{
			this.center = center;
			spiral = new Spiral(factorStep: 0.5, degreeStep: Math.PI / 18);
			rectangles = new List<Rectangle>();
		}

		public Rectangle PutNextRectangle(Size size)
		{
			if (size.Width <= 0 || size.Height <= 0)
				throw new ArgumentException("Lengths of size must be positive");

			var nextRect = GetNextNotIntersectRectangle(size);
			rectangles.Add(nextRect);

			return nextRect;
		}

		public Rectangle GetNextNotIntersectRectangle(Size size)
		{
			var nextRect = GetNextRectangle(size);
			while (IsIntersectWithExistRectangles(nextRect))
				nextRect = GetNextRectangle(size);

			return nextRect;
		}

		private Rectangle GetNextRectangle(Size size)
		{
			var nextPoint = spiral.GetNextPoint(center);
			return new Rectangle(nextPoint.X, nextPoint.Y, size.Width, size.Height);
		}

		public bool IsIntersectWithExistRectangles(Rectangle rect) =>
			rectangles.Any(r => r.IntersectsWith(rect));

		public Rectangle[] GetExistRectangles() => rectangles.ToArray();
	}
}