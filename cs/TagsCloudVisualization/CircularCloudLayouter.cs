using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private Point center;
        private List<Rectangle> rectangles = new List<Rectangle>();
        private CircularCloudLayouterRectanglePosition nextRectanglePosition;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = new Rectangle(new Point(0,0), rectangleSize);
            if (rectangles.Count == 0)
            {
                var centeredRectanglePoint = new Point(center.X - rectangleSize.Width / 2, 
                    center.Y - rectangleSize.Height / 2);
                rectangle.Location = centeredRectanglePoint;
                nextRectanglePosition = CircularCloudLayouterRectanglePosition.RIGHT;
            }
            else
            {
                rectangle.Location = GetNewLocationByNextPosition();
                SetNextRectanglePosition();
            }
            rectangles.Add(rectangle);
            return rectangle;
        }

        private Point GetNewLocationByNextPosition()
        {
            
            var lastRectangle = rectangles.Last();
            var newLocation = lastRectangle.Location;
            switch (nextRectanglePosition)
            {
                case CircularCloudLayouterRectanglePosition.RIGHT:
                {
                    newLocation.Offset(lastRectangle.Width, 0);
                    break;
                }
                case CircularCloudLayouterRectanglePosition.BOTTOM:
                {
                    newLocation.Offset(0, lastRectangle.Height);
                    break;
                }
                case CircularCloudLayouterRectanglePosition.LEFT:
                {
                    newLocation.Offset(- lastRectangle.Width, 0);
                    break;
                }
                case CircularCloudLayouterRectanglePosition.TOP:
                {
                    newLocation.Offset(0, - lastRectangle.Height);
                    break;
                }
            }

            return newLocation;
        }

        private void SetNextRectanglePosition()
        {
            switch (nextRectanglePosition)
            {
                case CircularCloudLayouterRectanglePosition.RIGHT:
                    nextRectanglePosition = CircularCloudLayouterRectanglePosition.BOTTOM;
                    break;
                case CircularCloudLayouterRectanglePosition.BOTTOM:
                    nextRectanglePosition = CircularCloudLayouterRectanglePosition.LEFT;
                    break;
                case CircularCloudLayouterRectanglePosition.LEFT:
                    nextRectanglePosition = CircularCloudLayouterRectanglePosition.TOP;
                    break;
                case CircularCloudLayouterRectanglePosition.TOP:
                    nextRectanglePosition = CircularCloudLayouterRectanglePosition.RIGHT;
                    break;
                default:
                    nextRectanglePosition =  CircularCloudLayouterRectanglePosition.BOTTOM;
                    break;
            }
        }
    }
}