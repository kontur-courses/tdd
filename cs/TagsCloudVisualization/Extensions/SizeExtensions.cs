using System;
using System.Drawing;

namespace TagsCloudVisualization.Extensions
{
    public static class SizeExtensions
    {
        public static Size SetRandom(this Size size, int minWidth, int maxWidth, int minHeight, int maxHeight)
        {
            var r = new Random();
            size.Width = r.Next(minWidth, maxWidth);
            size.Height = r.Next(minHeight, maxHeight);
            return size;
        }
    }
}
