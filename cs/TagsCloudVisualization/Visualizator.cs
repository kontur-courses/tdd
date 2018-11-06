using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class Visualizator
    {
        private readonly List<Rectangle> rectangles;

        public Visualizator(List<Rectangle> rectangles)
        {
            this.rectangles = rectangles;
        }

        public Bitmap CreateBitmap(Size size)
        {
            var bitmap = new Bitmap(size.Width, size.Height);
            var graphics = Graphics.FromImage(bitmap);
            var pen = new Pen(Color.Black);

            graphics.Clear(Color.White);
            if (rectangles.Any())
                graphics.DrawRectangles(pen, rectangles.ToArray());
            return bitmap;
        }
    }

    
}
