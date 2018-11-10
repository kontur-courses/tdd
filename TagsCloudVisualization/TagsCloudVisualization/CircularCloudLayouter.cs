using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        private readonly Circle circle;


        public CircularCloudLayouter(Point pointCenter)
        {
            circle = new Circle(pointCenter);
        }

        public Point GetCenter()
        {
            return circle.Center;
        }

        public Size GetSizeTagCloud()
        {
            return new Size(circle.Radius*2, circle.Radius*2);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangles.Count == 0)
                return CreateRectangle(new Point(-rectangleSize.Width/2, -rectangleSize.Height/2), rectangleSize);
             
            var resultLocation = Point.Empty;
            while (true)
            {
                if (TryFindLocation(circle.TopHalfPoints, -rectangleSize.Width/2, -rectangleSize.Height, rectangleSize, out resultLocation))
                    return CreateRectangle(resultLocation, rectangleSize);
                    
                if (TryFindLocation(circle.BottomHalfPoints, -rectangleSize.Width/2, -rectangleSize.Height/2, rectangleSize, out resultLocation))
                    return CreateRectangle(resultLocation, rectangleSize);

                circle.IncrementRadius(5);
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
            foreach (var point in arc)
            {
               
                var startPoint = new Point(point.X + xOffset, point.Y + yOffset);
                location = startPoint;
                if (!IsIntersectOtherRectangles(startPoint, sizeRect))                                    
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
