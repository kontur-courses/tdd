using System;
using System.Drawing;
using System.Numerics;

namespace TagsCloudVisualization
{
    public static class RectangleFExtensions
    {
        public static PointF GetCenter(this RectangleF rect)
            => new PointF(rect.X + rect.Width / 2f,
                rect.Y + rect.Height / 2f);

        public static RectangleF GetRectangleByCenter(SizeF sz, PointF point)
            => new RectangleF(new PointF
                (point.X - sz.Width / 2f,
                point.Y - sz.Height / 2f),
                sz);
    }

    public static class Vector2Extensions
    {
        public static PointF ToPoint(this Vector2 v)
            => new PointF(v.X, v.Y);



        public static double GetDistanseTo(this Vector2 v, PointF point)
            => (point.ToVector() - v).Length();
    }

    public static class PointFExtensions
    {
        public static Vector2 ToVector(this PointF p)
            => new Vector2(p.X, p.Y);
    }
}
