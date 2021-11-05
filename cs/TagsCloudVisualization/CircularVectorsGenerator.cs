using System;
using System.Collections.Generic;
using System.Drawing;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
    public class CircularVectorsGenerator : IVectorsGenerator
    {
        private readonly int _angles;
        private readonly double _multiplier;
        private double _lastAngle;

        public CircularVectorsGenerator(double multiplier, int angles)
        {
            _multiplier = multiplier;
            _angles = angles;
            _lastAngle = 0;
        }

        public IEnumerable<Point> Generate()
        {
            var step = Math.PI * 2 / _angles;
            while (true)
            {
                var x = Convert.ToInt32(_multiplier * _lastAngle * Math.Cos(_lastAngle));
                var y = Convert.ToInt32(_multiplier * _lastAngle * Math.Sin(_lastAngle));
                _lastAngle += step;
                yield return new Point(x, y);
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}