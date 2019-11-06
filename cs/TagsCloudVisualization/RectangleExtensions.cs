using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions // TODO: move
    {
        public static Point GetRectangleCenter(this Rectangle rectangle)
        {
            var xCenter = (int)Math.Round(rectangle.X + rectangle.Width / 2.0, MidpointRounding.AwayFromZero);
            var yCenter = (int)Math.Round(rectangle.Y - rectangle.Height / 2.0, MidpointRounding.AwayFromZero);

            return new Point(xCenter, yCenter);
        }

        private static double GetDiagonal(this Rectangle rectangle) =>
            Math.Sqrt(rectangle.Width * rectangle.Width + rectangle.Height * rectangle.Height);

        public static double GetCircumscribedCircleRadius(this Rectangle rectangle) => rectangle.GetDiagonal() / 2;

        public static Rectangle CreateMovedCopy(this Rectangle rectangle, Size offset) =>
            new Rectangle(rectangle.Location + offset, rectangle.Size);

        public static bool IntersectsWith(this Rectangle rectangle, IEnumerable<Rectangle> otherRectangles) =>
            otherRectangles.All(otherRectangle => !otherRectangle.IntersectsWith(rectangle));
    }
}