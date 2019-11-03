using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        readonly Point center;
        readonly List<IFigure> figures;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            figures = new List<IFigure>();
        }

        IEnumerator<Point> GetPointsOnCircular()
        {
            var maxAngle = 2 * Math.PI;
            var radius = 0;
            while (true)
            {
                var step = Math.PI / Math.Pow(radius + 1, 0.7);
                for (var angle = 0.0; angle < maxAngle; angle += step)
                {
                    var x = (int)(radius * Math.Cos(angle));
                    var y = (int)(radius * Math.Sin(angle));

                    yield return new Point(x, y);
                }
                radius++;
            }
        }

        IEnumerator<Point> pointsOnCircular = null;
        Point FindNextFreePoint()
        {
            if (pointsOnCircular == null)
                pointsOnCircular = GetPointsOnCircular();
            pointsOnCircular.MoveNext();
            while (figures.Any(rec => rec.Contains(pointsOnCircular.Current)))
                pointsOnCircular.MoveNext();
            return pointsOnCircular.Current;
        }

        Point FindFreeLocationForFigure(IFigure figure)
        {
            while (true)
            {
                var freePoint = FindNextFreePoint();
                figure.Center = freePoint;
                if (!figures.Any(f => f.IntersectsWith(figure)))
                    return figure.Location;
            }
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width < 0 || rectangleSize.Height < 0)
                throw new ArgumentOutOfRangeException();

            var rectLocalLocation = FindFreeLocationForFigure(new Figures.Rectangle() { Size = rectangleSize });

            figures.Add(new Figures.Rectangle(rectLocalLocation, rectangleSize));

            var rectGlobalLocation = new Point(center.X + rectLocalLocation.X, center.Y + rectLocalLocation.Y);
            return new Rectangle(rectGlobalLocation, rectangleSize);
        }
    }
}