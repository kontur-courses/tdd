using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICircularCloudLayouter
    {
        public Point Center { get;}

        private Spiral LayouterSpiral { get;}

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
            var nextRectangle = CreateNewRectangle(rectangleSize);
            while (RectangleList.Any(rectangle => rectangle.IntersectsWith(nextRectangle)))
                nextRectangle = CreateNewRectangle(rectangleSize);
            RectangleList.Add(nextRectangle);
            return nextRectangle;
        }

        private Rectangle CreateNewRectangle(Size rectangleSize)
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
                
                var rightUpNode = new Point(rectangle.X + rectangle.Width, rectangle.Y);
                var rightDownNode = new Point(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height);
                var leftDowNode = new Point(rectangle.X, rectangle.Y + rectangle.Height);
                var leftUpToCenter = Math.Sqrt(Math.Pow(rectangle.X - Center.X, 2) +
                                               Math.Pow(rectangle.Y - Center.Y, 2));
                var leftDownToCenter =
                    Math.Sqrt(Math.Pow(leftDowNode.X - Center.X, 2) + Math.Pow(leftDowNode.Y - Center.Y, 2));
                var rightUpToCenter = Math.Sqrt(Math.Pow(rightUpNode.X - Center.X, 2) + Math.Pow(rightUpNode.Y - Center.Y, 2));
                var rightDownToCenter =
                    Math.Sqrt(Math.Pow(rightDownNode.X - Center.X, 2) + Math.Pow(rightDownNode.Y - Center.Y, 2));
                possibleRadii.Add(new List<double>{rightDownToCenter, rightUpToCenter, leftDownToCenter, leftUpToCenter}.Max());
            }
            return possibleRadii.Max();
        }

        public double GetSumAreaOfRectangles()
        {
            double result = 0;
            foreach (var rectangle in RectangleList)
            {
                var rectangleArea= rectangle.Height * rectangle.Width;
                result += rectangleArea;
            }
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
            if (circleRadius <= 0)
                throw new ArgumentException();
            const double pi = Math.PI;
            var area = pi * Math.Pow(circleRadius, 2);
            return area;
        }
    }

    public class Program
    {
        public static void Main()
        {
            var rectangleSize = new Size(300, 100);
            var layouterCenter = new Point(2500, 2500);
            var layouter = new CircularCloudLayouter(layouterCenter);
            var visualization = new Visualization(layouter.RectangleList, new Pen(Color.White, 3));
            var cnt = 0;
            for (int i = 0; i < 30; i++)
            {
                layouter.PutNextRectangle(new Size(rectangleSize.Width + cnt, rectangleSize.Height + cnt));
                cnt += 30;
            }
            visualization.DrawAndSaveImage(new Size(5000,5000), "C:/alpha/img_different_size", ImageFormat.Jpeg);
        }
    }
}
