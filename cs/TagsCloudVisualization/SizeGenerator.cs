using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class SizeGenerator
    {
        private readonly Random random;
        private readonly int maxHeight;
        private readonly int maxWidth;
        private readonly int minHeight;
        private readonly int minWidth;

        public SizeGenerator(int minWidth, int maxWidth, int minHeight, int maxHeight)
        {
            random = new Random();
            this.minWidth = minWidth;
            this.maxWidth = maxWidth;
            this.minHeight = minHeight;
            this.maxHeight = maxHeight;
        }

        public List<Size> GenerateSizes(int count)
        {
            var generatedSizes = new List<Size>();

            for (var i = 0; i < count; i++)
            {
                generatedSizes.Add(new Size(random.Next(minWidth, maxWidth), random.Next(minHeight, maxHeight)));
            }

            return generatedSizes;
        }
    }
}