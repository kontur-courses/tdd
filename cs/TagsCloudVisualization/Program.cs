using System;
using System.Drawing;
using TagsCloudVisualization.TagCloud;

namespace TagsCloudVisualization
{
    internal class Program
    {
        private const int Width = 1920;
        private const int Height = 1080;

        static void Main(string[] args)
        {
            var size = new Size(Width, Height);
            var centerPoint = new Point(size.Width / 2, size.Height / 2);

            var layouter = new CircularCloudLayouter(centerPoint);

            var random = new Random();

            for (int i = 0; i < 157; i++)
            {
                layouter.PutNextRectangle(new Size(random.Next(20, 100), random.Next(20, 100)));
            }

            using (var bitmap = new Bitmap(Width, Height))
            using (var graphic = Graphics.FromImage(bitmap))
            {
                foreach (var rectangle in layouter.rectangles)
                {
                    var red = random.Next(256);
                    var green = random.Next(256);
                    var blue = random.Next(256);

                    graphic.DrawRectangle(new Pen(Color.FromArgb(red, green, blue)), rectangle);
                }

                bitmap.Save("testImage" + ".bmp");
            }
        }
    }
}
