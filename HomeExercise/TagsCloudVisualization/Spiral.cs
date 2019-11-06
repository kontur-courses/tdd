using System;
using System.Drawing;

namespace TagsCloudVisualization
{
	internal class Spiral
	{
		private const double DensityCoefficient = 1 / (2 * Math.PI);

		private double _currentAngle;
		private readonly Point _center;
		private readonly double _angleStep;

		private double _currentDensity;
		public double CurrentDensity
		{
			set => _currentDensity = value * DensityCoefficient;
		}
		public int? FirstLayerRadius { get; set; }

		public Spiral(double angleStep, Point center)
		{
			_angleStep = angleStep;
			_center = center;
			_currentAngle = 0;
		}

		public Point GetNextLocation()
		{
			if (FirstLayerRadius == null)
				throw new ArgumentException("FirstLayerRadius is null");
			
			var nextRadius = FirstLayerRadius.Value + _currentAngle * _currentDensity;
			_currentAngle += _angleStep;
			return ExMath.ToCartesianCoordinateSystem(nextRadius, _currentAngle, _center);
		}
	}
}