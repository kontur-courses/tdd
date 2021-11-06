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
            return Circle.GetPointsFrom(minRadius, center)
                .Select(p => new RectangleF(new PointF(p.X - size.Width / 2, p.Y - size.Height / 2), size))
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