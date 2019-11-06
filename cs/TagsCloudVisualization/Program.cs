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
                new ImageParameters(new Point(0, 0), 20, 100, 100),
                new ImageParameters(new Point(50, 100), 50, 800, 300),
                new ImageParameters(new Point(500, -300), 100, 300, 100),
                new ImageParameters(new Point(500, -300), 500, 300, 100)
            };

            for (var i = 0; i < parameters.Count; i++)
            {
                var layouter = CreateCloud(parameters[i]);
                var format = ImageFormat.Png;
                var fileName = $"image{i}.{format}";

                var bitmap = visualizer.Draw(layouter.Cloud);
                bitmap.Save(fileName, format);
            }
        }

        private static CircularCloudLayouter CreateCloud(ImageParameters parameters)
        {
            var layouter = new CircularCloudLayouter(parameters.Center);
            var rand = new Random();
            var max = parameters.MaxSide;
            var min = parameters.MinSide;

            for (var i = 0; i < parameters.RectanglesCount; i++)
                layouter.PutNextRectangle(new Size(rand.Next(min, max), rand.Next(min, max)));

            return layouter;
        }

        private class ImageParameters
        {
            public ImageParameters(Point center, int rectanglesCount, int maxSide, int minSide)
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