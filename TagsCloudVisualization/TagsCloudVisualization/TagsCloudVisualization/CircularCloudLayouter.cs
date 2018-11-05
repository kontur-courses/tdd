using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
	public class CircularCloudLayouter
	{
		private readonly Spiral spiral;
		private readonly List<Rectangle> rectangles;

		public CircularCloudLayouter(Point center)
		{
			spiral = new Spiral(factorStep: 0.5, degreeStep: Math.PI / 18, center: center);
			rectangles = new List<Rectangle>();
		}

		public Rectangle PutNextRectangle(Size size)
		{
			if (size.Width <= 0 || size.Height <= 0)
				throw new ArgumentException("Lengths of size must be positive");

			var nextRect = GetNextNotIntersectingRectangle(size);
			rectangles.Add(nextRect);

			return nextRect;
		}

		private Rectangle GetNextNotIntersectingRectangle(Size size)
		{
			Rectangle nextRect;
			do
			{
				nextRect = GetNextRectangle(size);
			} while (IntersectsWithExistingRectangles(nextRect));

			return nextRect;
		}

		private Rectangle GetNextRectangle(Size size)
		{
			var nextPoint = spiral.GetNextPoint();
			return new Rectangle(nextPoint.X, nextPoint.Y, size.Width, size.Height);
		}

		private bool IntersectsWithExistingRectangles(Rectangle rect) =>
			rectangles.Any(r => r.IntersectsWith(rect));

		public Rectangle[] GetRectangles() => rectangles.ToArray();
	}
}