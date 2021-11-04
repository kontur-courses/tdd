using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        public double CurrentAngle { get; set; }
        public double Step { get; set; }
        public double AngleStep { get; set; }

        public Point Central { get; set; }

        public Spiral(Point centralPoint)
        {
            Central = centralPoint;
            CurrentAngle = 0;
        }

        public Point GetNextPoint()
        {
            var nextX = Central.X + CurrentAngle * Step * Math.Cos(CurrentAngle);
            var nextY = Central.Y + CurrentAngle * Step * Math.Sin(CurrentAngle);
            var roundNextX = Convert.ToInt32(Math.Round(nextX));
            var roundNextY = Convert.ToInt32(Math.Round(nextY));
            var nextPoint = new Point(roundNextX, roundNextY);
            CurrentAngle += AngleStep;
            return nextPoint;

        }
    }
}
