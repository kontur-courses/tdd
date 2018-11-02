using System;
using System.Drawing;

namespace TagsCloudVisualization
{
	public class Spiral
	{
		private double degree;
		private int factor;
		private int step;

		public Spiral(int factor, int step)
		{
			this.factor = factor;
			this.step = step;
		}

		public void ChangeSpiralParametersIfIntersect()
		{
			factor++;
			degree += Math.PI / 18;
		}

		public void ChangeSpiralParameters(int counter)
		{
			if (counter >= 10)
				step = 1;

			factor--;
			degree += Math.PI / 18;
		}

		public Point GetNextPoint(Point center)
		{
			var x = center.X + (int)(step * factor * Math.Sin(degree));
			var y = center.Y + (int)(step * factor * Math.Cos(degree));

			return new Point(x, y);
		}
	}
}