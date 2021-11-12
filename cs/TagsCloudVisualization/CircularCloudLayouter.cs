using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.Linq;
using TagsCloudVisualization.PointGenerator;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        private readonly List<RectangleF> tagCloud;
        private readonly PointF center;
        private readonly Spiral spiral;
        private Cache cache = new Cache();

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
            cache.UpdateParameter(size, GetDistanceFromTheCenter(suitableRectangle));
            return suitableRectangle;
        }

        private RectangleF GetRectangleByCenterPoint(PointF rectangleCenter, Size size)
        {
            return new RectangleF(
                new PointF(rectangleCenter.X - size.Width / 2f, rectangleCenter.Y - size.Height / 2f),
                size);
        }

        private float GetDistanceFromTheCenter(RectangleF rectangle)
        {
            var rectangleCenterY = (rectangle.Bottom + rectangle.Top) / 2;
            var rectangleCenterX = (rectangle.Left + rectangle.Right) / 2;
            var rectangleCenter = new PointF(rectangleCenterX, rectangleCenterY);
            return center.DistanceTo(rectangleCenter);
        }

        private bool IsIntersectWithCloud(RectangleF newTag)
        {
            return tagCloud.Any(tag => tag.IntersectsWith(newTag));
        }
    }
}