using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class RectangleUtils
    {    
        public static int GetSquareDistanceToPoint(Rectangle currentRect, Point point)
        {
            var rectCenter = new Point(
                currentRect.X + currentRect.Width / 2,
                currentRect.Y + currentRect.Height / 2);
            var squareDistanceToCenter = (rectCenter.X - point.X) * (rectCenter.X - point.X) +
                                         (rectCenter.Y - point.Y) * (rectCenter.Y - point.Y);
            return squareDistanceToCenter;
        }

        public static  IEnumerable<Rectangle> GetAdjacentRectangles(Rectangle rect)
        {
            yield return new Rectangle(new Point(rect.X - 1, rect.Y), rect.Size);
            yield return new Rectangle(new Point(rect.X + 1, rect.Y), rect.Size);
            yield return new Rectangle(new Point(rect.X, rect.Y - 1), rect.Size);
            yield return new Rectangle(new Point(rect.X, rect.Y + 1), rect.Size);
        }
    }
}