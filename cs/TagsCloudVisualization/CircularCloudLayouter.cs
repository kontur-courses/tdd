using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public CircularCloudLayouter(Point center, IDistribution distribution)
        {
            if (center != distribution.Center)
                throw new ArgumentException();

            Center = center;
            Distribution = distribution;
            WordPositions = new List<Rectangle>();
        }

        public Point Center { get; private set; }
        public List<Rectangle> WordPositions { get; private set; }
        public IDistribution Distribution { get; private set; }


        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width < 1 || rectangleSize.Height < 1)
                throw new ArgumentException();

            var rectanglePosition = Distribution.GetNextPoint();
            var currRectangle = new Rectangle(rectanglePosition, rectangleSize);

            while (CheckIntersection(currRectangle))
            {
                rectanglePosition = Distribution.GetNextPoint();
                currRectangle.Location = rectanglePosition;
            }

            currRectangle = ComperessRectangle(currRectangle);
            WordPositions.Add(currRectangle);
            return currRectangle;
        }


        public bool CheckIntersection(Rectangle currRectangle)
        {
            return WordPositions.Any(rec => currRectangle.IntersectsWith(rec));
        }


        public Rectangle ComperessRectangle(Rectangle rectangle)
        {
            var changes = 1;
            while (changes > 0)
            {
                rectangle = CompressByAxis(rectangle, true, out changes);
                rectangle = CompressByAxis(rectangle, false, out changes);
            }

            return rectangle;
        }


        private Rectangle CompressByAxis(Rectangle rectangle, bool isByX, out int changes)
        {
            changes = 0;
            var stepX = rectangle.X < Center.X ? 1 : -1;
            var stepY = rectangle.Y < Center.Y ? 1 : -1;

            while ((isByX && rectangle.X != Center.X) ||
                   (!isByX && rectangle.Y != Center.Y))
            {
                var newRectangle = isByX
                    ? new Rectangle(new Point(rectangle.X + stepX, rectangle.Y), rectangle.Size)
                    : new Rectangle(new Point(rectangle.X, rectangle.Y + stepY), rectangle.Size);

                if (!CheckIntersection(newRectangle))
                {
                    rectangle = newRectangle;
                    changes++;
                    continue;
                }

                break;
            }

            return rectangle;
        }

    }
}