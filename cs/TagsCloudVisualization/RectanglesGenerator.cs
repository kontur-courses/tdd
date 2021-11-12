using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    internal static class RectanglesGenerator
    {
        internal static Rectangle[] GenerateSameRectanglesWithSize(int count, int minSize, int maxSize)
        {
            if (!AreCorrectSizeInterval(minSize, maxSize)) throw new ArgumentException("sizes should be positive");
            var rnd = new Random();
            var size = new Size(rnd.Next(minSize, maxSize), rnd.Next(maxSize));
            return GenerateRectangles(() => size, count);
        }

        internal static Rectangle[] GenerateDifferentRectangles(int count, int minSize, int maxSize)
        {
            if (!AreCorrectSizeInterval(minSize, maxSize)) throw new ArgumentException("sizes should be positive");
            var rnd = new Random();
            return GenerateRectangles(() => new Size(rnd.Next(minSize, maxSize), rnd.Next(minSize, maxSize)), count);
        }

        private static Rectangle[] GenerateRectangles(Func<Size> sizesGeneratorFunc, int count)
        {
            if (count <= 0)
                throw new ArgumentException("count should be positive");
            var layouter = new CircularCloudLayouter(new Point());
            for (var i = 0; i < count; i++)
            {
                var size = sizesGeneratorFunc();
                layouter.PutNextRectangle(size);
            }

            return layouter.GetRectangles();
        }

        private static bool AreCorrectSizeInterval(int minSize, int maxSize)
        {
            return minSize > 0 && maxSize > 0;
        }
    }
}