using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class RectangleSizeProvider
    {

        public static List<Size> GetRandomSizes(int seed, int rectanglesCount, int maxDimensionSize)
        {
            var sizesGenerator = new Random(seed);
            var sizes = new List<Size>();
            for (var i = 0; i < rectanglesCount; i++)
                sizes.Add(new Size(sizesGenerator.Next(maxDimensionSize), sizesGenerator.Next(maxDimensionSize)));
            return sizes;
        }
    }
}