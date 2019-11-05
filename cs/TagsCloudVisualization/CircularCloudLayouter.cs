using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        private Point center_cloud;
        private List<Rectangle> rectangles;
        private const double radiusStep = 1;
        private const double angleStep = 1;
        private double angle = 1;

        public CircularCloudLayouter(Point center)
        {
            center_cloud = center;
            rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = GetNextRectangle(rectangleSize);
            rectangles.Add(rectangle);
            return rectangle;
        }

        private Rectangle GetNextRectangle(Size rectangleSize)
        {
            var rectangle = GetRectangle(rectangleSize);
            while (CheckIntersect(rectangle))
            {
                rectangle = GetRectangle(rectangleSize);
            }
            return rectangle;
        }

        private bool CheckIntersect(Rectangle rectangle)
        {
            foreach (var r in rectangles)
                if (r.IntersectsWith(rectangle))
                    return true;
            return false;
        }

        private Rectangle GetRectangle(Size rectangleSize)
        {
            var x = (int)(Math.Cos(angle) * radiusStep * angle + center_cloud.X);
            var y = (int)(Math.Sin(angle) * radiusStep * angle + center_cloud.Y);
            var rectangle = new Rectangle(new Point(x, y), rectangleSize);
            angle += angleStep / angle;
            return rectangle;
        }

        public Rectangle[] GetRectangles()
        {
            return rectangles.ToArray();
        }
    }
}
