using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class SizesGenerator
    {
        public static List<Size> GenerateSizesList(int count, Size minSize, Size maxSize)
        {
            if (minSize.Height >= maxSize.Height 
                || minSize.Width >= maxSize.Width)
                throw new ArgumentException("minSizes fields must be less than maxSizes fields");
            
            var random = new Random();
            var sizes = new List<Size>();
            for (var i = 0; i < count; i++)
            {
                var newSize = new Size(random.Next(minSize.Width, maxSize.Width),
                    random.Next(minSize.Height, maxSize.Height));
                sizes.Add(newSize);
            }

            return sizes;
        }
    }
}