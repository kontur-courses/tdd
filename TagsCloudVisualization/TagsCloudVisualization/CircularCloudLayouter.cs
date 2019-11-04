using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly Point Center;

        private const double spiralRatio = 0.1;

        private double angle;
        private double length;

        private readonly List<Rectangle> tagRectangles;

        public CircularCloudLayouter(Point center)
        {
            tagRectangles = new List<Rectangle>();
            Center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0) throw new ArgumentException();
            var rect = CreateRectangleOnSpiral(rectangleSize);
            tagRectangles.Add(rect);
            return rect;
        }

        private Rectangle CreateRectangleOnSpiral(Size rectangleSize)
        {
            var shiftedCenter = Geometry.ShiftPointBySizeOffsets(Center, rectangleSize);
            var rect = new Rectangle(shiftedCenter, rectangleSize);

            while (tagRectangles.Any(r => r.IntersectsWith(rect)))
            {
                angle += spiralRatio;
                length += spiralRatio;
                var spiralPoint = Geometry.PolarToCartesion(length, angle);
                rect.X = shiftedCenter.X + spiralPoint.X;
                rect.Y = shiftedCenter.Y + spiralPoint.Y;
            }

            length -= Math.Max(length / 2, Geometry.GetLengthFromRectCenterToBorder(rect, Center));
            return rect;
        }

        public IEnumerable<Rectangle> GetRectangles() => tagRectangles;
    }
}