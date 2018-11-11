using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main()
        {
            var layouter = new CircularCloudLayouter(new Point(1000, 500));
            var random = new Random();
            for (var i = 0; i < 100; i++)
            {
                var height = random.Next(20, 70);
                var width = random.Next(80, 150);
                layouter.PutNextRectangle(new Size(width, height));
            }

            var visualizer = new RectanglesVisualizer(layouter.Rectangles);
            var bitmap = visualizer.RenderToBitmap();
            var imageFormat = ImageFormat.Jpeg;
            var filename = string.Format("100rectangles.{0}", imageFormat.ToString().ToLower());

            bitmap.Save(filename, imageFormat);
        }
    }
}
