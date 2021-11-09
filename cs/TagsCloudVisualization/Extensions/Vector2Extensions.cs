using System.Drawing;
using System.Numerics;

namespace TagsCloudVisualization
{
    public static class Vector2Extensions
    {
        public static PointF ToPoint(this Vector2 v) 
            => new PointF(v.X, v.Y);

        public static double GetDistanceTo(this Vector2 v, PointF p) 
            => (p.ToVector() - v).Length();
    }
}