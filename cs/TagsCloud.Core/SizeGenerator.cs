using System;
using System.Drawing;

namespace TagsCloud.Core
{
    internal static class SizeGenerator
    {
        private static readonly Random Random = new Random();

        public static Size GenerateSize(int minWidth, int maxWidth, int minHeight, int maxHeight)
        {
            return new Size(Random.Next(minWidth, maxWidth), Random.Next(minHeight, maxHeight));
        }
    }
}