using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ProjectCircularCloudLayouter
{
    public class CircularCloudLayouter
    {
        private readonly List<Rectangle> _rectangles;
        private double _spiralAngle;
        private const double SpiralStep = 0.5;
        public readonly Point Center;
        public int CloudRadius { get; private set; }

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            _rectangles = new List<Rectangle>();
        }
        
        public Rectangle GetCurrentRectangle => _rectangles.Last();

        public List<Rectangle> GetRectangles => _rectangles;

        public void PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("Size width and height must be positive");
            while (true)
            {
                var currentSpiralPosition = GetNewSpiralPoint();
                var rectangle = new Rectangle(currentSpiralPosition, rectangleSize);
                if (IsAnyIntersectWithRectangles(rectangle, _rectangles)) continue;
                _rectangles.Add(rectangle);
                UpdateCloudRadius(rectangle);
                break;
            }
        }

        public static bool IsAnyIntersectWithRectangles(Rectangle rectangleToCheck, List<Rectangle> rectangles) => 
            rectangles.Any(rec => rec.IntersectsWith(rectangleToCheck));
        
        public static int GetCeilingDistanceBetweenPoints(Point first, Point second) => 
            (int)Math.Ceiling(Math.Sqrt((first.X - second.X) * (first.X - second.X) +
                                        (first.Y - second.Y) * (first.Y - second.Y)));
        
        private Point GetNewSpiralPoint()
        {
            var position = new Point((int)(Center.X + SpiralStep*_spiralAngle * Math.Cos(_spiralAngle)), 
                (int)(Center.Y+SpiralStep*_spiralAngle  * Math.Sin(_spiralAngle)));
            _spiralAngle += 0.017;
            return position;
        }
        
        private void UpdateCloudRadius(Rectangle currentRectangle)
        {
            var maxDistance = new []
            {
                GetCeilingDistanceBetweenPoints(currentRectangle.Location, Center),
                GetCeilingDistanceBetweenPoints(currentRectangle.Location + new Size(currentRectangle.Width, 0), Center),
                GetCeilingDistanceBetweenPoints(currentRectangle.Location + new Size(0, currentRectangle.Height), Center),
                GetCeilingDistanceBetweenPoints(currentRectangle.Location + currentRectangle.Size, Center)
            }.Max();
            if (maxDistance> CloudRadius)
                CloudRadius = maxDistance;
        }
    }
}