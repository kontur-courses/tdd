using System;
using System.Drawing;

namespace TagsCloudVisualization.Curves
{
	public class Astroid : ICurve
	{
		private readonly double degreeStep;
		private readonly double factorStep;
		private int nextPointCounter;
		private Point center;

		public Astroid(double factorStep, double degreeStep, Point center)
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
			var x = center.X + (int)(factor * Math.Pow(Math.Cos(degree), 3));
			var y = center.Y + (int)(factor * Math.Pow(Math.Sin(degree), 3));
			nextPointCounter++;

			return new Point(x, y);
		}
	}
}