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
        private double _angle;
        private readonly List<Rectangle> _rectangles = new List<Rectangle>();
        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }
        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var coefficient = 0;
            Rectangle rectangle;
            do
            {
                coefficient += 1;
                rectangle = new Rectangle(
                    Center.X - (rectangleSize.Width / 2) + (int)(coefficient * Math.Cos(_angle)),
                    Center.Y - (rectangleSize.Height / 2) - (int)(coefficient * Math.Sin(_angle)),
                    rectangleSize.Width, rectangleSize.Height);
            } while (CheckCollisionWithAll(rectangle));

            MoveRectangleToCenter(rectangle);
            _rectangles.Add(rectangle);
            _angle += 0.5; //Math.PI / 7;
            return rectangle;
        }

        public void MoveRectangleToCenter(Rectangle rectangle)
        {

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
            foreach (var r in _rectangles )
            {
                g.DrawRectangle(Pens.DodgerBlue, r);
            }

            //g.Save();
            bitmap.Save(fileName + ".bmp");

        }
    }
}
