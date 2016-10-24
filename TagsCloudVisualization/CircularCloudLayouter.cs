using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : IEnumerable<Rectangle>
    {
        public Point Center { get; }
        private readonly Random random;
        private int radiusSetting;
        private int stepRadiusSetting = 5;
        private double deflectionAngle = Math.PI*2;
        private double stepDeflectionAngle = Math.PI / 24;
        private readonly List<Rectangle> existingRectangles;

        public CircularCloudLayouter(Point center)
        {
            random = new Random();
            existingRectangles = new List<Rectangle>();
            Center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {  
            deflectionAngle = 2 * Math.PI * random.NextDouble(); 
            while (true)
            {
                while (deflectionAngle > 0)
                {
                    int x = Center.X + (int) (radiusSetting*Math.Cos(deflectionAngle)) - rectangleSize.Width / 2;
                    int y = Center.Y + (int) (radiusSetting*Math.Sin(deflectionAngle)) - rectangleSize.Height / 2;
                    var newRectangle = new Rectangle(new Point(x, y), rectangleSize);
                    if (!IsIntersectionWithRectangles(newRectangle))
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

        private bool TryShiftToDirection(int directionX, int directionY, Rectangle rectangle, out Rectangle resultrectangle)
        {
            var newLocation = new Point(rectangle.X + directionX, rectangle.Y + directionY);
            resultrectangle = new Rectangle(newLocation, rectangle.Size);
            return !IsIntersectionWithRectangles(resultrectangle);
        }

        private Rectangle ShiftedToCenter(Rectangle rectangle)
        {
            var currentRectangle = rectangle;
            var rectengleCenter = new Point(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
            var directionX = Math.Sign(Center.X - rectengleCenter.X);
            var directionY = Math.Sign(Center.Y - rectengleCenter.Y);
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
                directionY = Math.Sign(Center.Y - rectengleCenter.Y);
                directionX = Math.Sign(Center.X - rectengleCenter.X);
            }
            return currentRectangle;
        }

        private bool IsIntersectionWithRectangles(Rectangle rectangle)
        {
            return existingRectangles.Any(r => r.IntersectsWith(rectangle));
        }

        public IEnumerator<Rectangle> GetEnumerator()
        {
            return existingRectangles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}