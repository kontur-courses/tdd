using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICircularCloudLayouter
    {
        public Point Center { get; set; }

        public Spiral LayouterSpiral { get; set; }

        public List<Rectangle> RectangleList { get; set; }


        public CircularCloudLayouter(Point center)
        {
            Center = center;
            LayouterSpiral = new Spiral(Center);
            RectangleList = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException();
            Rectangle nextRectangle;
            if (RectangleList.Count == 0)
            {
                nextRectangle = new Rectangle(Center, rectangleSize);
                RectangleList.Add(nextRectangle);
                return nextRectangle;
            }
            nextRectangle = CreteNewRectangle(rectangleSize);
            while (RectangleList.Any(rectangle => rectangle.IntersectsWith(nextRectangle)))
                nextRectangle = CreteNewRectangle(rectangleSize);
            RectangleList.Add(nextRectangle);
            return nextRectangle;
        }

        private Rectangle CreteNewRectangle(Size rectangleSize)
        {
            var rectangleCenterLocation = LayouterSpiral.GetNextPoint();
            var rectangleX = rectangleCenterLocation.X - rectangleSize.Width / 2;
            var rectangleY = rectangleCenterLocation.Y - rectangleSize.Height / 2;
            var rectangleLocation = new Point(rectangleX,rectangleY);
            var rectangle = new Rectangle(rectangleLocation, rectangleSize);
            return rectangle;
        }

        public double GetCircleRadius()
        {
            var possibleRadii = new List<double>();
            foreach (var rectangle in RectangleList)
            {
                
                var rightUp = new Point(rectangle.X + rectangle.Width, rectangle.Y);
                var rightDown = new Point(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height);
                var leftDown = new Point(rectangle.X, rectangle.Y + rectangle.Height);
                var leftUpToCenter = Math.Sqrt(Math.Pow(rectangle.X - Center.X, 2) +
                                               Math.Pow(rectangle.Y - Center.Y, 2));
                var leftDownToCenter =
                    Math.Sqrt(Math.Pow(leftDown.X - Center.X, 2) + Math.Pow(leftDown.Y - Center.Y, 2));
                var rightUpToCenter = Math.Sqrt(Math.Pow(rightUp.X - Center.X, 2) + Math.Pow(rightUp.Y - Center.Y, 2));
                var rightDownToCenter =
                    Math.Sqrt(Math.Pow(rightDown.X - Center.X, 2) + Math.Pow(rightDown.Y - Center.Y, 2));
                possibleRadii.Add(new List<double>(){rightDownToCenter, rightUpToCenter, leftDownToCenter, leftUpToCenter}.Max());
            }

            return possibleRadii.Max();
        }

        public double GetSumAreaOfRectangles()
        {
            double result = 0;
            foreach (var rectangle in RectangleList)
                result += rectangle.Height * rectangle.Width;

            return result;
        }

        public double GetEnclosingRectangleArea()
        {
            var vertexes = new List<Point>();
            foreach (var rectangle in RectangleList)
            {
                vertexes.Add(new Point(rectangle.X, rectangle.Y + rectangle.Height));
                vertexes.Add(new Point(rectangle.X, rectangle.Y));
                vertexes.Add(new Point(rectangle.X + rectangle.Width, rectangle.Y));
                vertexes.Add(new Point(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height));
            }

            var xCords = from vertex in vertexes select vertex.X;
            var sortListXCords = xCords.ToList().OrderBy(x=>x).ToList();
            var xMin = sortListXCords.First();
            var xMax = sortListXCords.Last();
            var yCords = from vertex in vertexes select vertex.Y;
            var sortListYCords = yCords.ToList().OrderBy(y => y).ToList();
            var yMin = sortListYCords.First();
            var yMax = sortListYCords.Last();

            var enclosingRectangle = new Rectangle(xMin, yMin, xMax - xMin, yMax - yMin);
            return enclosingRectangle.Height * enclosingRectangle.Width;

        }


        public double GetCircleArea(double circleRadius)
        {
            const double pi = Math.PI;
            var area = pi * Math.Pow(circleRadius, 2);
            return area;
        }
    }

    public class Program
    {
        public static void Main()
        {
        }
    }
}
