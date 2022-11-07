using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    internal class Program
    {
        public static int rectanglesCount = 100;
        public static int maxRectanglesHeight = 100;
        public static int maxRectanglesWidth = 100;

        static void Main()
        {
            var center = new Point(400, 400);
            var rectangleSizes = RectanglesRandomizer.GetSortedRectangles(
                maxRectanglesWidth, maxRectanglesHeight, rectanglesCount);

            var layouter = new CircularCloudLayouter(center, new Spiral(center));
            var rectangles = layouter.GetRectangles(rectangleSizes);
        }
    }
}