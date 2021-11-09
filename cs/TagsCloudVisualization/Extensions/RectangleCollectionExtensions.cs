using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class RectangleCollectionExtensions
    {
        public static Size GetSize(this IEnumerable<Rectangle> rectangles)
        {
            var (right, bottom) = (int.MinValue, int.MinValue);
            var (left, top) = (int.MaxValue, int.MaxValue);
            foreach (var rectangle in rectangles)
            {
                right = Math.Max(right, rectangle.Right);
                bottom = Math.Max(bottom, rectangle.Bottom);
                left = Math.Min(left, rectangle.Left);
                top = Math.Min(top, rectangle.Top);
            }
            return new Size(right - left, bottom - top);
        }

        public static bool IsAnyInCollectionIntersects(this IEnumerable<Rectangle> rectangles)
        {
            return rectangles
                .Any(rectangle => rectangle.IntersectsWith(rectangles.Where(rec => rec != rectangle)));
        }
    }
}