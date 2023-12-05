using System.Drawing;
using System.Numerics;

namespace TagCloud
{
    public class TagCloudForDebug
    {
        private int i;
        private List<RectangleF> rectangles = new List<RectangleF>();
        private PointF center;
        private RectangleF cur;
        private int max = 2;
        private List<Vector2> dirs;
        private float maxDistFromCenter;

        public TagCloudForDebug(Point center)
        {
            this.center = center;
        }

        public IEnumerable<Bitmap> PutNextRectangle(Size rectangleSize)
        {
            cur = new RectangleF(
                center.X,
                center.Y,
                rectangleSize.Width,
                rectangleSize.Height);

            var dir = GetDir();

            while (IntersectsWithAny(cur))
            {
                cur.Offset(dir.X, dir.Y);
                yield return GetLayout();
            }

            foreach (var el in MoveToCenter(cur))
            {
                cur.X = el.Item1;
                cur.Y = el.Item2;
                yield return GetLayout();
            }

            yield return GetLayout();
            if (maxDistFromCenter < center.DistanceTo(new PointF(cur.X, cur.Y)))
                maxDistFromCenter = center.DistanceTo(new PointF(cur.X, cur.Y));
            rectangles.Add(cur);
        }

        private IEnumerable<(float, float)> MoveToCenter(RectangleF rect)
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

                yield return (rect.X, rect.Y);
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

            for (var i = step; i < 90; i += step)
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

        public Bitmap GetLayout()
        {
            var c1 = Color.FromArgb(255, 90, 137, 185);
            var c2 = Color.FromArgb(255, 10, 28, 52);
            var c3 = Color.FromArgb(255, 193, 217, 249);

            var bmp = new Bitmap(1600, 900);
            var g = Graphics.FromImage(bmp);
            var pen = new Pen(c2, 5);
            var brush = new SolidBrush(c3);

            g.Clear(c1);

            foreach (var rect in rectangles)
            {
                g.FillRectangle(brush, rect);
                g.DrawRectangle(pen, rect);
            }

            g.DrawRectangle(pen, cur);
            g.FillRectangle(Brushes.Red, cur);

            foreach (var dir in dirs)
            {
                g.FillRectangle(Brushes.Red ,new RectangleF(dir.X * 200 + center.X
                    , dir.Y * 200 + center.Y, 5, 5));
            }

            return bmp;
        }
    }
}
