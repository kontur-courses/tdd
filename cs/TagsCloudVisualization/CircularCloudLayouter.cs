using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using static NUnit.Framework.Constraints.Tolerance;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public Point Center;
        public List<Rectangle> WordPositions;
        public double Radius;
        public double Angle;

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            Radius = 0;
            Angle = 0;
            WordPositions = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            while (true)
            {
                if (rectangleSize.Width < 1 || rectangleSize.Height < 1)
                    throw new ArgumentException();

                var x = Radius * Math.Cos(Angle) + Center.X;
                var y = Radius * Math.Sin(Angle) + Center.Y;

                var currRectangle = new Rectangle(new Point((int)x, (int)y), rectangleSize);

                if (!CheckIntersection(currRectangle))
                {
                    currRectangle = RectangleCompression(currRectangle);
                    WordPositions.Add(currRectangle);
                    return currRectangle;
                }

                Angle += 0.1;

                if (Angle >= Math.PI * 2)
                {
                    Angle = 0;
                    Radius += 0.1;
                }
            }
        }

        public bool CheckIntersection(Rectangle currRectangle)
        {
            return WordPositions.Any(rec =>
            {
                var x = Math.Max(currRectangle.X, rec.X);
                var num1 = Math.Min(currRectangle.X + currRectangle.Width, rec.X + rec.Width);
                var y = Math.Max(currRectangle.Y, rec.Y);
                var num2 = Math.Min(currRectangle.Y + currRectangle.Height, rec.Y + rec.Height);
                var res = num1 > x && num2 > y ? new Rectangle(x, y, num1 - x, num2 - y) : Rectangle.Empty;
                return !res.IsEmpty;
            });
        }

        public Rectangle RectangleCompression(Rectangle rectangle)
        {
            var changes = 1;
            while (changes > 0)
            {
                CompressionsAxis(ref rectangle, ref changes, Center.X,
                    (Point point) => point.X,
                    (Point point, int step) => new Point(point.X + step, point.Y));
                CompressionsAxis(ref rectangle, ref changes, Center.Y,
                    (Point point) => point.Y,
                    (Point point, int step) => new Point(point.X, point.Y + step));
            }

            return rectangle;
        }

        public void CompressionsAxis(ref Rectangle rectangle, ref int changes, int targetCoord,
            Func<Point, int> currCoord,
            Func<Point, int, Point> changePoint)
        {
            var step = (currCoord(rectangle.Location) < targetCoord) ? 1 : -1;
            changes = 0;
            while (currCoord(rectangle.Location) != targetCoord)
            {
                var newRectangle =
                    new Rectangle(changePoint(new Point(rectangle.X, rectangle.Y), step), rectangle.Size);
                if (!CheckIntersection(newRectangle))
                {
                    rectangle = newRectangle;
                    changes += 1;
                    continue;
                }

                break;
            }
        }

        public void Drawing(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            int w = 500;
            int h = 500;

            Bitmap bitmap = new Bitmap(w, h);
            Graphics graphics = Graphics.FromImage(bitmap);
            Random random = new Random();

            foreach (var word in WordPositions)
            {
                Color randomColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
                Brush brush = new SolidBrush(randomColor);
                var x = word.X + w/2;
                var y = word.Y + h/2;
                graphics.FillRectangle(brush, x, y, word.Width, word.Height);
            }

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string filePath = Path.Combine(projectDirectory, "images",name);
            Console.WriteLine(filePath);
            bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}