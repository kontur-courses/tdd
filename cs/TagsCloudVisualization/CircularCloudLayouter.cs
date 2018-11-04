using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public Point Center { get; }
        private List<Rectangle> rectangles = new List<Rectangle>();
        private const int AngleDelta = 1;
        private const int SpiralWidth = 1;
        private int currentAngle = 0;


        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rect = new Rectangle();
            if (rectangles.Count == 0)
            {
                rect = new Rectangle(Center, rectangleSize);
            }
            else
            {
                rect = new Rectangle(GetCurrentSpiralPosition(), rectangleSize);
                while (rect.IntersectsWithAnyFrom(rectangles))
                {
                    currentAngle += AngleDelta;
                    rect = new Rectangle(GetCurrentSpiralPosition(), rectangleSize);
                }
            }

            rectangles.Add(rect);
            return rect;
        }

        private Point GetCurrentSpiralPosition()
        {
            var x = Center.X + (int)(SpiralWidth * currentAngle * Math.Cos(currentAngle));
            var y = Center.Y + (int)(SpiralWidth * currentAngle * Math.Sin(currentAngle));
            return new Point(x, y);
        }
    }
}
