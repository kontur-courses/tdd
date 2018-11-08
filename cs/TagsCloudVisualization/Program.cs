using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    class Program
    {
        private static int minRectSize = 10;
        private static int maxRectSize = 50;
        private static int rectanglesCount = 1000;
        private static readonly Point Center = new Point(0, 0);

        private static void Main(string[] args)
        {
            if (!ParseArgs(args))
                return;
            var visualizer = new Visualizer();
            var rectangles = GenerateRandomRectangles(rectanglesCount);
            var bitmap = visualizer.Visualize(rectangles.ToArray());
            bitmap.Save("cloud" + ".bmp");
        }

        private static bool ParseArgs(string[] args)
        {
            const string helpMessage =
                "Tag Cloud Visualization\nUsage:\nTagCloudVisualization.exe rectanglesCount [minimumSize] [maximumSize]";
            if (args.Contains("help"))
            {
                Console.WriteLine(helpMessage);
                return false;
            }

            try
            {
                var intArgs = args.Select(int.Parse).ToArray();
                rectanglesCount = intArgs[0];
                if (intArgs.Length > 1)
                    minRectSize = intArgs[1];
                if (intArgs.Length > 2)
                    maxRectSize = intArgs[2];
            }
            catch (Exception)
            {
                Console.WriteLine("InvalidArguments!\n");
                Console.WriteLine(helpMessage);
                return false;
            }

            return true;
        }

        private static IEnumerable<Rectangle> GenerateRandomRectangles(int count)
        {
            var layouter = new CircularCloudLayouter(Center);
            var random = new Random(DateTime.Now.Millisecond);
            var sizes = new List<Size>(count);
            for (var i = 0; i < count; i++)
            {
                var width = random.Next(minRectSize, maxRectSize);
                var height = random.Next(minRectSize, maxRectSize);
                sizes.Add(new Size(width, height));
            }

            return AssignLayouter(sizes, layouter);
        }

        private static IEnumerable<Rectangle> AssignLayouter(IEnumerable<Size> sizes, CircularCloudLayouter layouter) =>
            sizes.Select(layouter.PutNextRectangle);
    }
}
