using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public Point Center { get; }
        public readonly List<Rectangle> Rectangles = new List<Rectangle>();

        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }
            
        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = new Rectangle(Center, rectangleSize);
            rectangle = MoveToValidPosition(rectangle);
            Rectangles.Add(rectangle);
            return rectangle;
        }

        private bool RectangleHasCollisions(Rectangle rectangle)
        {
            foreach (var otherRectangle in Rectangles)
            {
                if (rectangle.IntersectsWith(otherRectangle))
                    return true;
            }

            return false;
        }

        private Rectangle MoveToValidPosition(Rectangle rectangle)
        {
            var step = 1;
            while (RectangleHasCollisions(rectangle))
            {
                var x = step * Math.Cos(step);
                var y = step * Math.Sin(step);
                rectangle.Location = new Point((int)x + Center.X, (int)y + Center.Y);
                step++;
            }

            return rectangle;
        }
    }
}
