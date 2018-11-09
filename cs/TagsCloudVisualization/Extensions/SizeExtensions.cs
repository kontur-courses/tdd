using System;
using System.Drawing;

namespace TagsCloudVisualization.Extensions
{
    public static class SizeExtensions
    {
        public static Size SetRandom(this Size size, int minWidth, int maxWidth, int minHeight, int maxHeight)
        {
            var r = new Random();
            size.Width = r.Next(3, maxWidth);
            size.Height = r.Next(3, maxHeight);
            return size;
        }
    }
}
