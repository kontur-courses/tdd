using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly Point center;
        private readonly Random random;
        private int radiusSetting;
        private int stepRadiusSetting = 5;
        private double stepDeflectionAngle = Math.PI / 36;
        private readonly List<Rectangle> existingRectangles;

        public CircularCloudLayouter(Point center)
        {
            random = new Random();
            existingRectangles = new List<Rectangle>();
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {  
            var deflectionAngle = 2 * Math.PI * random.NextDouble(); 
            while (true)
            {
                while (deflectionAngle > 0)
                {
                    var x = center.X + (int) (radiusSetting*Math.Cos(deflectionAngle)) - rectangleSize.Width / 2;
                    var y = center.Y + (int) (radiusSetting*Math.Sin(deflectionAngle)) - rectangleSize.Height / 2;
                    var newRectangle = new Rectangle(new Point(x, y), rectangleSize);
                    if (!newRectangle.IsIntersectionWithRectangles(existingRectangles))
                    {
                        var resultRectangle = ShiftedToCenter(newRectangle);
                        existingRectangles.Add(resultRectangle);
                        return resultRectangle;
                    }
                    deflectionAngle -= stepDeflectionAngle;
                }
                deflectionAngle = 2 * Math.PI * random.NextDouble();
                radiusSetting += stepRadiusSetting;
            }
        }

        private bool TryShiftToDirection(int directionX, int directionY, Rectangle rectangle, out Rectangle resultRectangle)
        {
            var newLocation = new Point(rectangle.X + directionX, rectangle.Y + directionY);
            resultRectangle = new Rectangle(newLocation, rectangle.Size);
            return !resultRectangle.IsIntersectionWithRectangles(existingRectangles);
        }

        private Rectangle ShiftedToCenter(Rectangle rectangle)
        {
            var currentRectangle = rectangle;
            var rectengleCenter = new Point(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
            var directionX = Math.Sign(center.X - rectengleCenter.X);
            var directionY = Math.Sign(center.Y - rectengleCenter.Y);
            while (directionY != 0 || directionX != 0)
            {
                Rectangle newRectangle;
                if (directionY != 0 && TryShiftToDirection(0, directionY, currentRectangle, out newRectangle))
                {
                    currentRectangle = newRectangle;
                }
                else
                {
                    if (directionX != 0 && TryShiftToDirection(directionX, 0, currentRectangle, out newRectangle))
                    {
                        currentRectangle = newRectangle;
                    }
                    else
                    {
                        break;
                    }
                }
                rectengleCenter = new Point(
                    currentRectangle.X + currentRectangle.Width / 2,
                    currentRectangle.Y + currentRectangle.Height / 2);
                directionY = Math.Sign(center.Y - rectengleCenter.Y);
                directionX = Math.Sign(center.X - rectengleCenter.X);
            }
            return currentRectangle;
        }
    }
}