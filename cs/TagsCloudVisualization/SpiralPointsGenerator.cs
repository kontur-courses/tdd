using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class SpiralPointsGenerator : IPointsGenerator
    {
        public IEnumerable<Point> GetPoints(int distanceBetweenPoints, double angleIncrement = 0.01)
        {
            if (distanceBetweenPoints <= 0)
                throw new ArgumentException("Distance between points should be positive number");
            double currentSpiralAngle = 0;
            while (true)
            {
                var radius = distanceBetweenPoints * currentSpiralAngle;
                var x = radius * Math.Cos(currentSpiralAngle);
                var y = radius * Math.Sin(currentSpiralAngle);
                currentSpiralAngle += angleIncrement;
                var point = new Point((int) Math.Round(x), (int) Math.Round(y));
                yield return point;
            }
        }
    }
}