using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            DrawFirstExample();
            DrawSecondExample();
            DrawThirdExample();
        }

        public static void DrawFirstExample()
        {
            var sizeWindow = new Size(1000, 1000);
            var visualizer = new Visualizer(sizeWindow);
            var layouter = new CircularCloudLayouter(new Point(sizeWindow.Width/2, sizeWindow.Height/2));
            var random = new Random();
            var rectangles = new List<Rectangle>();
            for (int i = 0; i < 300; i++)
            {
                var size = new Size(random.Next(10, 50), random.Next(10, 50));
                rectangles.Add(layouter.PutNextRectangle(size));

            }
            visualizer.DrawRectangles(rectangles);
            visualizer.Save(Path.Combine(Directory.GetCurrentDirectory(), "example1.bmp"));
        }
        public static void DrawSecondExample()
        {
            var sizeWindow = new Size(1400, 1200);
            var visualizer = new Visualizer(sizeWindow);
            var layouter = new CircularCloudLayouter(new Point(sizeWindow.Width / 2, sizeWindow.Height / 2));
            var width = 60;
            var height = 30;
            var rectangles = new List<Rectangle>();
            for (int i = 0; i < 500; i++)
            {
                if (i % 100 == 0)
                    width -= 5;
                var size = new Size(width, height);
                rectangles.Add(layouter.PutNextRectangle(size));

            }
            visualizer.DrawRectangles(rectangles);
            visualizer.Save(Path.Combine(Directory.GetCurrentDirectory(), "example2.bmp"));
        }

        public static void DrawThirdExample()
        {
            var sizeWindow = new Size(1000, 1000);
            var visualizer = new Visualizer(sizeWindow);
            var layouter = new CircularCloudLayouter(new Point(sizeWindow.Width / 2, sizeWindow.Height / 2));
            var random = new Random();
            var rectangles = new List<Rectangle>();
            for (int i = 0; i < 100; i++)
            {
                var size = new Size(random.Next(5, 60), random.Next(5, 60));
                rectangles.Add(layouter.PutNextRectangle(size));

            }
            visualizer.DrawRectangles(rectangles);
            visualizer.Save(Path.Combine(Directory.GetCurrentDirectory(), "example3.bmp"));
        }
    }
}

