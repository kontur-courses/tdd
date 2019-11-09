using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class ArchimedeanSpiral : ISpiralGenerator
    {
        public Point Center => center;
        private IEnumerator<Point> spiralPoints;

        private readonly Point center;
        private readonly double spiralStep;

        public ArchimedeanSpiral(Point center, double spiralStep)
        {
            this.center = center;
            this.spiralStep = spiralStep;
            spiralPoints = GetAllSpiralPoints().GetEnumerator();
        }

        private IEnumerable<Point> GetAllSpiralPoints()
        {
            var currentPos = center;
            var angle = 0.0;

            while (currentPos.X < int.MaxValue || 
                   currentPos.Y < int.MaxValue)
            {
                var nextPos = new Point(
                    center.X + (int) Math.Round(angle * Math.Sin(angle)),
                    center.Y + (int) Math.Round(angle * Math.Cos(angle)));
                angle += spiralStep;

                if (currentPos == nextPos && currentPos != center) continue;

                currentPos = nextPos;
                yield return nextPos;
            }
        }

        public Point GetNextSpiralPoint()
        {
            spiralPoints.MoveNext();
            return spiralPoints.Current;
        }

        public List<Point> GetNextSpiralPoints(int count)
        {
            if(count <= 0)
                throw new ArgumentException("Count of elements need be more than zero");
            
            var result = new List<Point>();
            for (var i = 0; i < count; i++)
                result.Add(GetNextSpiralPoint());

            return result;
        }

        public void ResetSpiral() =>
            spiralPoints = GetAllSpiralPoints().GetEnumerator();

    }
}