using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    internal class Program
    {
        private const int CountWords = 500;
        private static readonly Random Random = new Random();

        private static void Main()
        {
            var cloud = new CircularCloudLayouter(new Point(CircularCloudVisualization.WindowWidth / 2,
                CircularCloudVisualization.WindowHeight / 2));

            for (var i = 0; i < CountWords; ++i)
                cloud.PutNextRectangle(new Size(Random.Next(20, 80), Random.Next(15, 30)));

            CircularCloudVisualization.SaveImage(cloud.Rectangles, $"../../Images/{CountWords}rectangles.jpg");
        }
    }
}