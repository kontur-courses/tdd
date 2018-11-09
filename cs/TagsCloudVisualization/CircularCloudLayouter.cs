using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly SpiralTrack spiralTrack;
        private readonly List<Rectangle> pastRectangles = new List<Rectangle>();

        public CircularCloudLayouter(Point center)
        {
            var step = 0.5;
            spiralTrack = new SpiralTrack(center, step);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var nextRectangle = Rectangle.Empty;

            while (nextRectangle == Rectangle.Empty ||
                   IntersectWithPastRectangles(nextRectangle))
            {
                var point = spiralTrack.GetNextPoint();
                var location = new Point(
                    point.X - rectangleSize.Width / 2,
                    point.Y - rectangleSize.Height / 2); 
                nextRectangle = new Rectangle(location, rectangleSize);
            }

            pastRectangles.Add(nextRectangle);
            return nextRectangle;
        }

        private bool IntersectWithPastRectangles(Rectangle rectangle) =>
            pastRectangles.Any(rect => rect.IntersectsWith(rectangle));
    }
}
