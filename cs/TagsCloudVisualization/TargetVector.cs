using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TagsCloudVisualization
{
    public class TargetVector
    {
        Point target;
        Point location;
        public TargetVector(Point target, Point location)
        {
            this.target = target;
            this.location = location;
        }

        public IEnumerable<Point> GetPartialDelta()
        {
            while (target != location)
            {
                var delta = GetDelta();
                yield return new Point(delta.X, 0);
                yield return new Point(0, delta.Y);
                location.Offset(delta.X, delta.Y);
            }
        }

        private Point GetDelta()
        {
            var dx = target.X - location.X;
            var dy = target.Y - location.Y;
            return new Point(GetOffsetPerCoodinate(dx), GetOffsetPerCoodinate(dy));
        }
        private int GetOffsetPerCoodinate(int coordinate)
        {
            if (coordinate > 0)
                return 1;
            else if (coordinate < 0)
                return -1;
            else
                return 0;
        }
    }
}
