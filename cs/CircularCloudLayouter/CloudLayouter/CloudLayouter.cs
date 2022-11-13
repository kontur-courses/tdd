using System;
using System.Collections.Generic;
using System.Drawing;


// ReSharper disable once CheckNamespace
namespace TagCloudVisualization
{
    public class CloudLayouter
    {
        private readonly List<Rectangle> rectangles;
        private readonly IEnumerator<Point> pointEnumerator;
        private readonly ICurve curve;

        public Rectangle[] Rectangles
        {
            get
            {
                if(center != curve.Center)
                    ChangeCenterPoint(curve.Center);
                return rectangles.ToArray();
            }
        }

    private Point center;

        public Point Center
        {
            get
            {
                if (center != curve.Center)
                {
                    ChangeCenterPoint(curve.Center);
                }
                return center;
            }

        }

        public CloudLayouter(ICurve curve)
        {
            this.curve = curve;
            pointEnumerator = curve.GetEnumerator();
            rectangles = new List<Rectangle>();
            center = curve.Center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
            {
                throw new ArgumentException("size cannot be less than or equal to zero");
            }

            if (center != curve.Center)
            {
                ChangeCenterPoint(curve.Center);
            }

            var nextRectangle = new Rectangle(GetCenterPointRectangle(center, rectangleSize),
                rectangleSize);
            while (true)
            {
                if (!nextRectangle.AreIntersectedAny(rectangles))
                    break;
                pointEnumerator.MoveNext();
                var location = pointEnumerator.Current;
                location = GetCenterPointRectangle(location, rectangleSize);
                nextRectangle = new Rectangle(location, rectangleSize);
            }

            rectangles.Add(nextRectangle);
            return nextRectangle;
        }

        private static Point GetCenterPointRectangle(Point location, Size size)
        {
            return new Point(
                location.X - size.Width / 2,
                location.Y - size.Height / 2);
        }

        public void ChangeCenterPoint(Point newCenter)
        {
            curve.ChangeCenterPoint(newCenter);
            var directionVector = new Point(newCenter.X - center.X,
                newCenter.Y - center.Y);
            center = new Point(newCenter.X, newCenter.Y);

            for (var i = 0; i < rectangles.Count; i++)
            {
                var newLocation = new Point(rectangles[i].X + directionVector.X,
                    rectangles[i].Y + directionVector.Y);
                rectangles[i] = new Rectangle(newLocation, rectangles[i].Size);
            }
        }
    }
}