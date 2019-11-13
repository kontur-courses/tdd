using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public const float Thickness = 1;
        public readonly Point Center;
        private readonly IEnumerator<PointF> spiralPoints;
        private readonly List<Rectangle> rectangles;

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            spiralPoints = new ArchimedesSpiral(Thickness, center).GetEnumerator();
            rectangles = new List<Rectangle>();
        }

        public CircularCloudLayouter(Point center, float thickness)
        {
            Center = center;
            spiralPoints = new ArchimedesSpiral(thickness, center).GetEnumerator();
            rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if(rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("Size is empty!");
            var rect = new Rectangle(Center, rectangleSize);
            while (rectangles.Any(x => x.IntersectsWith(rect)))
            {
                spiralPoints.MoveNext();
                rect.X = (int) spiralPoints.Current.X;
                rect.Y = (int) spiralPoints.Current.Y;
            }
            rect.Location = FindBetterDensityPoint(rect);
            rectangles.Add(rect);
            return rect;
        }

        private Point FindBetterDensityPoint(Rectangle rect)
        {
            if (TryFindPreviousSpinPoint(rect.Location, Thickness, out var previousSpinPointF))
            {
                var previousSpinPoint = new Point((int)previousSpinPointF.X, (int)previousSpinPointF.Y);
                foreach (var point in BuildPath(previousSpinPoint, rect.Location))
                {
                    rect.Location = point;
                    if (!rectangles.Any(x => x.IntersectsWith(rect)))
                        break;
                }
            }
            return rect.Location;
        }

        private static bool TryFindPreviousSpinPoint(PointF currentSpinPoint, float a, out PointF previousSpinPoint)
        {
            var (r, theta) =
                PointConverter.TransformCartesianToPolar(currentSpinPoint.X, currentSpinPoint.Y);
            theta -= (float)(2 * Math.PI * a);
            if (theta < 0)
            {
                previousSpinPoint = new PointF(0, 0);
                return false;
            }
            r = theta * a;
            var (x, y) = PointConverter.TransformPolarToCartesian(r, theta);
            previousSpinPoint = new PointF(x, y);
            return true;
        }

        private IEnumerable<Point> BuildPath(Point from, Point to)
        {
            var currentPoint = from;
            while (currentPoint != to)
            {
                if (currentPoint.X != to.X)
                {
                    currentPoint.X -= Math.Sign(currentPoint.X - to.X);
                    yield return currentPoint;
                }
                if (currentPoint.Y != to.Y)
                {
                    currentPoint.Y -= Math.Sign(currentPoint.X - to.Y);
                    yield return currentPoint;
                }
            }
        }

        public static List<Rectangle> CreateRandomLayout(Point center,
            float thickness, int minRectSize, int maxRectSize, int amountOfRectangles)
        {
            var layouter = new CircularCloudLayouter(center, thickness);
            var random = new Random();
            for (var i = 0; i < amountOfRectangles; i++)
            {
                var randomValue1 = random.Next(minRectSize, maxRectSize + 1);
                var randomValue2 = random.Next(minRectSize, maxRectSize + 1);
                var randomSize = new Size(Math.Max(randomValue1, randomValue2), Math.Min(randomValue1, randomValue2));
                layouter.PutNextRectangle(randomSize);
            }
            return layouter.rectangles;
        }
    }
}