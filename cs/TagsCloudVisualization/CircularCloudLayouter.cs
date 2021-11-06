using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        private readonly List<Rectangle> tagCloud = new List<Rectangle>();
        private readonly Point center;
        private int minRadius = 0;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var tag = new Rectangle(GetNextPosition(rectangleSize), rectangleSize);
            tagCloud.Add(tag);
            return tag;
        }

        public IEnumerable<Rectangle> GetCloud()
        {
            return tagCloud;
        }

        private Point GetNextPosition(Size newRectangleSize)
        {
            return GetNextNearestPositionForTag(newRectangleSize);
        }

        private Point GetNextNearestPositionForTag(Size size)
        {
            return Circle.GetPointsFrom(minRadius, center)
                .Select(p => new Rectangle(new Point(p.X - size.Width / 2, p.Y - size.Height / 2), size))
                .First(r => !IsIntersectWithCloud(r)).Location;
        }

        private bool IsIntersectWithCloud(Rectangle newTag)
        {
            foreach (var tag in tagCloud)
                if (tag.IntersectsWith(newTag))
                    return true;
            return false;
        }
    }
}