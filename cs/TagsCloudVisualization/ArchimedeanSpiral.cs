using System.Drawing;

namespace TagsCloudVisualization
{
    public class ArchimedeanSpiral
    {
        private readonly double _step;
        private readonly double _density;
        private readonly double _start;

        public ArchimedeanSpiral(double step, double density, double start)
        {
            _step = step;
            _density = density;
            _start = start;
        }

        public IEnumerable<Point> GetNextSpiralPoint()
        {
            var curRadius = 0.0;
            var curAngle = 0.0;
            var maxSize = 1000000;
            while (curRadius < maxSize)
            {
                var polarPoint = new PolarPoint(curRadius, curAngle);
                yield return (Point)polarPoint;
                curAngle += _step;
                curRadius = _start + _density * curAngle;
                
            }
        }
    }
}

