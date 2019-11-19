using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class RandomLayoutGenerator
    {
        public static IEnumerable<Rectangle> CreateRandomLayout(ICloudLayouter layouter, int minRectSize,
            int maxRectSize, int amountOfRectangles)
        {
            var random = new Random();
            var rectangles = new List<Rectangle>();
            for (var i = 0; i < amountOfRectangles; i++)
            {
                var randomValue1 = random.Next(minRectSize, maxRectSize + 1);
                var randomValue2 = random.Next(minRectSize, maxRectSize + 1);
                var randomSize = new Size(Math.Max(randomValue1, randomValue2), Math.Min(randomValue1, randomValue2));
                rectangles.Add(layouter.PutNextRectangle(randomSize));
            }
            return rectangles;
        }

        public static IEnumerable<Rectangle> CreateRandomCircularLayout(Point center, ISpiral spiral, int minRectSize,
            int maxRectSize, int amountOfRectangles)
        {
            var layouter = new CircularCloudLayouter(center, spiral);
            return CreateRandomLayout(layouter, minRectSize, maxRectSize, amountOfRectangles);
        }

        public static IEnumerable<Rectangle> CreateRandomCircularLayout(Point center,
          float thickness, int minRectSize, int maxRectSize, int amountOfRectangles)
        {
            var layouter = new CircularCloudLayouter(center, new ArchimedesSpiral(center, thickness));
            return CreateRandomLayout(layouter, minRectSize, maxRectSize, amountOfRectangles);
        }
    }
}