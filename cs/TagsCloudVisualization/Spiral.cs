using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        private double CurrentAngle { get; set; }

        private double AngleStep { get;}

        private Point Central { get;}


        public Spiral(Point centralPoint, double angleStep = Math.PI/360)
        {
            Central = centralPoint;
            CurrentAngle = 0;
            AngleStep = angleStep;
        }

        public Point GetNextPoint()
        {
            var nextX = Central.X + CurrentAngle * Math.Cos(CurrentAngle);
            var nextY = Central.Y + CurrentAngle * Math.Sin(CurrentAngle);
            var roundNextX = Convert.ToInt32(Math.Round(nextX));
            var roundNextY = Convert.ToInt32(Math.Round(nextY));
            var nextPoint = new Point(roundNextX, roundNextY);
            CurrentAngle += AngleStep;
            return nextPoint;
        }
    }
}
