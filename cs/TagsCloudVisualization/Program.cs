using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    class Program
    {
        private static int minimumRectSize = 10;
        private static int maximumRectSize = 50;
        private static int rectanglesCount = 1000;
        private static readonly Point center = new Point(0, 0);

        private static void Main(string[] args)
        {
            if (!ParseArgs(args))
                return;
            var visualizer = new Visualizer();
            var rectangles = GenerateRandomRectangles(rectanglesCount);
            var bitmap = visualizer.Visualize(rectangles.ToArray());
            bitmap.Save("cloud.bmp");
            Console.WriteLine("Saved to " + Path.Combine(Directory.GetCurrentDirectory(), "cloud.bmp"));
        }

        private static bool ParseArgs(string[] args)
        {
            const string helpMessage =
@"Tag Cloud Visualization
Usage:
TagCloudVisualization.exe rectanglesCount [minimumRectSize] [maximumRectSize]";
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
                    minimumRectSize = intArgs[1];

                if (intArgs.Length > 2)
                    maximumRectSize = intArgs[2];
            }
            catch (Exception)
            {
                Console.WriteLine("InvalidArguments!\n");
                Console.WriteLine(helpMessage);
                return false;
            }

            if (maximumRectSize < minimumRectSize)
            {
                Console.WriteLine($"{nameof(maximumRectSize)} must be no more than {nameof(minimumRectSize)}");
                return false;
            }

            var max = CheckIfArgNonNegative(maximumRectSize, nameof(maximumRectSize));
            var min = CheckIfArgNonNegative(minimumRectSize, nameof(minimumRectSize));


            return max && min;
        }

        private static bool CheckIfArgNonNegative(int value, string name)
        {
            if (value >= 0)
                return true;
            Console.WriteLine(name + " must be non negative");
            return false;
        }

        private static IEnumerable<Rectangle> GenerateRandomRectangles(int count)
        {
            var layouter = new CircularCloudLayouter(center);
            var random = new Random(DateTime.Now.Millisecond);
            var sizes = new List<Size>(count);
            for (var i = 0; i < count; i++)
            {
                var width = random.Next(minimumRectSize, maximumRectSize);
                var height = random.Next(minimumRectSize, maximumRectSize);
                sizes.Add(new Size(width, height));
            }

            return AssignLayouter(sizes, layouter);
        }

        private static IEnumerable<Rectangle> AssignLayouter(IEnumerable<Size> sizes, CircularCloudLayouter layouter) =>
            sizes.Select(layouter.PutNextRectangle);
    }
}
