using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    class Program
    {
        private const int MinRectSize = 10;
        private const int MaxRectSize = 50;
        private const int BitmapsCount = 3;
        private static readonly Point Center = new Point(0 , 0);

        private static void Main()
        {
            var rectanglesCount = 100;
            var visualizer = new Visualizer();
            for (var i = 0; i < BitmapsCount; i++)
            {
                var rectangles = GenerateRandomRectangles(rectanglesCount);
                var bitmap = visualizer.Visualize(rectangles, Center);
                bitmap.Save("cloud" + (i + 1) + ".bmp");
                rectanglesCount *= 10;
            }
        }

        private static IEnumerable<Rectangle> GenerateRandomRectangles(int count)
        {
            var layouter = new CircularCloudLayouter(Center);
            var random = new Random(DateTime.Now.Millisecond);
            var sizes = new List<Size>(count);
            for (var i = 0; i < count; i++)
            {
                var width = random.Next(MinRectSize, MaxRectSize);
                var height = random.Next(MinRectSize, MaxRectSize);
                sizes.Add(new Size(width, height));
            }

            return AssignLayouter(sizes, layouter);
        }

        private static IEnumerable<Rectangle> AssignLayouter(IEnumerable<Size> sizes, CircularCloudLayouter layouter) =>
            sizes.Select(layouter.PutNextRectangle);
    }
}
