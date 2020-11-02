using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;

namespace TagsCloudVisualization
{
    public class SpiralProvider
    {
        private double angle, radius;
        private double spiralParameter = 0.01;
        private Point center;

        public SpiralProvider(Point center)
        {
            this.center = center;
        }

        private Point GetPossiblePoint(Point center)
        {
            var x = (int)Math.Round(radius * Math.Cos(angle));
            var y = (int)Math.Round(radius * Math.Sin(angle));

            radius += spiralParameter;
            angle += Math.PI / 180;

            return new Point(center.X - x, center.Y - y);
        }

        public Rectangle GetRectangle(Size rectangleSize, List<Rectangle> rectangles)
        {
            Rectangle rectangle;
            do
            {
                rectangle = new Rectangle(GetPossiblePoint(center) - new Size(rectangleSize.Width / 2, rectangleSize.Height / 2),
                    rectangleSize);

            } while (rectangles.Where(rectangle.IntersectsWith).Any());

            return rectangle;
        }
    }
}
