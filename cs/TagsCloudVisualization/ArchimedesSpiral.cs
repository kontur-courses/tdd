using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class ArchimedesSpiral : IEnumerator<Point>
    {
        private readonly Point center;
        private readonly float radius;
        private float angle;
        private float increment;
        private float startIncrement;
        private float startAngle;


        public Point Current { get; private set; }

        object IEnumerator.Current => Current;

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
            startIncrement = increment;
            startAngle = angle;
            Current = center;
        }

        private Point GetNextCoordinates()
        {
            angle += increment;
            var x = center.X + (int) Math.Round(Math.Cos(angle) * (angle * radius));
            var y = center.Y + (int) Math.Round(Math.Sin(angle) * (angle * radius));
            return new Point(x, y);
        }

        public bool MoveNext()
        {
            Current = GetNextCoordinates();
            return true;
        }

        public void Reset()
        {
            Current = center;
            angle = startAngle;
            increment = startIncrement;
        }
        
        public void Dispose()
        { }
    }
}