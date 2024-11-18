using System.Drawing;

namespace TagsCloudVisualization
{
    internal static class CircularCloudLayouterPainter
    {
        private static int _defaultPadding = 10;

        public static Bitmap Draw(IList<Rectangle> rectangles, int? paddingPerSide = null)
        {
            if (rectangles.Count == 0)
            {
                throw new ArgumentException("Список прямоугольников пуст.");
            }

            var correctPaddingPerSide = paddingPerSide ?? _defaultPadding;

            var minimums = new Point(rectangles.Min(r => r.Left), rectangles.Min(r => r.Top));
            var maximums = new Point(rectangles.Max(r => r.Right), rectangles.Max(r => r.Bottom));

            var imageSize = GetImageSize(minimums, maximums, correctPaddingPerSide);
            var result = new Bitmap(imageSize.Width, imageSize.Height);

            using var graphics = Graphics.FromImage(result);
            graphics.Clear(Color.White);
            using var pen = new Pen(Color.Black, 1);
            for (var i = 0; i < rectangles.Count; i++)
            {
                var positionOnCanvas = GetPositionOnCanvas(
                    rectangles[i],
                    minimums,
                    correctPaddingPerSide);
                graphics.DrawRectangle(
                    pen,
                    positionOnCanvas.X,
                    positionOnCanvas.Y,
                    rectangles[i].Width,
                    rectangles[i].Height);
            }

            return result;
        }

        private static Point GetPositionOnCanvas(Rectangle rectangle, Point minimums, int padding)
            => new Point(rectangle.X - minimums.X + padding, rectangle.Y - minimums.Y + padding);

        private static Size GetImageSize(Point minimums, Point maximums, int paddingPerSide)
            => new Size(maximums.X - minimums.X + 2 * paddingPerSide, maximums.Y - minimums.Y + 2 * paddingPerSide);
    }
}