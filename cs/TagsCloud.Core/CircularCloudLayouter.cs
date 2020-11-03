using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TagsCloud.Tests")]
[assembly: InternalsVisibleTo("TagsCloud.Visualization")]

namespace TagsCloud.Core
{
    internal class CircularCloudLayouter : ICircularCloudLayouter
    {
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        private readonly ArchimedeanSpiral spiral;

        public CircularCloudLayouter(Point center)
        {
            spiral = new ArchimedeanSpiral(center, 0.005);
        }

        public IReadOnlyCollection<Rectangle> Rectangles => rectangles;

        public Rectangle PutNextRectangle(Size rectSize)
        {
            if (rectSize.Width <= 0 || rectSize.Height <= 0)
                throw new ArgumentException("rectangle's width and height must be positive numbers");
            var rect = GetNextRectangle(rectSize);
            rectangles.Add(rect);
            return rect;
        }

        private Rectangle GetNextRectangle(Size rectSize)
        {
            while (true)
            {
                var rect = new Rectangle(spiral.GetNextPoint() - new Size(rectSize.Width / 2, rectSize.Height / 2),
                    rectSize);
                if (!rect.IntersectsWith(rectangles))
                    return rect;
            }
        }
    }
}