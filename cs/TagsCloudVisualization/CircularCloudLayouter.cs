using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        private readonly List<RectangleF> tagCloud = new List<RectangleF>();
        private readonly PointF center;
        private int minRadius = 0;

        public CircularCloudLayouter(PointF center)
        {
            this.center = center;
        }

        public RectangleF PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("Size parameters should be positive");
            var tag = new RectangleF(GetNextPosition(rectangleSize), rectangleSize);
            tagCloud.Add(tag);
            return tag;
        }

        public IEnumerable<RectangleF> GetCloud()
        {
            return tagCloud;
        }

        public RectangleF GetBounds()
        {
            return GetCloud().Aggregate(new RectangleF(),
                (current, tag) => RectangleF.Union(current, tag));
        }

        private PointF GetNextPosition(Size newRectangleSize)
        {
            return GetNextNearestPositionForTag(newRectangleSize);
        }

        private PointF GetNextNearestPositionForTag(Size size)
        {
            var sp = new Spiral();
            return sp.GetPointsIn(center,new Size(1,1))
                .Select(p => new RectangleF(new PointF(p.X - size.Width / 2f, p.Y - size.Height / 2f), size))
                .First(r => !IsIntersectWithCloud(r)).Location;
        }

        public bool IsIntersectWithCloud(RectangleF newTag)
        {
            foreach (var tag in tagCloud)
                if (tag.IntersectsWith(newTag))
                    return true;
            return false;
        }
    }
}