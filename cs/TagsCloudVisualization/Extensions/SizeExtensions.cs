using System;
using System.Drawing;

namespace TagsCloudVisualization.Extensions
{
    public static class SizeExtensions
    {
        public static Size GenerateRandom(this Size size, int minWidth, int maxWidth, int minHeight, int maxHeight)
        {
            var random = new Random();
            size.Width = random.Next(minWidth, maxWidth);
            size.Height = random.Next(minHeight, maxHeight);
            return size;
        }
    }
}
