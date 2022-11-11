using System.Drawing;

namespace TagsCloudVisualization.Core
{
    public class ArchimedeanSpiral
    {
        private readonly Point _center;
        private readonly HashSet<Point> _oldPoints = new ();

        public ArchimedeanSpiral(Point center)
        {
            _center = center;
        }

        public Point GetNextPoint()
        {
            float newAngle = 0;

            while (true)
            {
                var rad = 2 * newAngle;

                var x = (_center.X + rad * Math.Cos(newAngle));
                var y = (_center.Y + rad * Math.Sin(newAngle));

                var point = new Point((int)x, (int)y);

                if (!_oldPoints.Contains(point))
                {
                    _oldPoints.Add(point);
                    return point;
                }

                newAngle += (float)(5 * Math.PI / 180);
            }
        }

    }
}
