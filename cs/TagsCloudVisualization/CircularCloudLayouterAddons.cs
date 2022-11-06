using System;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public partial class CircularCloudLayouter
    {
        private Point GetNextRectanglePosition(Size rectangleSize)
        {
            var shiftX = -rectangleSize.Width / 2;
            var shiftY = -rectangleSize.Height / 2;
            
            if (rectangles.Count == 0)
                return new Point(center.X + shiftX, center.Y + shiftY);

            var potentialPoint = GetPotentialPoint();

            while (rectangles.Any(rectangle =>
                       IsRectanglesIntersect(new Rectangle(potentialPoint, rectangleSize), rectangle)))
                potentialPoint = GetPotentialPoint();

            return potentialPoint;
        }
        
        private Point GetPotentialPoint()
        {
            angle += 0.1f;
            var x = center.X + 5.0f * angle * Math.Cos(angle);
            var y = center.Y + 2.5f * angle * Math.Sin(angle);

            var point = new Point((int)x, (int)y);
            spiralPoints.Add(point);
                
            return point;
        }

        public static bool IsRectanglesIntersect(Rectangle firstRect, Rectangle secondRect)
        {
            return !Rectangle.Intersect(firstRect, secondRect).IsEmpty;
        }

        public void Clear()
        {
            rectangles.Clear();
            spiralPoints.Clear();
            angle = 0;
        }
    }
}