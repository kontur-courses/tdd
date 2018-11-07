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
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        private readonly Circle circle;


        public CircularCloudLayouter(Point pointCenter)
        {
            circle = new Circle(pointCenter);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangles.Count == 0)
                return CreateRectangle(new Point(circle.Center.X, circle.Center.Y + rectangleSize.Height), rectangleSize);
             
            var resultLocation = Point.Empty;
            while (true)
            {
                if (TryFindLocation(circle.RightHalfPoints, 0, 0, rectangleSize, out resultLocation))
                    return CreateRectangle(resultLocation, rectangleSize);
                    
                if (TryFindLocation(circle.LeftHalfPoints, -rectangleSize.Width, -rectangleSize.Height, rectangleSize, out resultLocation))
                    return CreateRectangle(resultLocation, rectangleSize);

                circle.IncrementRadius(1);
            }
        }

        private Rectangle CreateRectangle(Point point, Size rectSize)
        {
            var curRect = new Rectangle(point, rectSize);
            rectangles.Add(curRect);
            return curRect;
        }

        private bool TryFindLocation(List<Point> arc, int xOffset, int yOffset, Size sizeRect, out Point location)
        {
            location = Point.Empty;
            foreach (var point in arc)
            {
                var startPoint = new Point(point.X + xOffset, point.Y + yOffset);
                location = startPoint;
                if (EnoughSpaceFor(startPoint, sizeRect))                                    
                    return true;                 
            }
            return false;
        }

        private bool EnoughSpaceFor(Point point, Size size)
        {
            return !rectangles.Any(rect => rect.IntersectsWith(new Rectangle(point, size)));
        }

    }
}
