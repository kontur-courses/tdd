using System;
using System.Drawing;

namespace TagsCloudVisualization.Distributions
{
    public class Spiral : IDistribution
    {
        private const float ShiftX = 5.0f;
        private const float ShiftY = 2.5f;
        
        private float angle = 0;
        private readonly Point center;

        public Spiral(Point center)
        {
            this.center = center;
        }
        
        public Point GetNextPoint()
        {
            angle += 0.1f;
            var x = center.X + ShiftX * angle * Math.Cos(angle);
            var y = center.Y + ShiftY * angle * Math.Sin(angle);

            return new Point((int)x, (int)y);
        }
    }
}