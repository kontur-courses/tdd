using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
	public class CircularCloudLayouter : ICloudLayouter
	{
		private readonly List<Rectangle> allRectangles;
		public Point center { get; }
		public IReadOnlyList<Rectangle> Rectangles => allRectangles;

		public CircularCloudLayouter(Point center)
		{
			this.center = center;
			allRectangles = new List<Rectangle>();
		}

		public Rectangle PutNextRectangle(Size rectangleSize)
		{
			if (SizeHaveZeroOrNegativeValue(rectangleSize))
				throw new ArgumentException("Size cannot be zero or negative");
			var mathSpiral = new MathematicalSpiral(center, Rectangles);
			var nextPointOnSpiral = mathSpiral.GetCoordinateOnSpiral(rectangleSize);
			var rectangle = new Rectangle(nextPointOnSpiral.X, nextPointOnSpiral.Y, rectangleSize.Width, rectangleSize.Height);
			allRectangles.Add(rectangle);
			return rectangle;
		}

		private bool SizeHaveZeroOrNegativeValue(Size size)
		{
			return size.Width <= 0 || size.Height <= 0;
		}
	}
}