using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.PointGenerator;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        private readonly List<RectangleF> tagCloud = new List<RectangleF>();
        private readonly PointF center;
        private IPointGenerator generator = new Spiral();

        public CircularCloudLayouter(PointF center)
        {
            this.center = center;
        }

        public CircularCloudLayouter WithPointGenerator(IPointGenerator pointGenerator)
        {
            this.generator = pointGenerator;
            return this;
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
            return GetCloud().Aggregate(new RectangleF(), RectangleF.Union);
        }

        private PointF GetNextPosition(Size size)
        {
            return generator.GetPoints(center)
                .Select(p => new RectangleF(new PointF(p.X - size.Width / 2f, p.Y - size.Height / 2f), size))
                .First(r => !IsIntersectWithCloud(r)).Location;
        }

        private bool IsIntersectWithCloud(RectangleF newTag)
        {
            return tagCloud.Any(tag => tag.IntersectsWith(newTag));
        }
    }
}