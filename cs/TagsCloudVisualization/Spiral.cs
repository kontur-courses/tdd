using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    class Spiral
    {
        private double _radius;
        private double _angle;
        private readonly double _radiusStep;
        private const double AngleStep = 0.1;
        private Point _center;
        private const int StartRadiusDelay = 10;
        private const double RadiusStepCoefficient = 0.06;
        private const int ScaleDivider = 50;


        public Spiral(double cloudRadius, Point center)
        {
            _center = center;
            _radius = Math.Max(cloudRadius - StartRadiusDelay, 0);
            _radiusStep = RadiusStepCoefficient * center.X / ScaleDivider;
        }

        public Point GetCurrentPosition(Size rectangleSize)
        {
            return new Point(
                (int)(_center.X + _radius * Math.Cos(_angle) - rectangleSize.Width / 2),
                (int)(_center.Y - _radius * Math.Sin(_angle)) - rectangleSize.Height / 2);
        }

        public void TakeStep()
        {
            _radius += _radiusStep;
            _angle += AngleStep;
        }
    }
}
