using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization
{
    public class Program
    {
        public static void Main()
        {
            ICloudLayouter layouter = new CircularCloudLayouter(new Point(600, 600));
            var visualizer = new CloudVisualizator(1200, 1200, Color.BlueViolet, Color.DeepPink);
            var random = new Random();

            for (var i = 0; i < 80; i++)
            {
                var size = new Size(random.Next(5, 100), random.Next(5, 100));
                visualizer.DrawRectangle(layouter.PutNextRectangle(size));
            }

            visualizer.SaveImage("80rectangles_with_random_size.png", ImageFormat.Png);
        }
    }
}