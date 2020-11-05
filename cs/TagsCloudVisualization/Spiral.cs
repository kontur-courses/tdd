using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class Spiral
    {
        private double _radius;
        private double _angle;
        private readonly double _radiusStep;
        private readonly double _angleStep;
        private Point _center;

        public Spiral(double cloudRadius, Point center)
        {
            _center = center;
            _radius = Math.Max(cloudRadius - 10, 0);
            _angle = 0d;
            _radiusStep = 0.06 * center.X / 50;
            _angleStep = 0.1;
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
            _angle += _angleStep;
        }
    }
}
