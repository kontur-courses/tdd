using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.PointsGenerators
{
    public class ArchimedesSpiral : IPointGenerator
    {
        public Point Center { get; }
        public Size CanvasSize { get; }
        public int SpiralParameter { get; }
        public float AngleStep { get; }

        private IEnumerator<Point> spiralPoints;

        public ArchimedesSpiral(
            Point center,
            Size? canvasSize = null,
            int spiralParameter = 2,
            float angleStep = 0.2f)
        {
            if (Math.Abs(angleStep) < 1e-3)
                throw new ArgumentException("Angle step must be not equal zero");
                
            if (spiralParameter == 0)
                throw new ArgumentException("Spiral parameter must be not equal zero");
            
            Center = center;
            SpiralParameter = spiralParameter;
            AngleStep = angleStep;
            CanvasSize = canvasSize ?? new Size(500, 500);
            
            if (CanvasSize.Width <= 0 || CanvasSize.Height <= 0)
                throw new ArgumentException("Canvas size must be more than zero");
            
            spiralPoints = GetSpiralPoints().GetEnumerator();
        }

        private IEnumerable<Point> GetSpiralPoints()
        {
            yield return Center;
            
            var angle = 0.0f;
            var currentPoint = Center;

            while (currentPoint.X < CanvasSize.Width && currentPoint.Y < CanvasSize.Height)
            {
                var x = SpiralParameter * (int) Math.Round(angle * Math.Cos(angle)) + Center.X;
                var y = SpiralParameter * (int) Math.Round(angle * Math.Sin(angle)) + Center.Y;
                var nextPoint = new Point(x, y);

                if (!nextPoint.Equals(currentPoint))
                    yield return nextPoint;

                currentPoint = nextPoint;
                angle += AngleStep;
            }
        }

        public Point GetNextPoint()
        {
            spiralPoints.MoveNext();
            return spiralPoints.Current;
        }

        public void StartOver()
        {
            spiralPoints = GetSpiralPoints().GetEnumerator();
        }
    }
}