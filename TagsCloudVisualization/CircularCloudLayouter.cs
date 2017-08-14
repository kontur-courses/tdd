using System;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private Dictionary<Point, Rectangle> Rectangles;
        private Point center;
        public Dictionary<Point, Rectangle> GetCloud()
        {
            var list = new List<int>();
            list.ToArray();
            return Rectangles;
        }

        public CircularCloudLayouter(Point center)
        {
            Rectangles = new Dictionary<Point, Rectangle>();
            this.center = center;
        }

        private Point GetClosestFreePoint(Size rectangleSize)
        {
            var lastRectangle = Rectangles.FirstOrDefault().Value;
            var x = lastRectangle.Point.X + lastRectangle.Size.Width / 2 + rectangleSize.Width / 2;
            var y = lastRectangle.Point.Y + lastRectangle.Size.Height / 2 + rectangleSize.Height / 2;
            var closestPoint = new Point(x, y);
            return closestPoint;

        }
        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            Rectangle rectangle;
            Point point;
            if (Rectangles.Count == 0)
            {
                point = center;
                rectangle = new Rectangle(rectangleSize, point);
            }
            else
            {
                point = GetClosestFreePoint(rectangleSize);
                rectangle = new Rectangle(rectangleSize,point);
            }
            Rectangles.Add(point,rectangle);
            return rectangle;
        }
    }

  }