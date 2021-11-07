using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly List<Rectangle> rects;

        #region Props

        public Point LayoutCenter { get; }
        public IEnumerable<Rectangle> Rects => rects.AsReadOnly();

        #endregion

        public CircularCloudLayouter(Point center)
        {
            rects = new List<Rectangle>();
            LayoutCenter = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Размеры прямоугольника должны быть больше 0");

            var optimalPoint = GetNextRectOptimalPoint(rectangleSize);
            if (optimalPoint == null)
                throw new InvalidOperationException("Невозможно определить расположение нового прямоугольника");

            var newRect = new Rectangle((Point) optimalPoint, rectangleSize);
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
            var delta = canvas.GetRectangleCenter() - (Size) LayoutCenter;

            var bitmap = new Bitmap(canvas.Width, canvas.Height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.FillRectangle(new SolidBrush(Color.DimGray), canvas);
                foreach (var rect in Rects)
                {
                    var pen = new Pen(Color.Chocolate, 1);
                    graphics.DrawRectangle(pen, new Rectangle(rect.Location + (Size) delta, rect.Size));
                }
            }

            bitmap.Save(path);
        }

        private Point? GetNextRectOptimalPoint(Size rectSize)
        {
            if (!rects.Any())
                return new Point(LayoutCenter.X - rectSize.Width / 2, LayoutCenter.Y - rectSize.Height / 2);

            (Point? Point, double Distance) FindOptimalPoint(Point start, Point end)
            {
                (Point? Point, double Distance) optimalPoint = (null, double.PositiveInfinity);
                for (var x = start.X; x <= end.X; x++)
                {
                    for (var y = start.Y; y <= end.Y; y++)
                    {
                        var newPoint = new Point(x, y);
                        var tempRect = new Rectangle(newPoint, rectSize);
                        var distance = tempRect.GetRectangleCenter().GetDistance(LayoutCenter);

                        if (!rects.Any(r => r.IntersectsWith(tempRect)) && distance < optimalPoint.Distance)
                            optimalPoint = (newPoint, distance);
                    }
                }

                return optimalPoint;
            }

            var nextRectOptimalPoints = new List<(Point? Point, double Distance)>();
            foreach (var rect in rects)
            {
                (Point? Point, double Distance)[] optimalPoints =
                {
                    FindOptimalPoint(
                        new Point(rect.Left - rectSize.Width, rect.Top - rectSize.Height),
                        new Point(rect.Right, rect.Top - rectSize.Height)),
                    FindOptimalPoint(
                        new Point(rect.Right, rect.Top - rectSize.Height),
                        new Point(rect.Right, rect.Bottom)),
                    FindOptimalPoint(
                        new Point(rect.Left - rectSize.Width, rect.Bottom),
                        new Point(rect.Right, rect.Bottom)),
                    FindOptimalPoint(
                        new Point(rect.Left - rectSize.Width, rect.Top - rectSize.Height),
                        new Point(rect.Left - rectSize.Width, rect.Bottom))
                };

                nextRectOptimalPoints.Add(optimalPoints.MinBy(p => p.Distance));
            }

            return nextRectOptimalPoints.MinBy(p => p.Distance).Point;
        }
    }
}