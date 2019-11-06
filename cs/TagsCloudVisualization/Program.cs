using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var cloud = new CircularCloudLayouter(new Point(0, 0));
            var visualizer = new CloudVisualizer(cloud);
            var random = new Random();
            for (var i = 0; i < 500; i++)
            {
                cloud.PutNextRectangle(new Size(random.Next(1, 100), random.Next(1, 100)));
            }

            visualizer.GetAndSaveImage("haha.png");
        }
    }
}
