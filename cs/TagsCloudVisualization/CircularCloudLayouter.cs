using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private Point Center { get; }
        private ArchimedeanSpiral Spiral { get; }
        private List<Rectangle> Rectangles { get; }
        private string Root { get; }
        private int RightBorder { get; set; }
        private int BottomBorder { get; set; }

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            Spiral = new ArchimedeanSpiral(Center, 0.2, 0);
            Rectangles = new List<Rectangle>();
            Root = new DirectoryInfo("..\\..\\..\\Data").FullName;
            RightBorder = 0;
            BottomBorder = 0;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
            {
                throw new ArgumentException();
            }

            return GetNewRectangle(rectangleSize);
        }

        private Rectangle GetNewRectangle(Size rectangleSize)
        {
            var location = Spiral.GetNextPoint();
            var rectangle = new Rectangle(location, rectangleSize);

            while (Collided(rectangle))
            {
                location = Spiral.GetNextPoint();
                rectangle = new Rectangle(location, rectangleSize);
            }

            if (rectangle.Left < 0 || rectangle.Top < 0)
            {
                throw new ArgumentException();
            }

            rectangle = MoveToCenter(rectangle);
            UpdateImageBorers(rectangle);
            Rectangles.Add(rectangle);

            return rectangle;
        }

        private Rectangle MoveToCenter(Rectangle rectangle)
        {
            var movedRectangle = rectangle;

            while (!Collided(rectangle) &&
                   rectangle.X != Center.X &&
                   rectangle.Y != Center.Y)
            {
                movedRectangle = rectangle;
                var deltaX = Center.X - rectangle.X < 0 ? -1 : 1;
                var deltaY = Center.Y - rectangle.Y < 0 ? -1 : 1;

                var position = new Point(rectangle.X + deltaX, rectangle.Y + deltaY);
                rectangle = new Rectangle(position, rectangle.Size);
            }

            return movedRectangle;
        }

        private void UpdateImageBorers(Rectangle rectangle)
        {
            if (RightBorder < rectangle.Right)
            {
                RightBorder = rectangle.Right;
            }

            if (BottomBorder < rectangle.Bottom)
            {
                BottomBorder = rectangle.Bottom;
            }
        }

        private bool Collided(Rectangle newRectangle) =>
            Rectangles.Any(rectangle => rectangle.IntersectsWith(newRectangle));

        public string GenerateNewFilePath()
        {
            var dateTime = DateTime.Now;
            return $"{Root}\\{dateTime:MMddyy-HHmmssffffff}.jpg";
        }

        public void SaveDrawing(string savePath)
        {
            var pen = new Pen(Color.MediumVioletRed, 4);
            var bitmap = new Bitmap(RightBorder + (int)pen.Width, BottomBorder + (int)pen.Width);
            var graphics = Graphics.FromImage(bitmap);

            graphics.DrawRectangles(pen, Rectangles.ToArray());

            if (!Directory.Exists(savePath) || !savePath.EndsWith(".jpg"))
            {
                throw new ArgumentException();
            }

            bitmap.Save(savePath);
        }
    }
}