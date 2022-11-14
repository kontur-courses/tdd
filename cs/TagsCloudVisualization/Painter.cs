using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class Painter
    {
        private const double IndentCoefficient = 1.2;
        private static readonly Pen rectanglePen = new(Brushes.DeepSkyBlue);

        private static void DrawAxes(Graphics graphics, Size imageSize)
        {
            var top = new Point(imageSize.Width / 2, 0);
            var bottom = new Point(imageSize.Width / 2, imageSize.Height);
            var left = new Point(0, imageSize.Height / 2);
            var right = new Point(imageSize.Width, imageSize.Height / 2);
            graphics.DrawLine(Pens.White, top, bottom);
            graphics.DrawLine(Pens.White, left, right);
        }

        private static void DrawRectangles(Graphics graphics, List<Rectangle> rectangles)
        {
            rectangles.ForEach(t => graphics.DrawRectangle(new Pen(Brushes.DeepSkyBlue), t));
        }

        private static List<Rectangle> MakeRectangles(CircularCloudLayouter layouter, int rectanglesCount)
        {
            var generator = new RectangleGenerator(new Size(10, 5), new Size(30, 30));
            var rectangles = new List<Rectangle>();
            for (var i = 0; i < rectanglesCount; i++)
                rectangles.Add(
                    layouter.PutNextRectangle(generator.GetRandomRectangle()));
            return rectangles;
        }

        public static List<Rectangle> TransformRectangles(List<Rectangle> rectangles, Size newCanvas)
        {
            return rectangles.Select(t =>
                new Rectangle(CalcPositionForCanvas(t.Location, newCanvas), t.Size)
            ).ToList();
        }


        public static void CreatePicture(Point center, int rectanglesCount, string path)
        {
            var layouter = new CircularCloudLayouter(center);
            var rectangles = MakeRectangles(layouter, rectanglesCount);
            DrawRectanglesToFile(center, rectangles, path);
        }

        private static Point CalcPositionForCanvas(Point position, Size imageSize)
        {
            var x = position.X + imageSize.Width / 2;
            var y = position.Y + imageSize.Height / 2;
            return new Point(x, y);
        }

        public static void DrawRectanglesToFile(Point center, List<Rectangle> rectangles, string path)
        {
            var size = GetNewCanvasSize(rectangles, center);
            rectangles = TransformRectangles(rectangles, size);
            var b = new Bitmap(size.Width, size.Height);

            using (var g = Graphics.FromImage(b))
            {
                DrawAxes(g, size);
                DrawRectangles(g, rectangles);
            }
            b.Save(path);
        }

        private static Size GetNewCanvasSize(List<Rectangle> rectangles, Point center)
        {
            var minX = rectangles.Min(t => t.Left);
            var maxX = rectangles.Max(t => t.Right);
            var minY = rectangles.Min(t => t.Top);
            var maxY = rectangles.Max(t => t.Bottom);

            var width = (int)(IndentCoefficient * (maxX - minX) + 2 * Math.Abs(center.X));
            var height = (int)(IndentCoefficient * (maxY - minY) + 2 * Math.Abs(center.Y));
            return new Size(width, height);
        }
    }
}