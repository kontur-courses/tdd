using System;
using System.Collections.Generic;
using System.Drawing;
using TagsCloudVisualization.Tests;

namespace TagsCloudVisualization
{
    public static class Program
    {
        public static void Main()
        {
            var layouter = new CircularCloudLayouter(new Point(250, 250));
            var rectangles = new List<Rectangle>();
            for (var i = 0; i < 100; i++)
                rectangles.Add(layouter.PutNextRectangle(new Size(15, 15)));
            var painter = new Painter(new Size(500, 500));
            var image = painter.GetMultiColorCloud(rectangles);
            Saving.SaveImageToDefaultDirectory("example", image);
        }
    }
}