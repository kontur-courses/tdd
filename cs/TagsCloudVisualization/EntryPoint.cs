using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    public class EntryPoint
    {
        public static void Main(string[] args)
        {
            var layouter = new CircularCloudLayouter(new Point(500, 500));
            var rectangles = layouter.PutNextRectangles(GenerateRectangles(SizeCreator_Random, start: 200, count:100))
                                     .ToList();
            Console.Out.WriteLine(rectangles.Count);
            DrawRectangles(rectangles, layouter.Center, expandPercent: 250, name: "random");
        }

        public static IEnumerable<Size> GenerateRectangles(Func<int, int, Size> sizeCreator, int start = 50, int count = 50)
        {
            return Enumerable.Range(start, count)
                             .Select(sizeCreator);
        }

        public static Size SizeCreator_SlowDecreasing(int n, int i)
        {
            return new Size((125 - n + i) % 125 + 1, n % 50 + 1);
        }

        public static Size SizeCreator_Random(int n, int i)
        {
            var random = new Random(42);
            return new Size(random.Next(10, 200), random.Next(10, 200));
        }
        public static Size SizeCreator_Decreasing(int n, int i)
        {
            return new Size(Math.Abs(300 -n), Math.Abs(300 - n));
        }
        public static void DrawRectangles(
            IEnumerable<Rectangle> rectangles,
            Point center,
            string name = "image",
            int expandPercent = 100)
        {
            var path = Path.Combine("..", "..", name.Replace(Path.AltDirectorySeparatorChar, '_') + ".png");

            var rectangleList = rectangles.ToList();
            var cloudSize = rectangleList.GetSize();
            var width = cloudSize.Width;
            var height = cloudSize.Height;
            width *= expandPercent / 100;
            height *= expandPercent / 100;

            var image = new Bitmap(width, height);
            var graphics = Graphics.FromImage(image);

            foreach (var rectangle in rectangleList)
            {
                var imageCenter = new Point(width / 2, height / 2) - center;
                rectangle.Offset(imageCenter);
                graphics.DrawRectangle(new Pen(Color.LimeGreen, 5), rectangle);
                graphics.FillRectangle(Brushes.White, rectangle);
            }

            image.Save(path);
        }
    }
}
