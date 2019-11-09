using System;
using System.Drawing;

namespace TagsCloudVisualization
{
	internal class Spiral
	{
		private double _currentAngle;
		private readonly Point _center;
		private readonly double _angleStep;
		private readonly double _verticalDensity;
		private readonly double _horizontalDensity;

		public Spiral(double angleStep, double verticalDensity, double horizontalDensity, Point center)
		{
			_angleStep = angleStep;
			_verticalDensity = verticalDensity;
			_horizontalDensity = horizontalDensity;
			_center = center;
		}

		public Point GetNextPoint()
		{
			var x = Math.Round(_horizontalDensity * _currentAngle * Math.Sin(_currentAngle)) + _center.X;
			var y = Math.Round(_verticalDensity * _currentAngle * Math.Cos(_currentAngle)) + _center.Y;
			_currentAngle += _angleStep;
			return new Point((int) x, (int) y);
		}
	}
}