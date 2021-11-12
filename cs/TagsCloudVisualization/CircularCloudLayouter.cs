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
        private readonly Spiral spiral;
        private double squareRectangles;
        public readonly List<Rectangle> Rectangles;
        public double SquareRectangles => squareRectangles;

        public CircularCloudLayouter(Point center)
        {
            centrPoint = center;
            nextPoint = centrPoint;
            Rectangles = new List<Rectangle>();
            spiral = new Spiral(centrPoint);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            {
                throw new ArgumentException(
                    $"Не корректные размеры прямоугольника. Высота: {rectangleSize.Height} ," +
                    $" Ширина: {rectangleSize.Width}");
            }

            var currentRectangle = new Rectangle(nextPoint, rectangleSize);
            while (IsIntersects(currentRectangle))
            {
                nextPoint = spiral.CalculatePointSpiral();
                currentRectangle.Location = nextPoint;
            }

            Rectangles.Add(CorrectLocationRectangle(currentRectangle));

            squareRectangles += currentRectangle.Square();

            return currentRectangle;
        }

        private bool IsIntersects(Rectangle rectangle)
        {
            return Rectangles.Any(rect => rect.IntersectsWith(rectangle));
        }

        public double Radius()
        {
            if (Rectangles == null)
                throw new Exception();

            var lastPoint = Rectangles.Last().Location; // последнняя точка самая дальняя от центра

            return Math.Sqrt(Math.Pow(lastPoint.X - centrPoint.X, 2) + Math.Pow(lastPoint.Y - centrPoint.Y, 2));
        }

        private Rectangle CorrectLocationRectangle(Rectangle rectangle)
        {
            rectangle = CorrectCoordXRectangle(rectangle);
            rectangle = CorrectCoordYRectangle(rectangle);
            return rectangle;
        }

        private Rectangle CorrectCoordXRectangle(Rectangle rectangle)
        {
            var oldPoint = rectangle.Location;
            var newPoint = rectangle.Location;

            if (oldPoint.X > centrPoint.X)
            {
                while (!IsIntersects(rectangle) && newPoint.X != centrPoint.X)
                {
                    newPoint.X--;

                    rectangle.Location = newPoint;
                }

                newPoint.X++;
                rectangle.Location = newPoint;
            }
            else if (oldPoint.X < centrPoint.X || newPoint.X != centrPoint.X)
            {
                while (!IsIntersects(rectangle) && newPoint.X != centrPoint.X)
                {
                    newPoint.X++;

                    rectangle.Location = newPoint;
                }

                newPoint.X--;
                rectangle.Location = newPoint;
            }

            return rectangle;
        }

        private Rectangle CorrectCoordYRectangle(Rectangle rectangle)
        {
            var oldPoint = rectangle.Location;
            var newPoint = rectangle.Location;

            if (oldPoint.Y > centrPoint.Y)
            {
                while (!IsIntersects(rectangle) && newPoint.Y != centrPoint.Y)
                {
                    newPoint.Y--;

                    rectangle.Location = newPoint;
                }

                newPoint.Y++;
                rectangle.Location = newPoint;
            }
            else if (oldPoint.Y < centrPoint.Y)
            {
                while (!IsIntersects(rectangle) && newPoint.Y != centrPoint.Y)
                {
                    newPoint.Y++;

                    rectangle.Location = newPoint;
                }

                newPoint.Y--;
                rectangle.Location = newPoint;
            }

            return rectangle;
        }
    }
}