using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Extensions
{
    public static class RectangleExtensions
    {
        public static Point GetCenter(this Rectangle rect)
        {
            return new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
        }

        public static IEnumerable<Point> GetBounds(this Rectangle rectangle)
        {
            yield return new Point(rectangle.Left, rectangle.Top);
            yield return new Point(rectangle.Right, rectangle.Top);
            yield return new Point(rectangle.Right, rectangle.Bottom);
            yield return new Point(rectangle.Left, rectangle.Bottom);
        }

        public static double CalculateSquare(this Rectangle rectangle)
        {
            return SquareCalculator.CalculateRectangleSquare(rectangle.Width, rectangle.Height);
        }
    }
}