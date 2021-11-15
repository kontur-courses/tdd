using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var layouter = new CircularCloudLayouter(new SpiralPointsGenerator(new Point(500, 500)));
            for (var i = 0; i < 500; i++)
            {
                layouter.PutNextRectangle(GenerateRandomSize(new Size(10, 10), new Size(60, 60)));
            }

            CloudVisualizer.Draw(layouter, new List<Color>()
                {
                    Color.Aqua,
                    Color.Blue,
                    Color.Aquamarine,
                    Color.Cyan,
                    Color.Indigo,
                    Color.Navy
                }, Color.Black)
                .Save("../../Samples/RandomSizes.png", ImageFormat.Png);
        }

        private static Size GenerateRandomSize(Size minSize, Size maxSize)
        {
            var rnd = new Random();
            return new Size(rnd.Next(minSize.Width, maxSize.Width), rnd.Next(minSize.Height, maxSize.Height));
        }
    }
}