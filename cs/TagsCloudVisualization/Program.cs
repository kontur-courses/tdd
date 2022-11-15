using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using TagsCloudVisualization.TagCloud;

namespace TagsCloudVisualization
{
    public class Program
    {
        private const int Width = 1920;
        private const int Height = 1080;

        static void Main(string[] args)
        {
            var size = new Size(Width, Height);
            var centerPoint = new Point(size.Width / 2, size.Height / 2);

            var layouter = new CircularCloudLayouter(centerPoint);

            var random = new Random();

            for (int i = 0; i < 500; i++)
            {
                layouter.PutNextRectangle(new Size(random.Next(20, 50), random.Next(20, 50)));
            }

            GenerateImage(layouter, Environment.CurrentDirectory, "testImage");
        }

        public static string GenerateImage(CircularCloudLayouter layouter, string directory, string fileName)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var random = new Random();
            using (var bitmap = new Bitmap(Width, Height))
            using (var graphic = Graphics.FromImage(bitmap))
            {
                graphic.Clear(Color.DimGray);

                foreach (var rectangle in layouter.rectangles)
                {
                    var red = random.Next(256);
                    var green = random.Next(256);
                    var blue = random.Next(256);
                    graphic.DrawRectangle(new Pen(Color.FromArgb(red, green, blue), 2), rectangle);
                }

                var path = Path.Combine(directory, fileName) + ".bmp";
                bitmap.Save(path, ImageFormat.Bmp);

                return path;
            }
        }
    }
}
