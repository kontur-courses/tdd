using System;

namespace TagsCloudVisualization
{
    class Direction
    {
        private double _currentAlpha;
        private const double AngleShift = 1;

        public double GetNextDirection()
        {
            var oldAlpha = _currentAlpha;
            _currentAlpha = (_currentAlpha + AngleShift) % (Math.PI * 2);

            return Math.Tan(oldAlpha);
        }
    }
}
