using System.Drawing;
using System.Runtime.ConstrainedExecution;

namespace TagCloud
{
    public static class PointExtensions
    {
        public static float DistanceTo(this PointF p1, PointF p2)
        {
            return (float)Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }
    }
}
