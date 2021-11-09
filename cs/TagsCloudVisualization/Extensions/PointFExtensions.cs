using System.Drawing;
using System.Numerics;

namespace TagsCloudVisualization
{
    public static class PointFExtensions
    {
        public static Vector2 ToVector(this PointF p)
            => new Vector2(p.X, p.Y);
    }
}
