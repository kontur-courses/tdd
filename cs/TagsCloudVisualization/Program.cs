using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization
{
    public class Program
    {
        static void Main(string[] args)
        {
            var visualizer = new CloudVisualizer(6000, 4000);

            var parameters = new List<ImageParameters>()
            {
                new ImageParameters(new Point(0, 0), 20, 100, 200),
                new ImageParameters(new Point(50, 100), 50, 300, 800),
                new ImageParameters(new Point(500, -300), 100, 100, 300),
                new ImageParameters(new Point(500, -300), 500, 100, 300)
            };

            for (var i = 0; i < parameters.Count; i++)
            {
                var cloud = CreateCloud(parameters[i]);
                var format = ImageFormat.Png;
                var fileName = $"image{i}.{format}";

                var bitmap = visualizer.Draw(cloud);
                bitmap.Save(fileName, format);
            }
        }

        private static Cloud CreateCloud(ImageParameters parameters)
        {
            var layouter = new CircularCloudLayouter(parameters.Center);
            var rand = new Random();
            var max = parameters.MaxSide;
            var min = parameters.MinSide;

            for (var i = 0; i < parameters.RectanglesCount; i++)
                layouter.PutNextRectangle(new Size(rand.Next(min, max), rand.Next(min, max)));

            return layouter.Cloud;
        }

        private class ImageParameters
        {
            public ImageParameters(Point center, int rectanglesCount, int minSide, int maxSide)
            {
                Center = center;
                RectanglesCount = rectanglesCount;
                MaxSide = maxSide;
                MinSide = minSide;
            }

            public Point Center { get; }
            public int RectanglesCount { get; }
            public int MaxSide { get; }
            public int MinSide { get; }
        }
    }
}