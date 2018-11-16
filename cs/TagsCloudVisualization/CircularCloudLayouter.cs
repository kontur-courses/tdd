using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly Point center;
        private readonly SpiralTrack spiralTrack;
        private readonly List<Rectangle> pastRectangles = new List<Rectangle>();

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            var step = 0.5;
            spiralTrack = new SpiralTrack(center, step);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var nextRectangle = PutNextRectangleBySpiralTrack(rectangleSize);
            PullToCenter(nextRectangle);
            pastRectangles.Add(nextRectangle);
            return nextRectangle;
        }

        public void PullToCenter(Rectangle rectangle)
        {
            ShiftRectangleAboutAxisBeforeIntersection(rectangle, Axis.OX);
            ShiftRectangleAboutAxisBeforeIntersection(rectangle, Axis.OY);
        }

        public void ShiftRectangleAboutAxisBeforeIntersection(Rectangle rectangle, 
            Axis axis)
        {
            var offset = GetRectangleOffsetAboutAxis(rectangle, axis);
            var shiftedRectangle = rectangle.ShiftByAxis(Math.Sign(offset), axis);

            while (offset != 0 && NotIntersectWithPastRectangles(shiftedRectangle))
            {
                shiftedRectangle = rectangle.ShiftByAxis(Math.Sign(offset), axis);
                rectangle.Location = shiftedRectangle.Location;
                offset = GetRectangleOffsetAboutAxis(rectangle, axis);
            }
        }

        private int GetRectangleOffsetAboutAxis(Rectangle rectangle, Axis axis)
        {
            var rectangleCenter = rectangle.GetCenter();

            switch (axis)
            {
                case Axis.OX:
                    return center.X - rectangleCenter.X;
                case Axis.OY:
                    return center.Y - rectangleCenter.Y;
                default:
                    throw new NotImplementedException();
            }
        }

        private bool NotIntersectWithPastRectangles(Rectangle rectangle) =>
            !pastRectangles.Any(rect => rect.IntersectsWith(rectangle));

        private Rectangle PutNextRectangleBySpiralTrack(Size rectangleSize)
        {
            while (true)
            {
                var point = spiralTrack.GetNextPoint();

                var location = new Point(
                    point.X - rectangleSize.Width / 2,
                    point.Y - rectangleSize.Height / 2);

                var rectangle = new Rectangle(location, rectangleSize);
                if (NotIntersectWithPastRectangles(rectangle))
                    return rectangle;
            }
        }
    }
}
