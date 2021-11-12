using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        public static bool IntersectsWith(this Rectangle rectangle, IEnumerable<Rectangle> rectangles)
        {
            return rectangles.Any(r => r.IntersectsWith(rectangle));
        }

        public static Rectangle GetRectanglesContainer(this IEnumerable<Rectangle> rectangles)
        {
            if (rectangles == null)
                throw new ArgumentNullException(nameof(rectangles));
            if (!rectangles.Any())
                throw new ArgumentException($"{nameof(rectangles)} is empty", nameof(rectangles));
            var leftXCoordinate = rectangles.Min(rect => rect.Left);
            var rightXCoordinate = rectangles.Max(rect => rect.Right);
            var topYCoordinate = rectangles.Min(rect => rect.Top);
            var bottomYCoordinate = rectangles.Max(rect => rect.Bottom);
            return new Rectangle(new Point(leftXCoordinate, topYCoordinate),
                new Size(rightXCoordinate - leftXCoordinate, bottomYCoordinate - topYCoordinate));
        }

        public static int GetSummaryArea(this IEnumerable<Rectangle> rectangles)
        {
            return rectangles.Select(rect => rect.Width * rect.Height).Sum();
        }
    }
}