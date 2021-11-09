using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    internal static class GeometryHelper
    {
        public static float GetDistanceBetweenPoints(PointF p1, PointF p2)
        {
            var x = p2.X - p1.X;
            var y = p2.Y - p1.Y;

            return MathF.Sqrt(x * x + y * y);
        }

        public static PointF GetRectangleCentre(Rectangle rect)
        {
            var x = rect.X + rect.Width / 2.0f;
            var y = rect.Y + rect.Height / 2.0f;

            return new PointF(x, y);
        }

        public static Point GetRectangleLocationFromCentre(PointF rectCentre, Size size)
        {
            var x = (int)Math.Floor(rectCentre.X - size.Width / 2.0);
            var y = (int)Math.Floor(rectCentre.Y - size.Height / 2.0);

            return new Point(x, y);
        }
    }
}