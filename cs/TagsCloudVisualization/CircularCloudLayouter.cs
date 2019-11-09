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
            var rectangleCenter = rectangle.GetCenter();
            var xDirection = 0;
            var yDirection = 0;
            while (!rectangle.IntersectsWithAny(Rectangles) && Center.GetDistanceTo(rectangleCenter) > 10)
            {
                xDirection = rectangleCenter.X >= Center.X ? rectangleCenter.X == Center.X ? 0 : -1 : 1;
                yDirection = rectangleCenter.Y >= Center.Y ? rectangleCenter.Y == Center.Y ? 0 : -1 : 1;
                rectangle.Offset(xDirection, yDirection);
            }
            rectangle.Offset(-xDirection, -yDirection);
            return rectangle;
        }
        
        public double GetCloudHorizontalLength()
        {
            var minX = Rectangles.OrderBy(rect => rect.X).First().X;
            var leftMostPoint = new Point(minX, Center.Y);
            var maxX = Rectangles.OrderBy(rect => rect.Right).Last().Right;
            var rightMostPoint = new Point(maxX, Center.Y);
            return leftMostPoint.GetDistanceTo(rightMostPoint);
        }
        
        public double GetCloudVerticalLength()
        {
            var minY = Rectangles.OrderBy(rect => rect.Y).First().Y;
            var topMostPoint = new Point(Center.X, minY); ;
            var maxY = Rectangles.OrderBy(rect => rect.Bottom).Last().Bottom;
            var bottomMostPoint = new Point(Center.X, maxY);
            return topMostPoint.GetDistanceTo(bottomMostPoint);
        }
    }
}