using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        public List<Rectangle> rectanglesList;
        private List<int> anglesList;
        private Point сenter;
        private int radius;

        public CircularCloudLayouter(Point currentCenter)
        {
            if (currentCenter.X < 0 || currentCenter.Y < 0)
                throw new ArgumentException("Center coordinates should be greater than null");
            rectanglesList = new List<Rectangle>();
            anglesList = new List<int>() { 36, 72, 108, 144, 180, 216, 252, 288, 324, 0 };
            сenter = currentCenter;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectanglesList.Count == 0)
            {
                var newCoordinates = new Point(сenter.X - (rectangleSize.Width / 2), сenter.Y - (rectangleSize.Height / 2));
                rectanglesList.Add(new Rectangle(newCoordinates, rectangleSize));
                radius = Math.Min(rectangleSize.Height, rectangleSize.Width) / 2;
                return new Rectangle(newCoordinates, rectangleSize);
            }

            var rectangle = new Rectangle(сenter, rectangleSize);
            var min = int.MaxValue;
            var result = сenter;
            while (result == сenter)
            {
                radius += Math.Min(rectangleSize.Height, rectangleSize.Width);
                for (var i = 0; i < anglesList.Count; i++)
                {
                    var angle = (anglesList[i] * (Math.PI)) / 180;
                    var newX = сenter.X + radius * Math.Cos(angle);
                    newX += Math.Sign(radius * Math.Cos(angle)) * rectangle.Width;
                    var newY = сenter.Y + radius * Math.Sin(angle);
                    newY += Math.Sign(radius * Math.Sin(angle)) * rectangle.Height;
                    rectangle.Location = new Point((int)newX, (int)newY);
                    if (Check(rectangle))
                    {
                        if ((Math.Pow(rectangleSize.Width, 2) + Math.Pow(rectangleSize.Height, 2)) < min)
                        {
                            min = (int)(Math.Pow(newY - сenter.Y, 2) + Math.Pow(newY - сenter.Y, 2));
                            result = new Point((int)newX, (int)newY);
                        }
                    }
                }
            }
            rectangle.Location = result;
            rectangle.Location = GetNormalizedRectanglePosition(rectangle, true, true);
            rectangle.Location = GetNormalizedRectanglePosition(rectangle, false, true);
            rectangle.Location = GetNormalizedRectanglePosition(rectangle, true, false);
            rectanglesList.Add(rectangle);
            radius = Math.Min(rectanglesList[0].Height, rectanglesList[0].Width) / 2;
            return rectanglesList[rectanglesList.Count - 1];
        }

        private bool Check(Rectangle rectangle)
        {
            for (var i = 0; i < rectanglesList.Count; i++)
            {
                if (rectanglesList[i].IntersectsWith(rectangle))
                    return false;
            }
            return true;
        }

        private Point GetNormalizedRectanglePosition(Rectangle rectangle, bool isXChanged, bool isYChanged)
        {
            if (isXChanged && !isYChanged && (сenter.X - rectangle.X) == 0)
                return rectangle.Location;
            if (isYChanged && !isXChanged && (сenter.Y - rectangle.Y) == 0)
                return rectangle.Location;
            var coefX = isXChanged ? 1 : 0;
            var coefY = isYChanged ? 1 : 0;

            while (Check(rectangle))
            {
                var oldsighnY = Math.Sign(сenter.Y - rectangle.Y);
                var oldsighnX = Math.Sign(сenter.Y - rectangle.Y);
                var newX = rectangle.X + Math.Sign(сenter.X - rectangle.X) * 5 * coefX;
                var newY = rectangle.Y + Math.Sign(сenter.Y - rectangle.Y) * 5 * coefY;
                if (oldsighnY != Math.Sign(сenter.Y - newY))
                    break;
                if (oldsighnX != Math.Sign(сenter.X - newX))
                    break;
                var newCoordinates = new Point((int)newX, (int)newY);
                rectangle.Location = newCoordinates;
            }
            while (!Check(rectangle))
            {
                var newX = rectangle.X - Math.Sign(сenter.X - rectangle.X) * 5 * coefX;
                var newY = rectangle.Y - Math.Sign(сenter.Y - rectangle.Y) * 5 * coefY;
                var newCoordinates = new Point((int)newX, (int)newY);
                rectangle.Location = newCoordinates;
            }
            return rectangle.Location;
        }        
    }
}
