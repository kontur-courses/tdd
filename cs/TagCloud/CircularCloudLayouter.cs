using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagCloud
{
    internal class Spiral
    {
        private Point center;
        private readonly int step;
        private double currentAngle;
        public Spiral(Point center, int step)
        {
            this.center = center;
            this.step = step;
            this.currentAngle = 0;
        }

        public Point GetNextPoint()
        {
            var x = (int) Math.Round(step * currentAngle * Math.Cos(currentAngle)) + center.X;
            var y = (int) Math.Round(step * currentAngle * Math.Sin(currentAngle)) + center.Y;
            currentAngle += 0.05;
            return new Point(x, y);
        }
    }
    
    class CircularCloudLayouter
    {
        private readonly Spiral spiral;
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        public CircularCloudLayouter(Point center)
        {
            spiral = new Spiral(center, 1);
        }
        
        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            while (true)
            {
                var point = spiral.GetNextPoint();
                var newRectangle = GetRectangleByCenter(rectangleSize, point);
                
                if (!RectangleIntersectWithOthers(newRectangle))
                {
                    rectangles.Add(newRectangle);
                    return newRectangle;
                }
            }
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
    }
}