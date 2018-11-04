using System;
using System.Drawing;

namespace TagsCloudVisualization
{
	public class Spiral
	{
		private readonly double degreeStep;
		private readonly double factorStep;
		private int nextPointCounter;

		public Spiral(double factorStep, double degreeStep)
		{
			this.factorStep = factorStep;
			this.degreeStep = degreeStep;
			nextPointCounter = 0;
		}

		public Point GetNextPoint(Point center)
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