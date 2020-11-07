using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class ArchimedeanSpiral
    {
        private Point Center { get; }
        private Point CurrentPoint { get; set; }
        private double DistanceBetweenLoops { get; }
        private double Angle { get; set; }
        private double AngleDelta { get; }

        public ArchimedeanSpiral(Point center, double distanceBetweenLoops, double angleDelta)
        {
            if (center.X < 0 || center.Y < 0)
            {
                throw new ArgumentException("center coordinates should not be negative numbers");
            }
            Center = center;

            if (angleDelta <= 0)
            {
                throw new ArgumentException("angleDelta should not be negative or zero");
            }
            AngleDelta = angleDelta;

            if (distanceBetweenLoops <= 0)
            {
                throw new ArgumentException("distanceBetweenLoops should not be negative or zero");
            }
            DistanceBetweenLoops = distanceBetweenLoops;
            Angle = 0;
            CurrentPoint = Center;
        }

        public Point GetNextPoint()
        {
            if (Angle == 0)
            {
                Angle += AngleDelta;
                return Center;
            }

            var nextPoint = CurrentPoint;
            while (CurrentPoint.Equals(nextPoint))
            {
                var x = Center.X + (int) (DistanceBetweenLoops * Angle * Math.Cos(Angle));
                var y = Center.Y + (int) (DistanceBetweenLoops * Angle * Math.Sin(Angle));
                Angle += AngleDelta;
                nextPoint = new Point(x, y);
            }

            CurrentPoint = nextPoint;
            return CurrentPoint;
        }
    }
}