using System;
using System.Drawing;

namespace CircularCloud
{
    public class ArchimedeanSpiral
    {
        public Point Center { get; private set; }
        protected const double Distance = 0.5;
        protected double Fi = 0;
        protected Point Previous = new Point(Int32.MaxValue, Int32.MinValue);

        public ArchimedeanSpiral(Point center)
        {
            Center = center;
        }

        public ArchimedeanSpiral(int x, int y)
        {
            Center = new Point(x, y);
        }

        public Point GetNextPoint()
        {
            Point result;
            do
            {
                var r = Distance * Fi;
                var x = r * Math.Cos(Fi);
                var y = r * Math.Sin(Fi);
                Fi += 0.0005;
                result = new Point((int) Math.Round(x) + Center.X, (int) Math.Round(y) + Center.Y);
            } while (result == Previous);

            Previous = result;
            return result;
        }
    }
}