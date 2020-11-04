using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    class ArchimedeanSpiral
    {
        private Point _center;
        private double _deltaAngle;
        private double _angle;
        private double _step;

        public ArchimedeanSpiral(Point center, double deltaAngle = Math.PI / 360, double step = 5)
        {
            _center = center;
            _deltaAngle = deltaAngle;
            _angle = 0;
            _step = step;
        }

        public Point GetNextPoint()
        {
            var x = (int) Math.Round(_center.X + (_step * _angle * Math.Cos(_angle)));
            var y = (int) Math.Round(_center.X + (_step * _angle * Math.Sin(_angle)));
            var point = new Point(x, y);
            _angle += _deltaAngle;
            return point;
        }
    }
}
