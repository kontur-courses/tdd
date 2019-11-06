using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class ArchimedesSpiral
    {
        private readonly Point center;
        private readonly float radius;
        private readonly float increment;
        private float angle;

        public ArchimedesSpiral(Point center, float radius, float increment, float angle = 0)
        {
            if (Math.Abs(radius) < float.Epsilon)
                throw new ArgumentException("Archimedes spiral radius can't be zero");
            if (Math.Abs(increment) < float.Epsilon)
                throw new ArgumentException("Archimedes spiral increment can't be zero");
            this.center = center;
            this.radius = radius;
            this.increment = increment;
            this.angle = angle;
        }

        private void Increment() => angle += increment;

        public Point GetNextCoordinates()
        {
            var x = center.X + (int) Math.Round(Math.Cos(angle) * (angle * radius));
            var y = center.Y + (int) Math.Round(Math.Sin(angle) * (angle * radius));
            Increment();
            return new Point(x, y);
        }
    }
}