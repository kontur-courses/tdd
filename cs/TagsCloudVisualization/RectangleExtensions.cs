using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        /// <summary>
        /// Gets the center point of this rectangle
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static PointF Center(this Rectangle rectangle)
        {
            var x = rectangle.Left + rectangle.Width / 2.0f;
            var y = rectangle.Top + rectangle.Height / 2.0f;
            return new PointF(x, y);
        }

        /// <summary>
        /// Calculate distance from the center of this rectangle to specified point
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static double DistanceTo(this Rectangle rectangle, Point point)
        {
            var rectCenter = rectangle.Center();
            var distanceToPoint = Math.Sqrt(Math.Pow(point.X - rectCenter.X, 2) + Math.Pow(point.Y - rectCenter.Y, 2));
            return distanceToPoint;
        }
    }
}
