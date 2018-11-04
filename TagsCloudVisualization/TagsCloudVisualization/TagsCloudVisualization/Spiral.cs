using System;
using System.Drawing;

namespace TagsCloudVisualization
{
	public class Spiral
	{
		private readonly double degreeStep;
		private readonly double factorStep;
		private int index;

		public Spiral(double factorStep, double degreeStep)
		{
			this.factorStep = factorStep;
			this.degreeStep = degreeStep;
			index = 0;
		}

		public Point GetNextPoint(Point center)
		{
			var degree = degreeStep * index;
			var factor = factorStep * index;
			var x = center.X + (int)(factor * Math.Sin(degree));
			var y = center.Y + (int)(factor * Math.Cos(degree));
			index++;

			return new Point(x, y);
		}
	}
}