using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private IEnumerator<Point> spiralEnumerator;
        private readonly List<Rectangle> arrangedRectangles = new List<Rectangle>();

        public CircularCloudLayouter(Point center)
        {
            if (center.X <= 0 || center.Y <= 0)
                throw new ArgumentException("Tags Cloud center coordinates should be positive.");
            spiralEnumerator = new ArchimedesSpiral(center, 1, 1).GetEnumerator();
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