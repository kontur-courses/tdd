using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        private readonly List<Rectangle> tagCloud = new();
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
            return GetPointsFrom(minRadius)
                .Select(p => new Rectangle(p-size/2, size))
                .First(r => !IsIntersectWithCloud(r)).Location;
        }

        private bool IsIntersectWithCloud(Rectangle newTag)
        {
            foreach (var tag in tagCloud)
                if (tag.IntersectsWith(newTag))
                    return true;
            return false;

        }

        private IEnumerable<Point> GetPointsFrom(int radius)
        {
            while (true)
            {
                foreach (var p in GetPointsInRadius(radius++))
                    yield return p;
            }
        }

        private IEnumerable<Point> GetPointsInRadius(int radius)
        {
            return GetPointsInRadiusInZeroCenter(radius)
                .Select(point => new Point(point.X + center.X, point.Y + center.Y));
        }

        private IEnumerable<Point> GetPointsInRadiusInZeroCenter(int radius)
        {
            yield return new Point(-radius, 0);
            for (var x = -radius + 1; x < radius; x++)
            {
                yield return new Point(x, (int)Math.Sqrt(radius*radius - x*x));
                yield return new Point(x, -(int)Math.Sqrt(radius*radius - x*x));
            }
            yield return new Point(radius, 0);
        }
    }
}