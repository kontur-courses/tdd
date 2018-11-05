using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class TagsCloudVisualizer
    {
        public static Bitmap GetPictureOfRectangles(List<Rectangle> rectangles)
        {
            var pictureRectangle = CalculatePictureRectangle(rectangles);
            var picture = new Bitmap(pictureRectangle.Width, pictureRectangle.Height);
            using (var graphics = Graphics.FromImage(picture))
            {
                graphics.TranslateTransform(-pictureRectangle.X, -pictureRectangle.Y);
                graphics.Clear(Color.Azure);
                var pen = new Pen(Color.Black, 10);
                graphics.DrawRectangles(pen, rectangles.ToArray());
            }

            return picture;
        }

        private static Rectangle CalculatePictureRectangle(List<Rectangle> rectangles)
        {
            var minTop = rectangles.Select(r => r.Top).Min();
            var maxBottom = rectangles.Select(r => r.Bottom).Max();
            var minLeft = rectangles.Select(r => r.Left).Min();
            var maxRight = rectangles.Select(r => r.Right).Max();

            return new Rectangle(
                new Point(minLeft, minTop), 
                new Size(maxRight - minLeft, maxBottom - minTop));
        }
    }
}
