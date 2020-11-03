using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace TagCloud
{
    internal class Spiral
    {
        private Point Center;
        private int Step;
        public Spiral(Point center, int step)
        {
            this.Center = center;
            this.Step = step;
        }

        public IEnumerable<Point> GetNextPoint
        {
            get
            {
                var angle = 0.0;
                while (true)
                {
                    var x = (int) Math.Round(Step * angle * Math.Cos(angle)) + Center.X;
                    var y = (int) Math.Round(Step * angle * Math.Sin(angle)) + Center.Y;
                    yield return new Point(x, y);
                    angle += 0.05;
                }
            }
        }
    }
    class CircularCloudLayouter
    {
        private readonly Point center;
        private Spiral spiral;
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }

        private Rectangle GetRectangleByCenter(Size rectangleSize, Point rectangleCenter)
        {
            var leftTopAngle = new Point(rectangleCenter.X - rectangleSize.Width / 2,
                rectangleCenter.Y - rectangleSize.Height / 2);
            return new Rectangle(leftTopAngle, rectangleSize);
        }
        
        private bool RectangleIntersectWithOthers(Rectangle checkedRectangle)
        {
            foreach (var rectangle in rectangles)
            {
                if (rectangle.IntersectsWith(checkedRectangle))
                {
                    return true;
                }
            }

            return false;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            spiral = new Spiral(center, 1);
            foreach (var point in spiral.GetNextPoint)
            {
                var newRectangle = GetRectangleByCenter(rectangleSize, point);
                
                if (!RectangleIntersectWithOthers(newRectangle))
                {
                    rectangles.Add(newRectangle);
                    return newRectangle;
                }
            }
            return new Rectangle(center, rectangleSize);
        }
    }
}