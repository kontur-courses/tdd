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
            for (var i = 0; i < 50; i++)
            {
                var height = random.Next(20, 70);
                var width = random.Next(80, 150);
                layouter.PutNextRectangle(new Size(width, height));
            }

            var visualizator = new Visualizator(layouter.Rectangles);
            var bitmap = visualizator.CreateBitmap(new Size(1920, 1080));
            var imageFormat = ImageFormat.Jpeg;
            var filename = string.Format("50rectangles.{0}", imageFormat.ToString().ToLower());

            bitmap.Save(filename, imageFormat);
        }
    }
}
