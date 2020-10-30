using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        //private List<Point> points;
        private Point center;
        private List<Rectangle> rectangels= new List<Rectangle>();

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var halfRectangleSize = new Size(rectangleSize.Width / 2, rectangleSize.Height / 2);
            foreach (var point in GetSpiralPoints())
            {
                var rectangle = new Rectangle(point-halfRectangleSize,rectangleSize);
                if (rectangels.Any(x => x.IntersectsWith(rectangle)))
                    continue;

                rectangels.Add(rectangle);
                return rectangle;
            }
            return new Rectangle();
        }

        private IEnumerable<Point> GetSpiralPoints()
        {
            var radius = 0;
            while (true)
            {
                for (var i = 0; i < 360; i++)
                {
                    yield return new Point((int) (Math.Cos(2 * Math.PI * i / 360) * radius + 0.5) + center.X,
                                           (int) (Math.Sin(2 * Math.PI * i / 360) * radius + 0.5) + center.Y);
                }
                radius++;
            }
            
        }
    }
}