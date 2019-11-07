using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class ArchimedesSpiral : IEnumerable<Point>
    {
        private readonly Point center;
        private readonly float radius;
        private readonly float increment;
        private readonly float angle;

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

        public IEnumerator<Point> GetEnumerator() => new ArchimedesSpiralEnumerator(center, radius, increment, angle);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class ArchimedesSpiralEnumerator : IEnumerator<Point>
    {
        private readonly Point center;
        private readonly float radius;
        private float angle;
        private readonly float increment;
        private readonly float startAngle;

        public Point Current { get; private set; }

        object IEnumerator.Current => Current;

        public ArchimedesSpiralEnumerator(Point center, float radius, float increment, float angle = 0)
        {
            if (Math.Abs(radius) < float.Epsilon)
                throw new ArgumentException("Archimedes spiral radius can't be zero");
            if (Math.Abs(increment) < float.Epsilon)
                throw new ArgumentException("Archimedes spiral increment can't be zero");
            this.center = center;
            this.radius = radius;
            this.increment = increment;
            this.angle = angle;
            startAngle = angle;
            Current = center;
        }

        public bool MoveNext()
        {
            var isOverFlown = false;
            try
            {
                checked
                {
                    angle += increment;
                    var x = center.X + (int) Math.Round(Math.Cos(angle) * (angle * radius));
                    var y = center.Y + (int) Math.Round(Math.Sin(angle) * (angle * radius));
                    Current = new Point(x, y);
                }
            }
            catch (OverflowException e)
            {
                Console.WriteLine(e);
                isOverFlown = true;
            }

            return !isOverFlown;
        }

        public void Reset()
        {
            Current = center;
            angle = startAngle;
        }

        public void Dispose()
        {
        }
    }
}