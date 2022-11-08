using System;
using System.Drawing;
using TagsCloudVisualization.TagCloud;

namespace TagsCloudVisualization
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var size = new Size(1920, 1080);
            var centerPoint = new Point(size.Width / 2, size.Height / 2);

            var ccl = new CircularCloudLayouter(centerPoint);

            var random = new Random();

            for (int i = 0; i < 157; i++)
            {
                ccl.PutNextRectangle(new Size(random.Next(20, 100), random.Next(20, 100)));
            }

            using (var bitmap = new Bitmap(1920, 1080))
            using (var graphic = Graphics.FromImage(bitmap))
            {
                foreach (var rectangle in ccl.rectangles)
                {
                    graphic.DrawRectangle(
                        new Pen(Color.FromArgb(random.Next(256), random.Next(256), random.Next(256))),
                        rectangle);
                }

                bitmap.Save("testImage" + ".bmp");
            }
        }
    }
}
