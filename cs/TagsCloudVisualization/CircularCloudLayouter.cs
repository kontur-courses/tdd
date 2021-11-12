using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly SpiralPointsCreator pointsCreator;
        private readonly List<Rectangle> rectangles;

        public CircularCloudLayouter(Point center)
        {
            pointsCreator = new SpiralPointsCreator(center);
            rectangles = new List<Rectangle>();
        }


        public Rectangle PutNextRectangle(Size size)
        {
            if (size.Height <= 0 || size.Width <= 0)
                throw new ArgumentException("All size params should be greater than zero!");

            Rectangle nextRectangle;
            do
            {
                nextRectangle = new Rectangle(pointsCreator.GetNextPoint(), size);
            } while (nextRectangle.IntersectsWith(rectangles));

            rectangles.Add(nextRectangle);
            return nextRectangle;
        }

        public Rectangle[] GetRectangles() => rectangles.ToArray();

    }
}