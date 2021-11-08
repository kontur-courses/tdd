using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    internal class Spiral
    {
        private double radius = 0;
        private double angle = 0;

        private Point center;

        private PointD currentPoint =>
            new PointD(
                Math.Cos(angle) * radius + center.X,
                Math.Sin(angle) * radius + center.Y);
        
        
    }
}