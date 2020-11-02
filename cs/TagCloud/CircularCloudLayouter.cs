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
        private Point center;
        private Spiral spiral;
        private List<Rectangle> rectangles = new List<Rectangle>();
        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            spiral = new Spiral(center, 1);
            var isSuitiablePoint = true;
            foreach (var point in spiral.GetNextPoint)
            {
                isSuitiablePoint = true;
                var newRectangle = new Rectangle(point, rectangleSize);
                foreach (var rectangle in rectangles)
                {
                    if (rectangle.IntersectsWith(newRectangle))
                    {
                        isSuitiablePoint = false;
                        break;
                    }
                }

                if (isSuitiablePoint)
                {
                    rectangles.Add(newRectangle);
                    return newRectangle;
                }
            }
            return new Rectangle(center, rectangleSize);
        }
    }
}