using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace TagsCloudVisualization
{
    class Program
    {
        private static readonly Random Random = new Random();
        static void Main(string[] args)
        {
            DrawExample(FirstFillFunc, "example1");
            DrawExample(SecondFillFunc, "example2");
            DrawExample(ThirdFillFunc, "example3");
        }

        public static List<Rectangle> FirstFillFunc(CircularCloudLayouter layouter)
        {
            var rectangles = new List<Rectangle>();
            for (var i = 0; i < 300; i++)
            {
                var size = new Size(Random.Next(10, 50), Random.Next(10, 50));
                rectangles.Add(layouter.PutNextRectangle(size));
            }

            return rectangles;
        }

        public static List<Rectangle> SecondFillFunc(CircularCloudLayouter layouter)
        {
            var width = 60;
            const int height = 30;
            var rectangles = new List<Rectangle>();
            for (var i = 0; i < 500; i++)
            {
                if (i % 100 == 0)
                    width -= 5;
                var size = new Size(width, height);
                rectangles.Add(layouter.PutNextRectangle(size));
            }

            return rectangles;
        }

        public static List<Rectangle> ThirdFillFunc(CircularCloudLayouter layouter)
        {
            var rectangles = new List<Rectangle>();
            for (var i = 0; i < 100; i++)
            {
                var size = new Size(Random.Next(5, 60), Random.Next(5, 60));
                rectangles.Add(layouter.PutNextRectangle(size));
            }

            return rectangles;
        }

        public static void DrawExample(Func<CircularCloudLayouter, List<Rectangle>> fillFunc, string nameExample)
        {
            var sizeWindow = new Size(1200, 1200);
            var visualizer = new Visualizer(sizeWindow);
            var layouter = new CircularCloudLayouter(new Point(sizeWindow.Width / 2, sizeWindow.Height / 2));

            var rectangles = fillFunc(layouter);

            visualizer.DrawRectangles(rectangles);
            visualizer.Save(Path.Combine(Directory.GetCurrentDirectory(), $"{nameExample}.bmp"));
        }
    }
}

