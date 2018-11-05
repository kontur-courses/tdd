using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            var rectangles = new List<Rectangle>();
            var random = new Random();
            for (var i = 0; i < 100; i++)
            {
                var randomSize = new Size(random.Next(10, 20) + i, random.Next(20, 30) + i);
                var newRectangle = layouter.PutNextRectangle(randomSize);
                rectangles.Add(newRectangle);
            }
            var resBitmap = RectanglesVisualizer.Visualize(rectangles);
            resBitmap.Save("100 rectangles from small to big.bmp");
        }
    }
}