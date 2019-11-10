using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly List<Rectangle> arrangedRectangles = new List<Rectangle>();
        private readonly IEnumerator<Point> spiralEnumerator;

        public CircularCloudLayouter(Point center, ArchimedesSpiral spiral = null)
        {
            if (center.X <= 0 || center.Y <= 0)
                throw new ArgumentException("Tags Cloud center coordinates should be positive.");
            spiralEnumerator = spiral == null
                ? new ArchimedesSpiral(center, 0.5f, 0.5f).GetEnumerator()
                : spiral.GetEnumerator();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("Tags Cloud rectangle size parameters should be positive.");
            var tempRect = new Rectangle(spiralEnumerator.Current, rectangleSize);
            while (arrangedRectangles.Any(r => r.IntersectsWith(tempRect)) && spiralEnumerator.MoveNext())
                tempRect.Location = spiralEnumerator.Current;
            arrangedRectangles.Add(tempRect);
            return tempRect;
        }
    }
}