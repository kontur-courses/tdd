using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly Point center;
        private readonly LinkedList<Rectangle> rectangles;
        private double angle = 0;
        private double radius = 0;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            rectangles = new LinkedList<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width == 0 || rectangleSize.Height == 0)
                throw new ArgumentException();

            var rectangle = GetAddedRectangle(rectangleSize, center, angle, radius);
            while (rectangle.IsIntersectsOthersRectangles(rectangles))
            {
                angle += Math.PI / 12;
                radius += 0.25;
                rectangle = GetAddedRectangle(rectangleSize, center, angle, radius);
            }

            var movingPoint = GetMovingToCenterVector(rectangle,center);
            ShiftRectangleToCenter(ref rectangle, movingPoint);
            ShiftRectangleToCenter(ref rectangle, new Point(movingPoint.X,0));
            ShiftRectangleToCenter(ref rectangle, new Point(0,movingPoint.Y));

            rectangles.AddLast(rectangle);
            return rectangle;
        }

        private void ShiftRectangleToCenter(ref Rectangle rectangle, Point shiftPoint)
        {
            if (shiftPoint.X == 0 && shiftPoint.Y == 0)
                return;

            var nextRectangle = rectangle;
            var resultRectangle = rectangle;
            for (int x = rectangle.X, y = rectangle.Y;
                 !nextRectangle.IsIntersectsOthersRectangles(rectangles) && x != center.X && y != center.Y;
                 x += shiftPoint.X, y += shiftPoint.Y)
            {
                resultRectangle = nextRectangle;
                nextRectangle = new Rectangle(x, y, rectangle.Width, rectangle.Height);
            }
            rectangle = resultRectangle;
        }

        private Rectangle GetAddedRectangle(Size rectangleSize, Point centerPoint, double angle, double radius)
        {
            var location = new Point(centerPoint.X + GetX(radius, angle), centerPoint.Y + GetY(radius, angle));
            return new Rectangle(location, rectangleSize);
        }

        private Point GetMovingToCenterVector(Rectangle rectangle, Point center)
        {
            var x = (center.X - rectangle.X);
            var y = (center.Y - rectangle.Y);
            if (x != 0)
                x /= Math.Abs(x);
            if (y != 0)
                y /= Math.Abs(y);
            return new Point(x, y);
        }

        private int GetX(double r, double a) => (int)(r * Math.Cos(a));
        private int GetY(double r, double a) => (int)(r * Math.Sin(a));
    }
}
