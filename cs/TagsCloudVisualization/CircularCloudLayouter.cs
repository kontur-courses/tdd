using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly List<Rectangle> Rectangles;
        public readonly Point Center;
        private double Angle;
        private double SpiralStep = 0.1;
        private double RadiusStep = 0.5;

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            Rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            Rectangle newRect;
            do
            {
                var location = GetNextLocation(rectangleSize);
                newRect = new Rectangle(location, rectangleSize);
            } 
            while (IsIntersecting(newRect));
            Rectangles.Add(newRect);
            return newRect;
        }

        private Point GetNextLocation(Size rectangleSize)
        {
            var radius = RadiusStep * Angle;
            var centerX = Center.X + (int)(radius * Math.Cos(Angle));
            var centerY = Center.Y + (int)(radius * Math.Sin(Angle));
            Angle += SpiralStep;
            return GetCornerPoint(new Point(centerX, centerY), rectangleSize);
        }

        private bool IsIntersecting(Rectangle rectangle)
        {
            foreach (var existingRectangle in Rectangles)
            {
                if (existingRectangle.IntersectsWith(rectangle))
                    return true;
            }
            return false;
        }

        public static Point GetCornerPoint(Point center, Size size)
        {
            return new Point(center.X - size.Width / 2, center.Y - size.Height / 2);
        }

        public static Point GetCenterPoint(Rectangle rect)
        {
            return new Point(rect.Location.X + rect.Width / 2, rect.Location.Y + rect.Height / 2);
        }
    }
}
