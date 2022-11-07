using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class RandomRectanglesCreator
    {
        public static Rectangle[] GetRectangles(int count, Point center, double spiralStep = 1d)
        {
            var circularCloudLayouter = new CircularCloudLayouter(center, spiralStep);
            var rectangles = GetSizesOfRectangle(count)
                .OrderByDescending(size => size.Height)
                .ThenByDescending(size => size.Width)
                .Select(size => circularCloudLayouter.PutNextRectangle(size))
                .ToArray();
            return rectangles;
        }

        private static IEnumerable<Size> GetSizesOfRectangle(int count,
            int minWidth = 10, int maxWidth = 100, int minHeight = 5, int maxHeight = 40)
        {
            var rd = new Random();
            for (var i = 0; i < count; i++)
            {
                var width = rd.Next(minWidth, maxWidth);
                var height = rd.Next(minHeight, maxHeight);
                if (width < height)
                {
                    i--;
                    continue;
                }
                yield return new Size(width, height);
            }
        }
    }
}