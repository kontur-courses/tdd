using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualizer
{
    class Program
    {
        static void Main(string[] args)
        {
            var random = new Random();
            var spiral = new ArchimedeanSpiral(new Point(500,500));
            var CCL = new CircularCloudLayouter(spiral);

            var rectangles = new List<Rectangle>();
            for (int i = 0; i < 10000; i++)
            {
                var size = new Size(random.Next(1, 3), random.Next(1, 3));
                rectangles.Add(CCL.PutNewRectangle(size));
            }
            var bitmap = new Bitmap(1000, 1000);
            var graphics = Graphics.FromImage(bitmap);
            foreach (var rectangle in rectangles)
            {
                var r = random.Next(0, 255);
                var g = random.Next(0, 255);
                var b = random.Next(0, 255);
                graphics.DrawRectangle(new Pen(Color.FromArgb(r, g, b)), rectangle);
            }
            bitmap.Save("Bitmap3.bmp");
        }
    }
}
