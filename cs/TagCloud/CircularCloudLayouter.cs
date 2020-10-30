using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace TagCloud
{
    class Spiral
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
                var i = 1;
                var newPoint = Center;
                yield return Center;
                while (true)
                {
                    var sign = 1;
                    if (i % 2 == 0)
                    {
                        sign = -1;
                    }
                    for (var j = 0; j < i; j++)
                    {
                        newPoint = new Point(newPoint.X, newPoint.Y+ sign * Step);

                        yield return newPoint;
                    }
                    for (var j = 0; j < i; j++)
                    {
                        newPoint = new Point(newPoint.X - sign * Step, newPoint.Y);

                        yield return newPoint;
                    }
                    i++;
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