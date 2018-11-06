using System;
using System.Drawing;

namespace TagsCloudVisualization
{
	class Heart : ICurve
	{
		private readonly double degreeStep;
		private readonly double factorStep;
		private int nextPointCounter;
		private Point center;

		public Heart(double factorStep, double degreeStep, Point center)
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
			var x = center.X + (int)(1.3*factor * Math.Cos(degree));
			var y = center.Y + (int)(-factor * (Math.Sin(degree)+Math.Sqrt(Math.Abs(Math.Cos(degree)))));
			nextPointCounter++;

			return new Point(x, y);
		}
	}
}
