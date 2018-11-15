using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization.Classes
{
    public class SpiralGenerator
    {
        private readonly Point center;
        private double Angle { get; set; }
        private double Radius { get; set; }
        private double shiftAngle = 0.3;
        private double shiftRadius = 0.003;

        public SpiralGenerator(Point center)
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException("Coordinates of the center must be positive numbers");
            this.center = center;
        }

        public Rectangle GetNextRectangleOnSpiral(Size size)
        {
            if (size.IsEmpty)
                throw new ArgumentException("Size of the rectangle must be not empty");
            
            var rectanglCenter = new Point((int) (center.X + Radius * Math.Cos(Angle)),
                (int) (center.Y + Radius * Math.Sin(Angle)))  - new Size(size.Width / 2, size.Height / 2);

            Radius += shiftRadius;
            Angle += shiftAngle;

            return new Rectangle(rectanglCenter, size);
        }
    }
}
