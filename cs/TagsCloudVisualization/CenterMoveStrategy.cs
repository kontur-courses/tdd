using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class CenterMoveStrategy : IPlacementStrategy
    {
        public List<Rectangle> Rectangles { get; }
        public Point Center { get; }

        public CenterMoveStrategy(List<Rectangle> rectangles, Point center)
        {
            Rectangles = rectangles;
            Center = center;
        }

        public Rectangle PlaceRectangle(Rectangle newRectangle)
        {
            return TryMoveCloserToCenter(newRectangle);
        }

        private bool RectangleHasCollisions(Rectangle rectangle)
        {
            return Rectangles.Any(r => r.IntersectsWith(rectangle));
        }

        private Rectangle TryMoveCloserToCenter(Rectangle rectangle)
        {
            var stepX = 1 * Math.Sign(Center.X - rectangle.Location.X);
            var stepY = 1 * Math.Sign(Center.Y - rectangle.Location.Y);

            var previousLocation = rectangle.Location;
            while (!RectangleHasCollisions(rectangle) && rectangle.X != Center.X + stepX)
            {
                previousLocation = rectangle.Location;
                rectangle = rectangle.Move(stepX, 0);
            }
            rectangle.Location = previousLocation;

            while (!RectangleHasCollisions(rectangle) && rectangle.Y != Center.Y + stepY)
            {
                previousLocation = rectangle.Location;
                rectangle = rectangle.Move(0, stepY);
            }
            rectangle.Location = previousLocation;

            return rectangle;
        }
    }
}
