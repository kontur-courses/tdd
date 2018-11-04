using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var random = new Random();
            var rectangles = new List<Rectangle>();
            var layout = new CircularCloudLayouter();
            rectangles.Add(layout.PutNextRectangle(new Size(800,200)));
            for (var i = 0; i < 100; i++)
            {
                var randomSize = new Size(random.Next(150,200), random.Next(75, 100));
                var newRectangle = layout.PutNextRectangle(randomSize);
                rectangles.Add(newRectangle);
            }
            var visualizer = new CircularCloudVisualizer(layout);
            var bitmap = visualizer.DrawRectangles(layout.Rectangles);
            bitmap.Save("100_rectangles.png");
        }
    }
}
