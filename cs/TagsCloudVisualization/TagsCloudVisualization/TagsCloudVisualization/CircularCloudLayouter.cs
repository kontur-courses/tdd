using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        const int maxAngleInDegrees = 360;
        const int angleIncrementInDegrees = 15;
        const int radiusIncrement = 1;

        Point cloudCenter;
        int currentSpiralAngleInDegrees;
        int spiralRadius;
        List<Rectangle> rectangles;

        public CircularCloudLayouter(Point center)
        {
            cloudCenter = center;
            rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            Point rectangleLocation;

            if (!rectangles.Any())
            {
                rectangleLocation = new Point();
                spiralRadius = Math.Min(rectangleSize.Width, rectangleSize.Height);
            }
            else
                rectangleLocation = FindRectangleLocation(rectangleSize);

            var rectangle = new Rectangle(rectangleLocation, rectangleSize);
            rectangles.Add(rectangle);
            rectangle.Location = new Point(cloudCenter.X + rectangle.Location.X, cloudCenter.Y + rectangle.Location.Y);
            return rectangle;
        }

        bool CheckIntersections(Rectangle rectangle)
        {
            var emptyRectangle = new Rectangle(new Point(0, 0), new Size(0, 0));
            foreach (var r in rectangles)
            {
                var intersect = Rectangle.Intersect(r, rectangle);
                if (!intersect.Equals(emptyRectangle))
                    return true;
            }

            return false;
        }

        double ConvertFromDegreesToRadians(double degrees)
        {
            return 2 * Math.PI / (maxAngleInDegrees / degrees);
        }

        void UnfoldSpiral()
        {
            currentSpiralAngleInDegrees += angleIncrementInDegrees;
            if (currentSpiralAngleInDegrees >= maxAngleInDegrees)
            {
                currentSpiralAngleInDegrees -= maxAngleInDegrees;
                spiralRadius += radiusIncrement;
            }
        }

        Point FindRectangleLocation(Size rectangleSize)
        {
            var newPoint = new Point
                (
                    (int)(spiralRadius * Math.Sin(ConvertFromDegreesToRadians(currentSpiralAngleInDegrees))),
                    (int)(spiralRadius * Math.Cos(ConvertFromDegreesToRadians(currentSpiralAngleInDegrees)))
                );
            var newRectangle = new Rectangle(newPoint,rectangleSize);

            UnfoldSpiral();

            if (CheckIntersections(newRectangle))
            {
                newPoint = FindRectangleLocation(rectangleSize);
            }

            return OptimizeRectangleLocation(newPoint, rectangleSize);
        }

        Point MovePointToCenter(Point point)
        {
            var vector = new Point(-point.X, -point.Y);
            var newX = vector.X;
            var newY = vector.Y;
            if (newX != 0)
                newX = point.X + vector.X / Math.Abs(vector.X);
            if (newY != 0)
                newY = point.Y + vector.Y / Math.Abs(vector.Y);
            return new Point(newX, newY);
        }

        Point OptimizeRectangleLocation(Point point, Size rectangleSize)
        {
            var rectangle = new Rectangle(point,rectangleSize);
            while (true)
            {
                var newLocation = MovePointToCenter(point);
                rectangle.Location = newLocation;
                if (!CheckIntersections(rectangle))
                {
                    point = newLocation;
                }
                else
                    break;
            }

            return point;
        }
    }
}
