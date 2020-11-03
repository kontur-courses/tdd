using System;
using System.Drawing;
using TagsCloud.Core;

namespace TagsCloud.Visualization
{
    internal class Program
    {
        private const int CountWords = 1000;
        private static readonly Random Random = new Random();

        private static void Main()
        {
            var cloud = new CircularCloudLayouter(new Point(CircularCloudVisualization.WindowWidth / 2,
                CircularCloudVisualization.WindowHeight / 2));

            for (var i = 0; i < CountWords; ++i)
                cloud.PutNextRectangle(new Size(Random.Next(25, 60), Random.Next(10, 20)));

            CircularCloudVisualization.SaveImage(cloud.Rectangles, $"../../Images/{CountWords}rectangles.jpg");
        }
    }
}