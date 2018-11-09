using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly Point center;
        private int radius;
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }

        public CircularCloudLayouter(int x, int y)
        {
            center = new Point(x, y);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var angle = 0.0;
            Rectangle rectangle;
            do
            {
                rectangle = new Rectangle(
                    center.X - (rectangleSize.Width / 2) + (int)(radius * Math.Cos(angle)),
                    center.Y - (rectangleSize.Height / 2) + (int)(radius * Math.Sin(angle)),
                    rectangleSize.Width, rectangleSize.Height);

                angle += Math.PI / 18;
                if (angle < Math.PI * 2)
                    continue;

                angle = 0;
                radius++;
            } while (CheckCollisionWithAll(rectangle));

            if (rectangles.Count > 0)
                rectangle = MoveRectangleToCenter(rectangle);
            rectangles.Add(rectangle);
            return rectangle;
        }

        private Rectangle MoveRectangleToCenter(Rectangle rectangle)
        {
            Point originalLocation;
            do
            {
                originalLocation = new Point(rectangle.X, rectangle.Y);

                if (rectangle.X + (rectangle.Width / 2) > center.X)
                    rectangle = MoveRectangleIfNoCollision(rectangle, -1, 0);

                if (rectangle.Y + (rectangle.Height / 2) > center.Y)
                    rectangle = MoveRectangleIfNoCollision(rectangle, 0, -1);

                if (rectangle.X + (rectangle.Width / 2) < center.X)
                    rectangle = MoveRectangleIfNoCollision(rectangle, 1, 0);

                if (rectangle.Y + (rectangle.Height / 2) < center.Y)
                    rectangle = MoveRectangleIfNoCollision(rectangle, 0, 1);

            } while (originalLocation != rectangle.Location);
            return rectangle;
        }

        private Rectangle MoveRectangleIfNoCollision(Rectangle rectangle, int dx, int dy)
        {
            var changed = rectangle;
            changed.X += dx;
            changed.Y += dy;

            return CheckCollisionWithAll(changed) ? rectangle : changed;
        }
        private bool CheckCollisionWithAll(Rectangle rect)
        {
            foreach (var other in rectangles)
            {
                if (Geometry.IsRectangleIntersection(rect, other))
                    return true;
            }

            return false;
        }
    }
}
