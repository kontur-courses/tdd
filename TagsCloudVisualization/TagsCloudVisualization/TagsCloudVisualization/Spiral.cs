using System;
using System.Drawing;

namespace TagsCloudVisualization
{
	public class Spiral
	{
		private readonly double degreeStep;
		private readonly double factorStep;
		private int nextPointCounter;
		private Point center;

		public Spiral(double factorStep, double degreeStep, Point center)
		{
			this.factorStep = factorStep;
			this.degreeStep = degreeStep;
			this.center = center;
			nextPointCounter = 0;
		}

		public Point GetNextPoint()
		{
			var degree = degreeStep * nextPointCounter;
			var factor = factorStep * nextPointCounter;
			var x = center.X + (int)(factor * Math.Sin(degree));
			var y = center.Y + (int)(factor * Math.Cos(degree));
			nextPointCounter++;

			return new Point(x, y);
		}
	}
}