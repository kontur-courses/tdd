using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private Point centrPoint;
        private Point nextPoint;
        private List<Rectangle> rectangles;
        private readonly Spiral spiral;
        public List<Rectangle> Rectangles => rectangles;

        public CircularCloudLayouter(Point center)
        {
            centrPoint = center;
            nextPoint = centrPoint;
            rectangles = new List<Rectangle>();
            spiral = new Spiral(centrPoint);
        }


        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            {
                throw new ArgumentException(
                    $"Не корректные размера прямоугольника. Высота: {rectangleSize.Height} ," +
                    $" Ширина: {rectangleSize.Width}");
            }

            if (rectangleSize.Width < 0 || rectangleSize.Width < 0)
            {
                throw new ArgumentException(
                    $"Не корректные размера прямоугольника. Высота: {rectangleSize.Height} ," +
                    $" Ширина: {rectangleSize.Width}");
            }

            Rectangle currentRectangle;
            if (rectangles.Count == 0)
            {
                currentRectangle = new Rectangle(centrPoint, rectangleSize);
                rectangles.Add(currentRectangle);

                return currentRectangle;
            }

            currentRectangle = new Rectangle(nextPoint, rectangleSize);
            while (IsIntersects(currentRectangle))
            {
                nextPoint = spiral.CalcPointSpiral();
                currentRectangle.Location = nextPoint;
            }

            rectangles.Add(currentRectangle);

            return currentRectangle;
        }

        public bool IsIntersects(Rectangle rectangle)
        {
            return rectangles.Any(rect => rect.IntersectsWith(rectangle));
        }
    }
}