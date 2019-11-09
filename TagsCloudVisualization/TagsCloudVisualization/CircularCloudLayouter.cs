using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public Point Center => spiral.Center;

        private readonly Spiral spiral;

        private readonly List<Rectangle> rectangleMap;

        public CircularCloudLayouter(Point center)
        {
            rectangleMap = new List<Rectangle>();
            spiral = new Spiral(center);
        }

        public IEnumerable<Rectangle> GetRectangles()
        {
            return rectangleMap;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException();
            var rectangle = CreateRectangleOnSpiral(rectangleSize);
            rectangleMap.Add(rectangle);
            return rectangle;
        }

        private Rectangle CreateRectangleOnSpiral(Size rectangleSize)
        {
            var shiftedCenter = Geometry.ShiftPointBySizeOffsets(spiral.Center, rectangleSize);
            var rectangle = new Rectangle(shiftedCenter, rectangleSize);
            while (rectangleMap.Any(r => r.IntersectsWith(rectangle)))
            {
                var spiralPoint = spiral.GetNextSpiralPoint();
                rectangle.X = shiftedCenter.X + spiralPoint.X;
                rectangle.Y = shiftedCenter.Y + spiralPoint.Y;
            }
            spiral.AlignDirection(rectangle);
            return rectangle;
        }
    }

    internal class Spiral
    {
        public readonly Point Center;

        private const double spiralRatio = 0.1;

        private double angle;

        private double distanceFromCenter;

        public Spiral(Point center)
        {
            Center = center;
        }

        public Point GetNextSpiralPoint()
        {
            angle += spiralRatio;
            distanceFromCenter += spiralRatio;
            return Geometry.PolarToCartesian(distanceFromCenter, angle);
        }

        public void AlignDirection(Rectangle rectangle)
        {
            distanceFromCenter -= Math.Max(distanceFromCenter / 2,
                Geometry.GetLengthFromRectCenterToBorderOnVector(rectangle, Center));
        }
    }
}