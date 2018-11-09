using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TagsCloudVisualization
{
    struct SpiralInfo
    {
        public readonly double RadiusStep;
        public readonly double AngleStep;
        public readonly Point Center;
        public const int MaxAngle = 360;

        public SpiralInfo(double radiusStep, double angleStep, Point center)
        {
            RadiusStep = radiusStep;
            AngleStep = angleStep;
            Center = center;
        }
    }

    class CircularCloudLayouter
    {
        public readonly HashSet<Rectangle> Rectangles;
        private readonly SpiralInfo spiralInfo;

        public CircularCloudLayouter(Point center, double radiusStep = 0.0001, double angleStep = 0.1)
        {
            Rectangles = new HashSet<Rectangle>();
            spiralInfo = new SpiralInfo(radiusStep, angleStep, center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Width and height should be integer positive numbers");

            var newRectangle = new Rectangle(GetAppropriatePlace(rectangleSize), rectangleSize);
            Rectangles.Add(newRectangle);
            return newRectangle;
        }

        public Point GetAppropriatePlace(Size rectangleSize)
        {
            if (Rectangles.Count == 0)
                return spiralInfo.Center;

            var currentPosition = spiralInfo.Center;
            var currentAngle = 0.0;
            var currentRadius = 0.0;
            var potentialRectangle = new Rectangle(currentPosition, rectangleSize);
            while (HaveIntersectionWithAnotherRectangle(potentialRectangle) ||
                   IsPlacedInAnotherRectangle(potentialRectangle))
            {
                currentPosition = GetNextPosition(
                    currentPosition, ref currentAngle, ref currentRadius, rectangleSize);
                if (!potentialRectangle.Location.Equals(currentPosition))
                    potentialRectangle = new Rectangle(currentPosition, rectangleSize);
            }
            return currentPosition;
        }

        private Point GetNextPosition(
            Point currentPosition, ref double currentAngle, ref double currentRadius, Size rectangleSize)
        {
            var sin = Math.Sin(currentAngle);
            var cos = Math.Cos(currentAngle);
            currentPosition.X = (int)(spiralInfo.Center.X - currentRadius * currentAngle * sin);
            currentPosition.Y = (int)(spiralInfo.Center.Y + currentRadius * currentAngle * cos);
            currentAngle = (currentAngle + spiralInfo.AngleStep) % SpiralInfo.MaxAngle;
            currentRadius += spiralInfo.RadiusStep;

            return currentPosition;
        }

        private bool HaveIntersectionWithAnotherRectangle(Rectangle rectangle)
        {
            return Rectangles.Count(anotherRectangle => HaveIntersection(rectangle, anotherRectangle)) > 0;
        }

        private bool IsPlacedInAnotherRectangle(Rectangle verifiableRectangle)
        {
            return Rectangles.Count(rectangle => IsNestedRectangle(verifiableRectangle, rectangle)) > 0;
        }

        public bool HaveIntersection(Rectangle first, Rectangle second)
        {
            return first.Left <= second.Right && first.Right >= second.Left && first.Top <= second.Bottom &&
                   first.Bottom >= second.Top &&
                   !IsNestedRectangle(first, second) && !IsNestedRectangle(second, first);
        }

        private bool IsPointInRectangle(Point point, Rectangle rectangle)
        {
            return point.X > rectangle.Left && point.X < rectangle.Right &&
                   point.Y > rectangle.Top && point.Y < rectangle.Bottom;
        }

        public bool IsNestedRectangle(Rectangle first, Rectangle second)
        {
            return IsPointInRectangle(new Point(first.Left, first.Top), second) &&
                   IsPointInRectangle(new Point(first.Right, first.Bottom), second);
        }
    }
}
