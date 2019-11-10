using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Visualizer
    {
        private readonly Size pictureSize;

        public Visualizer(Size pictureSize)
        {
            this.pictureSize = pictureSize;
        }

        public Bitmap GetLayoutBitmap(IEnumerable<Rectangle> rectangles, Color backgroundColor, Color rectangleColor)
        {
            var bitmap = new Bitmap(pictureSize.Width, pictureSize.Height);
            var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(backgroundColor);
            foreach (var rectangle in rectangles)
            {
                graphics.DrawRectangle(new Pen(rectangleColor), rectangle);
            }

            return bitmap;
        }
    }
}