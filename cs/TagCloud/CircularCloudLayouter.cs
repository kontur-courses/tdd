using System.Drawing;
using System.Numerics;

namespace TagCloud
{
    public class CircularCloudLayouter : ITagCloudLayouter
    {
        private int i;
        private List<RectangleF> rectangles = new List<RectangleF>();
        private PointF center;
        private RectangleF cur;
        private int max = 2;
        private List<Vector2> dirs;

        public Rectangle[] Rectangles => rectangles.Select(Rectangle.Truncate).ToArray();

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            cur = new RectangleF(
                center.X,
                center.Y,
                rectangleSize.Width,
                rectangleSize.Height);

            MoveOutOfcenter(ref cur);

            MoveToCenter(ref cur);

            rectangles.Add(cur);

            return Rectangle.Truncate(cur);
        }

        private void MoveOutOfcenter(ref RectangleF rect)
        {
            var dir = GetDir();

            while (IntersectsWithAny(cur))
                cur.Offset(dir.X, dir.Y);
        }

        private void MoveToCenter(ref RectangleF rect)
        {
            bool movedDx = true, movedDy = true;
            while (movedDx || movedDy)
            {
                var dx = (int)(center.X - rect.X);
                var dy = (int)(center.Y - rect.Y);
                dx /= dx != 0 ? Math.Abs(dx) : 1;
                dy /= dy != 0 ? Math.Abs(dy) : 1;

                movedDx = dx == 0 ? false : OffsetIfDontCollide(ref rect, dx, 0);
                movedDy = dy == 0 ? false : OffsetIfDontCollide(ref rect, 0, dy);
            }
        }

        private bool OffsetIfDontCollide(ref RectangleF rect, float x, float y)
        {
            rect.Offset(x, y);

            if (IntersectsWithAny(rect))
            {
                rect.Offset(-x, -y);
                return false;
            }

            return true;
        }

        private bool IntersectsWithAny(RectangleF rect)
        {
            rect.X = (int)rect.X;
            rect.Y = (int)rect.Y;
            return rectangles.Any(r => rect.IntersectsWith(r));
        }

        private Vector2 GetDir()
        {
            if (i % 100 == 0)
            {
                max++;
                UpdateDirs();
            }

            return dirs[i++ % dirs.Count];
        }

        private void UpdateDirs()
        {
            var dirs = new List<Vector2>();

            var step = 90f / max;

            var muls = new[] { -1, 1, 1, -1, -1 };

            for (var i = step; i <= 90 - step; i += step)
            {
                var rad = i * Math.PI / 180;
                var tan = Math.Tan(rad);
                var x = 1f;
                var y = (float)(tan * x);

                for (var j = 0; j < muls.Length - 1; j++)
                {
                    var vector = new Vector2(x * muls[j], y * muls[j + 1]);
                    vector /= vector.Length();
                    dirs.Add(vector);
                }
            }

            this.dirs = dirs;
        }
    }
}
