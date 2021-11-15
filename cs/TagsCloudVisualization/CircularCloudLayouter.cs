using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly List<Rectangle> rects;

        public Point LayoutCenter { get; }
        public IEnumerable<Rectangle> Rects => rects.AsReadOnly();


        public CircularCloudLayouter(Point center)
        {
            rects = new List<Rectangle>();
            LayoutCenter = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Размеры прямоугольника должны быть больше 0");

            var newRect = rects.Any()
                ? rects.SelectMany(rect => GetPossibleRectangles(rect, rectangleSize))
                    .MinBy(possibleRect => possibleRect.Distance).Rect
                : new Rectangle(
                    new Point(LayoutCenter.X - rectangleSize.Width / 2, LayoutCenter.Y - rectangleSize.Height / 2),
                    rectangleSize);

            rects.Add(newRect);
            return newRect;
        }

        public void ToBitmap(string path)
        {
            var margin = 50;

            if (!Rects.Any())
                throw new InvalidOperationException(
                    "Невозможно составить представление, т.к. в нём отсутствуют элементы.");

            var rectsUnion = Rects.Aggregate(Rectangle.Union);
            var canvas = new Rectangle(0, 0, rectsUnion.Width + margin, rectsUnion.Height + margin);
            var center = canvas.GetRectangleCenter();
            var delta = center - (Size) LayoutCenter;

            var bitmap = new Bitmap(canvas.Width, canvas.Height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.FillRectangle(new SolidBrush(Color.DimGray), canvas);
                foreach (var rect in Rects)
                {
                    graphics.FillRectangle(
                        new SolidBrush(Color.Coral),
                        new Rectangle(rect.Location + (Size) delta, rect.Size));
                    graphics.DrawRectangle(
                        new Pen(Color.Chocolate, 1),
                        new Rectangle(rect.Location + (Size) delta, rect.Size));
                }

                graphics.FillRectangle(
                    new SolidBrush(Color.Black),
                    center.X - 1, center.Y, 3, 1);
                graphics.FillRectangle(
                    new SolidBrush(Color.Black),
                    center.X, center.Y - 1, 1, 3);
            }

            bitmap.Save(path);
        }

        private IEnumerable<(Rectangle Rect, double Distance)> GetPossibleRectangles(Rectangle rect, Size newRectSize)
        {
            (Point, Point)[] waypoints =
            {
                (new Point(rect.Left - newRectSize.Width, rect.Top - newRectSize.Height),
                    new Point(rect.Right, rect.Top - newRectSize.Height)),
                (new Point(rect.Right, rect.Top - newRectSize.Height),
                    new Point(rect.Right, rect.Bottom)),
                (new Point(rect.Left - newRectSize.Width, rect.Bottom),
                    new Point(rect.Right, rect.Bottom)),
                (new Point(rect.Left - newRectSize.Width, rect.Top - newRectSize.Height),
                    new Point(rect.Left - newRectSize.Width, rect.Bottom))
            };

            foreach (var (start, stop) in waypoints)
            {
                for (var x = start.X; x <= stop.X; x++)
                {
                    for (var y = start.Y; y <= stop.Y; y++)
                    {
                        var newRect = new Rectangle(new Point(x, y), newRectSize);
                        var distance = newRect.GetRectangleCenter().GetDistance(LayoutCenter);
                        if (!rects.Any(r => r.IntersectsWith(newRect)))
                            yield return (newRect, distance);
                    }
                }
            }
        }
    }
}