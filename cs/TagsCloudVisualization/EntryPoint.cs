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
            var rectangles = layouter.PutNextRectangles(GenerateRectangles())
                                     .ToList();
            Console.Out.WriteLine(rectangles.Count);
            DrawRectangles(rectangles, layouter.Center, expandPercent: 200);
        }

        public static IEnumerable<Size> GenerateRectangles()
        {
            return Enumerable.Range(50, 25)
                             .Select((n, i) => new Size((125 - n * 2 + i) % 125 + 1, n % 50 + 1));
        }

        public static void DrawRectangles(
            IEnumerable<Rectangle> rectangles,
            Point center,
            string name = "image",
            int expandPercent = 100)
        {
            var path = Path.Combine("..", "..", name.Remove(Path.AltDirectorySeparatorChar) + ".png");

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
