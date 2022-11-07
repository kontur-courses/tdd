using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloud
{
    public static class RectangleSizeGenerator
    {
        private static Random _random;

        static RectangleSizeGenerator()
        {
            _random = new Random();
        }

        public static List<Size> GetRandomSizesList(int count, Size minSize, Size maxSize)
        {
            var result = new List<Size>();

            for (int i = 0; i < count; i++)
            {
                double scaleFactor = _random.NextDouble();

                int width = Math.Max(minSize.Width, (int)(scaleFactor * maxSize.Width));

                int height = Math.Max(minSize.Height, (int)(scaleFactor * maxSize.Height));

                result.Add(new Size(width, height));
            }

            return result;
        }

        public static List<Size> GetRandomSizesOrderedList(int count, Size minSize, Size maxSize)
        {
            return GetRandomSizesList(count, minSize, maxSize).OrderByDescending(s => s.Width * s.Height).ToList();
        }

        public static List<Size> GetConstantSizesList(int count, Size size)
        {
            var result = new List<Size>();

            for (int i = 0; i < count; i++)
                result.Add(size);

            return result;
        }

    }
}