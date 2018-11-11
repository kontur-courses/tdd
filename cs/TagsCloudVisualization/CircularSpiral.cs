using System;
using System.Drawing;

namespace TagsCloudVisualization
{
	internal class CircularSpiral
	{
		private readonly Point center;
		private readonly double width;
		private double angle;

		public CircularSpiral(Point center, double width)
		{
			this.center = center;
			this.width = width;
		}

		public Point GetNextCoordinate()
		{
			angle += 0.01;
			var radius = width * angle;
			var x = (int) (radius * Math.Cos(angle)) + center.X;
			var y = (int) (radius * Math.Sin(angle)) + center.Y;
			return new Point(x, y);
		}
	}
}