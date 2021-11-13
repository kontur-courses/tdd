using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.PointGenerator;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        private readonly List<RectangleF> tagCloud;
        private readonly IPointGenerator pointGenerator;
        private RectangleF cloudRectangle;
        public RectangleF Unioned => cloudRectangle;
        public SizeF SizeF => new SizeF(cloudRectangle.Width, cloudRectangle.Height);

        public CircularCloudLayouter(IPointGenerator pointGenerator)
        {
            tagCloud = new List<RectangleF>();
            this.pointGenerator = pointGenerator;
        }


        public RectangleF PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("Size parameters should be positive");
            var tag = GetNextRectangle(rectangleSize);
            tagCloud.Add(tag);
            UpdateCloudBorders(tag);
            return tag;
        }

        public RectangleF[] GetCloud()
        {
            return tagCloud.ToArray();
        }


        private void UpdateCloudBorders(RectangleF newTag)
        {
            cloudRectangle = RectangleF.Union(cloudRectangle, newTag);
        }


        private RectangleF GetNextRectangle(Size size)
        {
            var points = pointGenerator.GetPoints(size);
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