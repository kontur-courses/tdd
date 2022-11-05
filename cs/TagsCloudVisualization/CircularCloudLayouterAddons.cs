using System;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public partial class CircularCloudLayouter
    {
        private Point GetNextRectanglePosition(Size rectangleSize)
        {
            var nextPoint = new Point(0, 0);

            if (rectangles.Count == 0)
                return nextPoint;

            var point = GetPotentialPoint();

            while (rectangles.Any(rectangle =>
                       IsRectanglesIntersect(new Rectangle(point, rectangleSize), rectangle)))
                point = GetPotentialPoint();
            
            nextPoint = point;

            return nextPoint;
        }
        
        private Point GetPotentialPoint()
        {
            angle += 0.3f;
            var x = 5.0f * angle * Math.Cos(angle);
            var y =  2.5f * angle * Math.Sin(angle);

            var point = new Point((int)x, (int)y);
            spiralPoints.Add(point);
                
            return point;
        }

        private static bool IsRectanglesIntersect(Rectangle firstRect, Rectangle secondRect)
        {
            return !(firstRect.X > secondRect.X + secondRect.Width
                     || firstRect.X + firstRect.Width < secondRect.X
                     || firstRect.Y > secondRect.Y + secondRect.Height
                     || firstRect.Y + firstRect.Height < secondRect.Y);
        }
    }
}