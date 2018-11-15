using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        private readonly Circle circle;

        public Size Size
        {
            get { return new Size(circle.Radius * 2, circle.Radius * 2); }
        }

        public CircularCloudLayouter(Point pointCenter)
        {
            circle = new Circle(pointCenter);
        }


        public Point GetCenter()
        {
            return circle.Center;
        }


        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangles.Count == 0)
                return CreateRectangle(new Point(-rectangleSize.Width/2, -rectangleSize.Height/2), rectangleSize);
             
            var resultLocation = Point.Empty;
            while (true)
            {
                if (TryFindLocation(rectangleSize, out resultLocation))
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

        private bool TryFindLocation(Size sizeRect, out Point location)
        {
            var yOffset = -sizeRect.Height / 2;
            var xOffset = -sizeRect.Width / 2;
            foreach (var point in circle.GetCirclePoints())
            {
                location = new Point(point.X + xOffset, point.Y + yOffset);
                if (!IsIntersectOtherRectangles(location, sizeRect))
                    return true;
                location = new Point(point.X + xOffset, -point.Y + yOffset);
                if (!IsIntersectOtherRectangles(location, sizeRect))
                    return true;
            }
            location = Point.Empty;
            return false;
        }

        private bool IsIntersectOtherRectangles(Point point, Size size)
        {
            return rectangles.Any(rect => rect.IntersectsWith(new Rectangle(point, size)));
        }
    }
}
