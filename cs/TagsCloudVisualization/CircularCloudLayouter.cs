using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly Point center;
        private readonly IPointsGenerator pointsGenerator;

        public CircularCloudLayouter(Point center, IPointsGenerator pointsGenerator)
        {
            this.center = center;
            this.pointsGenerator = pointsGenerator;
        }

        public List<Rectangle> Rectangles { get; } = new List<Rectangle>();

        public void PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Rectangle width and height should be positive numbers");
            Rectangle newRectangle;
            while (true)
            {
                var nextPoint = pointsGenerator.GetNextPoint();
                var location = center + (Size)nextPoint;
                newRectangle = new Rectangle(location, rectangleSize).MoveToHavePointInCenter(location);
                if (Rectangles.All(rectangle => !rectangle.IntersectsWith(newRectangle)))
                    break;
            }
            Rectangles.Add(newRectangle);
        }
    }
}