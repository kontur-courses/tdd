using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class SpiralPointsGenerator : IPointsGenerator
    {
        public List<Point> AllGeneratedPoints { get; } = new List<Point>();

        private readonly int distanceBetweenPoints;
        private int currentSpiralAngle;

        public SpiralPointsGenerator(int distanceBetweenPoints)
        {
            if (distanceBetweenPoints <= 0)
                throw new ArgumentException("Distance between points should be positive number");
            this.distanceBetweenPoints = distanceBetweenPoints;
        }

        public Point GetNextPoint()
        {
            var radius = distanceBetweenPoints * currentSpiralAngle;
            var x = radius * Math.Cos(currentSpiralAngle);
            var y = radius * Math.Sin(currentSpiralAngle);
            currentSpiralAngle++;
            var point = new Point((int)Math.Round(x), (int)Math.Round(y));
            AllGeneratedPoints.Add(point);
            return point;
        }
    }
}