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
        private readonly List<Rectangle> _rectangles = new List<Rectangle>();
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

            if (_rectangles.Count > 0 && withDensity)
                rectangle = MoveRectangleToCenter(rectangle);
            _rectangles.Add(rectangle);
            return rectangle;
        }
        public Rectangle MoveRectangleToCenter(Rectangle rectangle)
        {
            //Console.WriteLine("Уплотняем прямоугольник №{0} в облако", _rectangles.Count);
            var wasChanged = true;
            while (wasChanged)
            {
                var original = new Point(rectangle.X, rectangle.Y);
                while (!CheckCollisionWithAll(rectangle) && rectangle.X + rectangle.Width / 2 > Center.X)
                    rectangle.X--;
                rectangle.X++;

                while (!CheckCollisionWithAll(rectangle) && rectangle.Y + rectangle.Height / 2 > Center.Y)
                    rectangle.Y--;
                rectangle.Y++;

                while (!CheckCollisionWithAll(rectangle) && rectangle.X + rectangle.Width / 2 < Center.X)
                    rectangle.X++;
                rectangle.X--;

                while (!CheckCollisionWithAll(rectangle) && rectangle.Y + rectangle.Height / 2 < Center.Y)
                    rectangle.Y++;
                rectangle.Y--;

                wasChanged = original.X != rectangle.X || original.Y != rectangle.Y;
            }

            return rectangle;
        }

        public bool IsCollision(Rectangle rectangle, Rectangle other)
        {
            return !(rectangle.X + rectangle.Width < other.X || other.X + other.Width < rectangle.X
                  || rectangle.Y + rectangle.Height < other.Y || other.Y + other.Height < rectangle.Y);
        }

        public bool CheckCollisionWithAll(Rectangle rect)
        {
            foreach (var other in _rectangles)
            {
                if (IsCollision(rect, other))
                    return true;
            }

            return false;
        }
        public void SaveBitmap(string fileName, int width, int height)
        {
            var bitmap = new Bitmap(width, height);
            var g = Graphics.FromImage(bitmap);
            g.FillRectangle(Brushes.LightBlue, 0, 0, width, height);
            foreach (var r in _rectangles )
            {
                g.FillRectangle(Brushes.RoyalBlue, r);
                g.DrawRectangle(Pens.DarkBlue, r);
            }

            bitmap.Save(fileName + ".bmp");
        }
    }
}
