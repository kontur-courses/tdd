using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICircularCloudLayouter
    {
        public Point Center { get; }

        private List<Rectangle> layoutedRectangles;
        private Spiral spiral;

        public CircularCloudLayouter()
        {
            Center = new Point(0, 0);
            spiral = new Spiral(Center, 4);
            layoutedRectangles = new List<Rectangle>();
        }

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            spiral = new Spiral(Center, 4);
            layoutedRectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            {
                throw new ArgumentException($"rectangleSize must have only positive values: {rectangleSize}");
            }

            var newRectangle = GenerateRectangle(rectangleSize);
            layoutedRectangles.Add(newRectangle);

            return newRectangle;
        }

        private Rectangle GenerateRectangle(Size rectangleSize)
        {
            var rectangle = new Rectangle();

            foreach (var spiralPoint in spiral.GetSpiralPoints())
            {
                rectangle = GetRectangleByCenterPoint(spiralPoint, rectangleSize);

                if (!RectangleIsIntersectOthers(rectangle))
                {
                    break;
                }
            }

            return rectangle;
        }

        private bool RectangleIsIntersectOthers(Rectangle rectangle)
        {
            return layoutedRectangles.Any(rectangle.IntersectsWith);
        }

        private Rectangle GetRectangleByCenterPoint(Point centerPoint, Size rectangleSize)
        {
            var northWestCornerLocation = new Point(centerPoint.X - rectangleSize.Width / 2,
                centerPoint.Y - rectangleSize.Height / 2);

            return new Rectangle(northWestCornerLocation, rectangleSize);
        }
    }
}