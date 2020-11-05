using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CircularCloudLayouterTests
{
    public class CircularCloudLayouter
    {
        private Point center;
        private double coefOfSpiralEquation = 0.5;
        private int spiralTurnoverNumber;
        private double anglePhi;
        private double deltaOfAnglePhi = Math.PI / 90;
        private List<Rectangle> rectangles;

        public CircularCloudLayouter(Point point)
        {
            center = point;
            rectangles = new List<Rectangle>();
            anglePhi = 2 * Math.PI;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException();
            if (spiralTurnoverNumber == 0)
            {
                spiralTurnoverNumber++;
                return PutFirstRectangle(rectangleSize);
            }

            var rectangle = rectangles.Last();
            while (RectangleIntersectAnyRectangles(rectangle))
            {
                var coordinates = GetCoordinatesOfRectangle(rectangleSize);
                rectangle = new Rectangle(new Point(coordinates.X, coordinates.Y), rectangleSize);
                anglePhi += deltaOfAnglePhi;
            }

            rectangles.Add(rectangle);
            return rectangle;
        }

        private Point GetCoordinatesOfRectangle(Size rectangleSize)
        {
            var pointOnSpiral = GetNextPointOnSpiral();
            var x = pointOnSpiral.X + center.X;
            var y = pointOnSpiral.Y + center.Y;
            if (x >= center.X && y <= center.Y)
            {
                y -= rectangleSize.Height;
            }
            else if (x <= center.X && y <= center.Y)
            {
                y -= rectangleSize.Height;
                x -= rectangleSize.Width;
            }
            else if (x <= center.X && y >= center.Y)
            {
                x -= rectangleSize.Width;
            }

            return new Point(x, y);
        }

        private bool RectangleIntersectAnyRectangles(Rectangle rectangle)
        {
            foreach (var rectangle1 in rectangles)
            {
                if (rectangle.IntersectsWith(rectangle1))
                    return true;
            }

            return false;
        }

        private Point GetNextPointOnSpiral()
        {
            var x = (int) Math.Round(coefOfSpiralEquation * anglePhi * Math.Cos(anglePhi));
            var y = (int) Math.Round(coefOfSpiralEquation * anglePhi * Math.Sin(anglePhi));
            return new Point(x, y);
        }

        private Rectangle PutFirstRectangle(Size rectangleSize)
        {
            var x = center.X;
            var y = center.Y;
            var rectangle = new Rectangle(new Point(x, y), rectangleSize);
            rectangles.Add(rectangle);
            return rectangle;
        }
    }
}