using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.Extensions
{
    public static class RectangleFExtensions
    {
        public static bool IntersectsWithAny(this RectangleF rect, IEnumerable<RectangleF> rectangles)
            => rectangles.Any(r => r.IntersectsWith(rect));

        public static float GetArea(this RectangleF rectangle) => rectangle.Height * rectangle.Width;

        public static PointF GetCenterPoint(this RectangleF rectangle) 
            => new PointF(
                rectangle.X - rectangle.Width / 2,
                rectangle.Y - rectangle.Height / 2);
    }
}