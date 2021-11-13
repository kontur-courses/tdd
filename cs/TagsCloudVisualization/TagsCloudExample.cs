using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class TagsCloudExample
    {
        public static List<Rectangle> GetExample1()
        {
            var layouter = new CircularCloudLayouter(Point.Empty);
            for (var j = 0; j < 4; j++)
            for (var i = 1; i < 250; i++)
                layouter.PutNextRectangle(new Size(i, i));
            return layouter.Rectangles;
        }

        public static List<Rectangle> GetExample2()
        {
            var random = new Random();
            var layouter = new CircularCloudLayouter(Point.Empty);
            for (var i = 1; i < 500; i++)
                layouter.PutNextRectangle(new Size(random.Next(20, 50), random.Next(20, 50)));
            return layouter.Rectangles;
        }

        public static List<Rectangle> GetExample3()
        {
            var layouter = new CircularCloudLayouter(Point.Empty);
            for (var i = 1; i < 500; i++)
                layouter.PutNextRectangle(new Size(20, 40));
            return layouter.Rectangles;
        }
    }
}