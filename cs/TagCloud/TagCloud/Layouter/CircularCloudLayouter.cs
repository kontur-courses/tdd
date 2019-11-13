using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloud.Layouter;
using TagCloud.Tests;

namespace TagCloud
{
    public class CircularCloudLayouter
    {
        private ISpiral spiral;
        private readonly Point center;
        private readonly List<Rectangle> rectangleMap;
        
        public int MinXCoord { get; set; }
        public int MinYCoord { get; set; }
        public int MaxXCoord { get; set; }
        public int MaxYCoord { get; set; }

        public Rectangle GetInscribedSquare()
        {   
            // Abs because our coordinates may be negative
            var width = Math.Min(Math.Abs(MaxXCoord - MinXCoord), Math.Abs(MaxYCoord - MinYCoord));
            var height = width;
            return new Rectangle(center.X - width/2, center.Y - height/2, width, height);
        }

        public Rectangle GetCircumscribedSquare()
        {
            // Abs because our coordinates may be negative
            var width = Math.Max(Math.Abs(MaxXCoord - MinXCoord), Math.Abs(MaxYCoord - MinYCoord));
            var height = width;
            return new Rectangle(center.X - width/2, center.Y - height/2, width, height);
        }

        public CircularCloudLayouter(Point center, double roStepMultiplier = 1)
        {
            this.center = center;
            spiral = new СoncentricCircles(roStepMultiplier);
            rectangleMap = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width == 0 || rectangleSize.Height == 0)
                throw new ArgumentException("rectangleSize is degenerate");
            var centerShift = new Point(-rectangleSize.Width / 2, - rectangleSize.Height / 2);
            foreach (var spiralPoint in spiral.IterateBySpiralPoints())
            {
                var rect = new Rectangle(new Point(spiralPoint.X + centerShift.X, spiralPoint.Y + centerShift.Y), rectangleSize).Shift(center);
                if (IsIntersects(rect)) 
                    continue;
                rectangleMap.Add(rect);
                UpdateMinMaxCoords(rect);
                return rect;
            }
            throw new ArgumentException("Spiral doesn't return appropriate points");
        }

        private void UpdateMinMaxCoords(Rectangle rect)
        {
            if (rect.X < MinXCoord)
                MinXCoord = rect.X;
            if (rect.Y < MinYCoord)
                MinYCoord = rect.Y;
            if (rect.X + rect.Width > MaxXCoord)
                MaxXCoord = rect.X + rect.Width;
            if (rect.Y + rect.Height > MaxYCoord)
                MaxYCoord = rect.Y + rect.Height;
        }

        public IEnumerable<Rectangle> GetAllRectangles()
        {
            return rectangleMap.Select(rect => rect.Shift(center));
        }

        private bool IsIntersects(Rectangle target)
        {
            return rectangleMap.Any(rectangle => rectangle.IntersectsWith(target));
        }
    }
}