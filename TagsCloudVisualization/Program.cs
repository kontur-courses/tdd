using System;
using System.Drawing;
using System.IO;
namespace TagCloud
{
    class Program
    {
        static void Main()
        {
            var center = new Point(600, 400);
            var rnd = new Random(0);
            var layouter = new CircularCloudLayouter(center, new Spiral(center));
            for (int k = 0; k < 3; k++)
            {
                layouter = new CircularCloudLayouter(center, new Spiral(center));
                for (int i = 0; i < 200; i++)
                {
                    var size = new Size(rnd.Next(8 + k * 2, 60 + k * 100), rnd.Next(6 + k, 40 + k * 100));
                    layouter.PutNextRectangle(size);
                }
                VisualizationPrinter.Print($"Случайные прямоугольники {k}", layouter.Rectangles, center);
            }
            layouter = new CircularCloudLayouter(center, new Spiral(center));
            for (int i = 0; i < 100; i++)
            {
                var size = new Size(100, 100);
                layouter.PutNextRectangle(size);
            }
            VisualizationPrinter.Print("Квадраты", layouter.Rectangles, center);
        }
    }
}
