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
                sizes.Add(new Size(
                    sizesGenerator.Next(1, maxDimensionSize), 
                    sizesGenerator.Next(1, maxDimensionSize)));
            return sizes;
        }

        public static List<Size> GetRandomWordLikeSizes(int seed, int rectanglesCount,
            int minHeight,
            int maxHeight,
            int minWidthToHeightRatio = 2,
            int maxWidthToHeightRatio = 8,
            bool canBeVertical = true)
        {
            var sizesGenerator = new Random(seed);
            var sizes = new List<Size>();
            for (var i = 0; i < rectanglesCount; i++)
            {
                var height = sizesGenerator.Next(minHeight, maxHeight);
                var width = height * sizesGenerator.Next(minWidthToHeightRatio, maxWidthToHeightRatio);

                if (canBeVertical && sizesGenerator.Next() % 2 == 0)
                    (width, height) = (height, width);

                sizes.Add(new Size(width, height));
            }

            return sizes;
        }
    }
}