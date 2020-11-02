using System;
using System.Drawing;

namespace TagsCloudVisualization.Core
{
    public class ArchimedeanSpiral
    {
        private Point Center { get; }
        private const double Theta = 0.5;

        private double polarArgument;

        public ArchimedeanSpiral(Point center)
        {
            Center = center;
            polarArgument = 0;
        }

        public Point GetNextPoint()
        {
            var radius = polarArgument * Theta;
            var x = radius * Math.Cos(polarArgument);
            var y = radius * Math.Sin(polarArgument);
            polarArgument += 0.01;
            return new Point(Center.X + (int) Math.Round(x), Center.Y - (int) Math.Round(y));
        }
    }
}