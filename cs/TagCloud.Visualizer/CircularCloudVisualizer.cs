using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloud.Visualizer
{
    internal static class CircularCloudVisualizer
    {
        private static void Main()
        {
            for (var i = 1; i <= 3; i++)
            {
                var rectangles = CreateRandomRectangles(i * 20);
                FileCreator.CreateBitmapFile(rectangles.ToList(), i.ToString(), "images");
            }
        }

        private static IEnumerable<Rectangle> CreateRandomRectangles(int amount)
        {
            var rnd = new Random();
            var cloudLayouter = new CircularCloudLayouter(new Point(600, 600));
            for (var counter = 0; counter < amount; counter++)
                yield return cloudLayouter.PutNextRectangle(new Size(rnd.Next(50, 150), rnd.Next(50, 150)));
        }
    }
}