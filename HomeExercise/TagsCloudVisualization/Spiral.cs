using System;
using System.Drawing;

namespace TagsCloudVisualization
{
	internal struct Spiral
	{
		internal const double DensityCoefficient = 1 / 6.28226;

		private double _currentAngle;
		private readonly Point _center;
		private double _currentDensity;
		private readonly double _angleStep;
		private readonly int _firstLayerRadius;

		public Spiral(double angleStep, int firstLayerRadius, double initialDensity, Point center)
		{
			_angleStep = angleStep;
			_firstLayerRadius = firstLayerRadius;
			_currentDensity = initialDensity;
			_center = center;
			_currentAngle = 0;
		}

		public void UpdateDensity(Size rectangleSize) => _currentDensity = CalculateDensity(rectangleSize);

		public static double CalculateDensity(Size rectangleSize)
		{
			var diagonal = Math.Sqrt(Math.Pow(rectangleSize.Width, 2) + Math.Pow(rectangleSize.Height, 2));
			return Math.Round(diagonal / 2) * DensityCoefficient;
		}
		
		public Point GetNextLocation()
		{
			var nextRadius = _firstLayerRadius + _currentAngle * _currentDensity;
			_currentAngle += _angleStep;
			return ToCartesianCoordinateSystem(nextRadius, _currentAngle, _center);
		}

		private static Point ToCartesianCoordinateSystem(double radius, double angle, Point center)
		{
			var x = radius * Math.Cos(angle) + center.X;
			var y = radius * Math.Sin(angle) + center.Y;
			return new Point(RoundCoordinate(x, center.X), RoundCoordinate(y, center.Y));
		}

		private static int RoundCoordinate(double value, int centerCoordinate)
		{
			if (value > 0)
				return value > centerCoordinate ? (int) Math.Ceiling(value) : (int) value;
			return value > centerCoordinate ? (int) value: (int) Math.Ceiling(value);
		}
	}
}