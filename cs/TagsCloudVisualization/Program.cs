using System;
using System.Drawing;
using System.IO;

namespace TagsCloudVisualization
{
    internal class Program
    {
        private static void Main()
        {
            var layouter = new CircularCloudLayouter(new Point(1500, 1500));
            var rnd = new Random();
            for (var i = 0; i < 1000; i++)
            {
                layouter.PutNextRectangle(new Size(rnd.Next(50, 200), rnd.Next(50, 200)));
            }

            var bmpVisualizer = new BitmapVisualizer(layouter.Rectangles.ToArray());
            bmpVisualizer.DrawRectangles(Color.Black, Color.Red);
            bmpVisualizer.Save("example.png");
        }
    }
}