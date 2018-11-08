using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class TagsCloudVisualizer
    {
        public static Bitmap GetPictureOfRectangles(IReadOnlyList<Rectangle> rectangles)
        {
            var pictureRectangle = CalculatePictureRectangle(rectangles);
            var picture = new Bitmap(pictureRectangle.Width, pictureRectangle.Height);
            using (var graphics = Graphics.FromImage(picture))
            {
                graphics.TranslateTransform(-pictureRectangle.X, -pictureRectangle.Y);
                graphics.Clear(Color.FromArgb(22, 44, 79));
                var pen = new Pen(Color.DarkOrange, 3);
                graphics.DrawRectangles(pen, rectangles.ToArray());

            }

            return picture;
        }

        private static Rectangle CalculatePictureRectangle(IReadOnlyList<Rectangle> rectangles)
        {
            var minTop = rectangles.Min(r => r.Top);
            var maxBottom = rectangles.Max(r => r.Bottom);
            var minLeft = rectangles.Min(r => r.Left);
            var maxRight = rectangles.Max(r => r.Right);

            return new Rectangle(
                new Point(minLeft, minTop), 
                new Size(maxRight - minLeft, maxBottom - minTop));
        }
    }
}
