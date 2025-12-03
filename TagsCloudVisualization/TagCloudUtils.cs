using System;
using System.Drawing;
using System.Linq;


namespace TagCloud
{
    internal static class TagCloudUtils
    {
        public static double Distance(Point a, Point b)
        {
            var dx = a.X - b.X;
            var dy = a.Y - b.Y;
            return Math.Sqrt(dx * dx + dy * dy);

        }
        public static double Distance(double ax, double ay, double bx, double by)
        {
            var dx = ax - bx;
            var dy = ay - by;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static double DistanceToCenter(Rectangle rect, Point center)
        {
            double cx = rect.Left + rect.Width / 2.0;
            double cy = rect.Top + rect.Height / 2.0;

            return Distance(cx, cy, center.X, center.Y);
        }

        public static bool IntersectsAny(Rectangle rect, IEnumerable<Rectangle> others)
        {
            return others.Any(r => r.IntersectsWith(rect));
        }

        
        public static Rectangle MoveHorizontalTowardsCenter(Rectangle rect, Point center)
        {
            var centerRect = new Point(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);
            int dx = Math.Sign(center.X - centerRect.X);
            //rect.Offset(dx, 0);
            return new Rectangle(rect.Left + dx, rect.Top, rect.Width, rect.Height);

        }
        public static Rectangle MoveVerticalTowardsCenter(Rectangle rect, Point center)
        {
            var centerRect = new Point(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);
            int dy = Math.Sign(center.Y - centerRect.Y);
            //rect.Offset(0, dy);
            return new Rectangle(rect.Left, rect.Top + dy, rect.Width, rect.Height);
        }
    }
}