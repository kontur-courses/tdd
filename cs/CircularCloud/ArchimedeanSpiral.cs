using System;
using System.Drawing;

namespace CircularCloud
{
    public class ArchimedeanSpiral
    {
        public Point Center { get; }
        protected const double Distance = 0.5;
        protected double Angel;
        protected Point? Previous;

        public ArchimedeanSpiral(Point center) => Center = center;

        public Point GetNextPoint()
        {
            Point result;
            do
            {
                var radius = Distance * Angel;
                var x = radius * Math.Cos(Angel);
                var y = radius * Math.Sin(Angel);
                Angel += 0.0005;
                result = new Point((int) Math.Round(x) + Center.X, (int) Math.Round(y) + Center.Y);
            } while (result == Previous);

            Previous = result;
            return result;
        }
    }
}