using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class ArchimedeanSpiral : IDistribution
    {
        const double DegreeInRadians = Math.PI / 180;
        const double FullRotation = 2 * Math.PI;
        
        public Point Center { get; }
        public double Step { get; }
        public double AngleStep { get; }
        public double AngleInRadians { get; private set; }

        public ArchimedeanSpiral(Point center, double step = 1, double angleStep = DegreeInRadians)
        {
            Center = center;
            Step = step;
            AngleStep = angleStep;
            AngleInRadians = 0;
        }

        public IEnumerable<Point> GetPoints()
        {
            while (true)
            {
                var x = Center.X + GetOffsetFromCenterOnX();
                var y = Center.Y + GetOffsetFromCenterOnY();
                yield return new Point((int)x, (int)y);

                AngleInRadians += AngleStep;
            }
        }

        private double GetOffsetFromCenterOnX()
        {
            return (Step / FullRotation) * AngleInRadians * Math.Sin(AngleInRadians);
        }

        private double GetOffsetFromCenterOnY()
        {
            return (Step / FullRotation) * AngleInRadians * Math.Cos(AngleInRadians);
        }
    }
}