using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class ArchimedesSpiral
    {
        private readonly Point center;
        private readonly float radius;
        private readonly float increment;
        private float angle = 0;
        public ArchimedesSpiral(Point center, float radius, float increment)
        {
            this.center = center;
            this.radius = radius;
            this.increment = increment;
        }

        private void Increment() => angle += increment;

        public Point GetNextCoordinates()
        {
            var x = center.X + (int)Math.Round(Math.Cos(angle) * (angle * radius));
            var y = center.Y + (int)Math.Round(Math.Sin(angle) * (angle * radius));
            Increment();
            return new Point(x, y);
        }
    }
}