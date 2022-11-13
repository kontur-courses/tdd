using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class RectangleCoordinatesCalculator
    {
        public static Point CalculateRectangleCoordinates(Point rectangleCenter, Size rectangleSize)
        {
            if (rectangleSize.Height < 0 || rectangleSize.Width < 0)
                throw new ArgumentException("Incorrect size of rectangle");
            return new Point(rectangleCenter.X - rectangleSize.Width / 2, rectangleCenter.Y - rectangleSize.Height / 2);
        }
    }
}