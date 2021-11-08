using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    internal class Program
    {
        public static void Main(string[] args)
        {
        }

        private static List<Rectangle> Gen1()
        {
            var layouter = new CircularCloudLayouter(Point.Empty);
            for (var i = 1; i < 500; i++)
                layouter.PutNextRectangle(new Size(i, i));
            return layouter.Rectangles;
        }

        private static List<Rectangle> Gen2()
        {
            var random = new Random();
            var layouter = new CircularCloudLayouter(Point.Empty);
            for (var i = 1; i < 500; i++)
                layouter.PutNextRectangle(new Size(random.Next(20, 50), random.Next(20, 50)));
            return layouter.Rectangles;
        }

        private static List<Rectangle> Gen3()
        {
            var layouter = new CircularCloudLayouter(Point.Empty);
            for (var i = 1; i < 500; i++)
                layouter.PutNextRectangle(new Size(5, 5));
            return layouter.Rectangles;
        }
    }
}