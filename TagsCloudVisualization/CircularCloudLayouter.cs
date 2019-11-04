using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;

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

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if(rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("Size is empty!");
            var rect = new Rectangle(Center, rectangleSize);
            while (rectangles.Any(x => x.IntersectsWith(rect)))
            {
                rect.X = (int) spiralPoints.Current.X;
                rect.Y = (int) spiralPoints.Current.Y;
                spiralPoints.MoveNext();
            }
            if (ArchimedesSpiral.TryFindPreviousSpinPoint(rect.Location, Thickness, out var previousSpinPointF))
            {
                var previousSpinPoint = new Point((int) previousSpinPointF.X, (int) previousSpinPointF.Y);
                foreach (var point in BuildPath(previousSpinPoint, rect.Location))
                {
                    rect.Location = point;
                    if (!rectangles.Any(x => x.IntersectsWith(rect)))
                        break;
                }
            }
            rectangles.Add(rect);
            return rect;
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
    }
}