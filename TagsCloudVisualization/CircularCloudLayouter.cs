using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization

{
    public class CircularCloudLayouter
    {
        public readonly Point Center;
        private readonly int Frequency = 36;

        public List<Rectangle> Rectangles { get; private set; }

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            Rectangles = new List<Rectangle>();
        }
        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException(rectangleSize.ToString());
            var rect = new Rectangle(FindRectangleOptimalLocation(rectangleSize), rectangleSize);
            Rectangles.Add(rect);
            return rect;
        }
        
        private Point FindRectangleOptimalLocation(Size rectSize)
        {
            var result = new Point();
            foreach(var centerPoint in SpiralPointGenerator())
            {
                if(!NewRectangleIntersectsWithAny(centerPoint,rectSize))
                {
                    result = ConvertToLocation(centerPoint, rectSize);
                    break;
                }
            }
            return result;
        }

        private bool NewRectangleIntersectsWithAny(Point rectCenter, Size rectSize)
        {
            var rect = new Rectangle(ConvertToLocation(rectCenter, rectSize), rectSize);
            return rect.IntersectsWithAny(Rectangles);
        }

        private Point ConvertToLocation(Point center, Size recSize)
        {
            return new Point(center.X - recSize.Width / 2, center.Y - recSize.Height / 2);
        }
        private IEnumerable<Point> SpiralPointGenerator()
        {
            var angle = 0.0;
            var curPoint = Center;
            while (true)
            {
                yield return curPoint;
                angle += Math.PI / Frequency;

                var x = (int)(angle * Math.Cos(angle) - Center.X);
                var y = (int)(angle * Math.Sin(angle) - Center.Y);
                curPoint = new Point(x, y);
            }
        }
    }
}