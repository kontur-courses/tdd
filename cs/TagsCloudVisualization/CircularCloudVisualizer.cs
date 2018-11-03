using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    class CircularCloudVisualizer
    {
        private readonly Size bitmapSize;

        public CircularCloudVisualizer(CircularCloudLayouter layouter)
        {
            bitmapSize = layouter.LayoutSize;
        }

        public Bitmap DrawRectangles(List<Rectangle> rectangles)
        {
            var canvas = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            using (var graphics = Graphics.FromImage(canvas))
            {
                graphics.Clear(Color.Azure);
                foreach (var rectangle in rectangles)
                    graphics.DrawRectangle(new Pen(Color.BlueViolet, 3), rectangle);
            }

            return canvas;
        }

    }
}
