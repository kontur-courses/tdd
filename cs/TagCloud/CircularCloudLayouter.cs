using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloud
{
    public class CircularCloudLayouter
    {
        private readonly ArchimedeanSpiral _spiral;

        public Point CloudCenter { get; }

        public List<Rectangle> Rectangles { get; }

        public CircularCloudLayouter(Point cloudCenter)
        {
            CloudCenter = cloudCenter;

            Rectangles = new List<Rectangle>();

            _spiral = new ArchimedeanSpiral(cloudCenter);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            VerifyRectangleSize(rectangleSize);

            Rectangle rectangle;

            do
            {
                Point point = _spiral.GetNextPoint();

                rectangle = new Rectangle(new Point(point.X - rectangleSize.Width / 2, point.Y - rectangleSize.Height / 2), rectangleSize);

            } while (IsIntersectWithAnyRectangle(rectangle));

            Rectangles.Add(ShiftRectangleToCenterPoint(rectangle));

            return rectangle;
        }

        private static void VerifyRectangleSize(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("width and height of rectangle must be more than zero");
        }

        private bool IsIntersectWithAnyRectangle(Rectangle rectangle)
        {
            return Rectangles?.Any(r => r.IntersectsWith(rectangle)) ?? false;
        }

        private Rectangle ShiftRectangleToCenterPoint(Rectangle rectangle)
        {
            if (IsСoincidesWithCloudCenter(rectangle))
                return rectangle;

            Vector[] shiftAxialOrts = GetShiftAxialOrts(rectangle);

            while (!shiftAxialOrts[0].IsZeroVector() || !shiftAxialOrts[1].IsZeroVector())
            {
                for (int i = 0; i < shiftAxialOrts.Length; i++)
                {
                    if (shiftAxialOrts[i].IsZeroVector())
                        continue;

                    if (!TryMoveRectangleAlongVector(rectangle, shiftAxialOrts[i], out Rectangle movedRectangle))
                    {
                        shiftAxialOrts[i].SetToZeroVector();
                        continue;
                    }

                    rectangle = movedRectangle;

                    int[] axleDistances = GetAxialDistancesBetweenPoints(CloudCenter, rectangle.GetCenter());

                    if (axleDistances[i] == 0)
                        shiftAxialOrts[i].SetToZeroVector();
                }
            }

            return rectangle;
        }

        private int[] GetAxialDistancesBetweenPoints(Point pointA, Point pointB)
        {
            return new int[] { pointA.X - pointB.X, pointA.Y - pointB.Y };
        }

        private bool IsСoincidesWithCloudCenter(Rectangle rectangle)
        {
            int[] axleDistances = GetAxialDistancesBetweenPoints(CloudCenter, rectangle.GetCenter());

            return axleDistances[0] == 0 && axleDistances[1] == 0;
        }

        private Vector[] GetShiftAxialOrts(Rectangle rectangle)
        {
            int[] axleDistances = GetAxialDistancesBetweenPoints(CloudCenter, rectangle.GetCenter());

            return new Vector[]
            {
                new Vector(axleDistances[0] == 0 ? 0 : axleDistances[0] / Math.Abs(axleDistances[0]), 0),
                new Vector(0, axleDistances[1] == 0 ? 0 : axleDistances[1] / Math.Abs(axleDistances[1]))
            };
        }

        private bool TryMoveRectangleAlongVector(Rectangle rectangle, Vector vector, out Rectangle movedRectangle)
        {
            movedRectangle = rectangle.MoveAlongVector(vector);

            if (IsIntersectWithAnyRectangle(movedRectangle))
                return false;

            return true;
        }

        public void PutRectangles(List<Size> sizes)
        {
            foreach (var size in sizes)
                this.PutNextRectangle(size);
        }
    }
}
