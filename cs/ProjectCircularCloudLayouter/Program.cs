using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ProjectCircularCloudLayouter
{
    class Program
    {
        static void Main()
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            GenerateRectangles.MakeLayouter(layouter, 5000, 50, 100,
                20, 45);
            layouter.MakeImageTagsCircularCloud("circularCloud.bmp", ImageFormat.Bmp);
            Console.WriteLine("Изображение сохранено");
        }
    }
}