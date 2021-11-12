using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.PointGenerator;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        private readonly List<RectangleF> tagCloud;
        private readonly PointF center;
        private readonly Spiral spiral;

        public CircularCloudLayouter(PointF center, Spiral spiral)
        {
            tagCloud = new List<RectangleF>();
            this.center = center;
            this.spiral = spiral;
        }


        public RectangleF PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("Size parameters should be positive");
            var tag = GetNextRectangle(rectangleSize);
            tagCloud.Add(tag);
            return tag;
        }


        private RectangleF GetNextRectangle(Size size)
        {
            var points = spiral.GetPoints(center, size);
            var rectangles = points.Select(p => GetRectangleByCenterPoint(p, size));
            var suitableRectangle = rectangles.First(r => !IsIntersectWithCloud(r));
            return suitableRectangle;
        }

        private RectangleF GetRectangleByCenterPoint(PointF rectangleCenter, Size size)
        {
            return new RectangleF(
                new PointF(rectangleCenter.X - size.Width / 2f, rectangleCenter.Y - size.Height / 2f),
                size);
        }

        private bool IsIntersectWithCloud(RectangleF newTag)
        {
            return tagCloud.Any(tag => tag.IntersectsWith(newTag));
        }
    }
}