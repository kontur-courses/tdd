using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.PointGenerator;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        private  List<RectangleF> tagCloud;
        private  PointF center;
        private  IPointGenerator generator;

        public CircularCloudLayouter(PointF center, IPointGenerator pointGenerator)
        {
            tagCloud = new List<RectangleF>();
            this.center = center;
            generator = pointGenerator;
        }


        public RectangleF PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("Size parameters should be positive");
            var tag = new RectangleF(GetNextPosition(rectangleSize), rectangleSize);
            tagCloud.Add(tag);
            return tag;
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