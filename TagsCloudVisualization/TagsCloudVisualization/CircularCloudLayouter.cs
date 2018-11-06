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
        public Point Center { get; }
        private int distance;
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        public List<Rectangle> Result { get => new List<Rectangle>(rectangles); }
        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }
        public Rectangle PutNextRectangle(Size rectangleSize, bool withDensity=true)
        {
            var angle = 0.0;
            Rectangle rectangle;
            do
            {
                rectangle = new Rectangle(
                    Center.X - (rectangleSize.Width / 2) + (int)(distance * Math.Cos(angle)),
                    Center.Y - (rectangleSize.Height / 2) + (int)(distance * Math.Sin(angle)),
                    rectangleSize.Width, rectangleSize.Height);

                angle += Math.PI / 18;
                if (angle >= Math.PI * 2)
                {
                    angle = 0;
                    distance++;
                }
            } while (CheckCollisionWithAll(rectangle));

            if (rectangles.Count > 0 && withDensity)
                rectangle = MoveRectangleToCenter(rectangle);
            rectangles.Add(rectangle);
            return rectangle;
        }
        private Rectangle MoveRectangleToCenter(Rectangle rectangle)
        {
            var wasChanged = true;
            while (wasChanged)
            {
                var original = new Point(rectangle.X, rectangle.Y);

                if (rectangle.X + (rectangle.Width / 2) > Center.X)
                    rectangle.X--;
                if (CheckCollisionWithAll(rectangle))
                    rectangle.X++;

                if (rectangle.Y + (rectangle.Height / 2) > Center.Y)
                    rectangle.Y--;
                if (CheckCollisionWithAll(rectangle))
                    rectangle.Y++;

                if (rectangle.X + (rectangle.Width / 2) < Center.X)
                    rectangle.X++;
                if (CheckCollisionWithAll(rectangle))
                    rectangle.X--;

                if (rectangle.Y + (rectangle.Height / 2) < Center.Y)
                    rectangle.Y++;
                if (CheckCollisionWithAll(rectangle))
                    rectangle.Y--;

                wasChanged = original != rectangle.Location;
            }
            return rectangle;
        }

        public bool IsCollision(Rectangle rectangle, Rectangle other)
        {
            return !(rectangle.X + rectangle.Width < other.X || other.X + other.Width < rectangle.X
                  || rectangle.Y + rectangle.Height < other.Y || other.Y + other.Height < rectangle.Y);
        }

        private bool CheckCollisionWithAll(Rectangle rect)
        {
            foreach (var other in rectangles)
            {
                if (IsCollision(rect, other))
                    return true;
            }

            return false;
        }
    }
}
