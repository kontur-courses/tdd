using System;
using System.Drawing;

namespace TagCloudTask.Geometry
{
    public class DirectingArrow
    {
        private readonly Point _startPoint;
        private double _angleRadian;

        private int _radiusVectorLength;

        public DirectingArrow(Point startPoint)
        {
            _startPoint = startPoint;
            _angleRadian = 0;
            _radiusVectorLength = 0;
        }

        public void Rotate()
        {
            var rotationAngle = Math.PI / (6 + _radiusVectorLength);
            _angleRadian += rotationAngle;

            if (_angleRadian >= 2 * Math.PI)
            {
                _angleRadian -= 2 * Math.PI;
                _radiusVectorLength += 1;
            }
        }

        public Point GetEndPoint()
        {
            var x = (int)(_startPoint.X + _radiusVectorLength * Math.Cos(_angleRadian));
            var y = (int)(_startPoint.Y + _radiusVectorLength * Math.Sin(_angleRadian));

            return new Point(x, y);
        }
    }
}