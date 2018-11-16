using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class SpiralStrategy : IPlacementStrategy
    {
        public List<Rectangle> Rectangles { get; }
        public Point Center { get; }

        public SpiralStrategy(List<Rectangle> rectangles, Point center)
        {
            Rectangles = rectangles;
            Center = center;
        }

        public Rectangle PlaceRectangle(Rectangle newRectangle)
        {
            return MoveToValidPosition(newRectangle);
        }

        private bool RectangleHasCollisions(Rectangle rectangle)
        {
            return Rectangles.Any(r => r.IntersectsWith(rectangle));
        }

        private Rectangle MoveToValidPosition(Rectangle rectangle)
        {
            for (var step = 1; RectangleHasCollisions(rectangle); step++)
            {
                var x = step * Math.Cos(step);
                var y = step * Math.Sin(step);
                rectangle.Location = new Point((int)x + Center.X, (int)y + Center.Y);
            }

            return rectangle;
        }
    }
}
