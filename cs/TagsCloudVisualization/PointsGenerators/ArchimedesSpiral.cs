using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.PointsGenerators
{
    public class ArchimedesSpiral : IPointGenerator
    {
        public Point Center { get; }
        
        private readonly int spiralParameter;
        private readonly float angleStep;
        private IEnumerator<Point> spiralPoints;

        public ArchimedesSpiral(
            Point center,
            int spiralParameter = 2,
            float angleStep = 0.2f)
        {
            if (Math.Abs(angleStep) < 1e-3)
                throw new ArgumentException("Angle step must be not equal zero");
                
            if (spiralParameter == 0)
                throw new ArgumentException("Spiral parameter must be not equal zero");
            
            Center = center;
            this.spiralParameter = spiralParameter;
            this.angleStep = angleStep;
            spiralPoints = GetSpiralPoints().GetEnumerator();
        }

        private IEnumerable<Point> GetSpiralPoints()
        {
            yield return Center;
            
            var angle = 0.0f;
            var currentPoint = Center;

            while (currentPoint.X < int.MaxValue && currentPoint.Y < int.MaxValue)
            {
                var x = spiralParameter * (int) Math.Round(angle * Math.Cos(angle)) + Center.X;
                var y = spiralParameter * (int) Math.Round(angle * Math.Sin(angle)) + Center.Y;
                var nextPoint = new Point(x, y);

                if (!nextPoint.Equals(currentPoint))
                    yield return nextPoint;

                currentPoint = nextPoint;
                angle += angleStep;
            }
        }
        
        public Point? GetNextPoint()
        {
            if (spiralPoints.MoveNext())
                return spiralPoints.Current;
            return null;
        }

        public void StartOver()
        {
            spiralPoints.Dispose();
            spiralPoints = GetSpiralPoints().GetEnumerator();
        }
    }
}