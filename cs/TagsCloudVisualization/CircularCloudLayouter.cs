using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly ArchimedesSpiral archimedesSpiral;

        public List<Rectangle> Rectangles { get; }
        public Point Center { get; }

        public CircularCloudLayouter(Point center)
        {
            this.Center = center;
            archimedesSpiral = new ArchimedesSpiral(center, 0.1);
            Rectangles = new List<Rectangle>();
        }
        
        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if(rectangleSize.IsEmpty)
                throw new ArgumentException("Empty rectangle size");
            var rectangle = GetNextRectangle(rectangleSize);
            Rectangles.Add(rectangle);
            return rectangle;
        }

        private Rectangle GetNextRectangle(Size rectangleSize)
        {
            var rectangleToPlace = default(Rectangle);
            foreach (var point in archimedesSpiral.GetSpiralPoints())
            {
                var possibleRectangle = new Rectangle(point, rectangleSize).GetCentered(point);
                if (possibleRectangle.IntersectsWithAny(Rectangles)) continue;
                rectangleToPlace = GetRectanglePushedCloserToCenter(possibleRectangle);
                break;
            }
            return rectangleToPlace;
        }

        private Rectangle GetRectanglePushedCloserToCenter(Rectangle rectangle)
        {
            var shouldPushByX = true;
            var shouldPushByY = true;
            var lastNonZeroXOffset = 0;
            var lastNonZeroYOffset = 0;
            while (shouldPushByX || shouldPushByY)
            {
                var rectangleCenter = rectangle.GetCenter();
                var xDirection = 0;
                var yDirection = 0;
                
                xDirection = rectangleCenter.X >= Center.X ? rectangleCenter.X == Center.X ? 0 : -1 : 1;
                shouldPushByX = ShouldPushRectangleByX(rectangle, xDirection);
                if (!shouldPushByX)
                    xDirection = 0;
                else if(xDirection != 0)
                    lastNonZeroXOffset = xDirection;
                
                yDirection = rectangleCenter.Y >= Center.Y ? rectangleCenter.Y == Center.Y ? 0 : -1 : 1;
                shouldPushByY = ShouldPushRectangleByY(rectangle, yDirection);
                if (!shouldPushByY) 
                    yDirection = 0;
                else if(yDirection != 0)
                    lastNonZeroYOffset = yDirection;

                rectangle.Offset(xDirection, yDirection);
                if (!rectangle.IntersectsWithAny(Rectangles)) continue;
                rectangle.Offset(-lastNonZeroXOffset, -lastNonZeroYOffset);
                break;
            }
            return rectangle;
        }

        private bool ShouldPushRectangleByX(Rectangle rectangle, int dx)
        {
            rectangle.Offset(dx, 0);
            return !rectangle.IntersectsWithAny(Rectangles) && rectangle.GetCenter().X != Center.X;
        }
        
        private bool ShouldPushRectangleByY(Rectangle rectangle, int dy)
        {
            rectangle.Offset(0, dy);
            return !rectangle.IntersectsWithAny(Rectangles) && rectangle.GetCenter().Y != Center.Y;
        }
    }
}