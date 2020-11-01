using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    internal class Program
    {
        private const int CountWords = 1000;
        private const int WindowWidth = 1920;
        private const int WindowHeight = 1080;
        private static readonly Random Random = new Random();

        private static void Main()
        {
            var cloud = new CircularCloudLayouter(new Point(WindowWidth / 2, WindowHeight / 2));

            for (var i = 0; i < CountWords; ++i)
                cloud.PutNextRectangle(new Size(Random.Next(30, 50), Random.Next(15, 20)));

            CircularCloudVisualization.SaveImage(cloud.Rectangles, WindowWidth, WindowHeight,
                $"{CountWords}rectangles.jpg");
        }
    }
}