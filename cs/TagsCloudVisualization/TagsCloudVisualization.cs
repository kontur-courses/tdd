using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Linq;
using Newtonsoft.Json.Bson;

namespace TagsCloudVisualization
{
    public class TagsCloudVisualization
    {
        private readonly Point center;
        private readonly List<Rectangle> rectangles;
        private readonly RoundSpiralPositionGenerator positionGenerator;

        public TagsCloudVisualization(Point center)
        {
            if (center.X < 0 || center.Y < 0)
            {
                throw new ArgumentException();
            }
            this.center = center;
            rectangles = new List<Rectangle>();
            positionGenerator = new RoundSpiralPositionGenerator(center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width < 0 || rectangleSize.Height < 0)
            {
                throw new ArgumentException();
            }
            var nextPosition = positionGenerator.Next();
            var rectangle = new Rectangle(nextPosition, rectangleSize);
            while (IntersectsWithPrevious(rectangle) || rectangle.X < 0 || rectangle.Y < 0)
            {
                nextPosition = positionGenerator.Next();
                rectangle.MoveToPosition(nextPosition);
            }
            OptimizeLocation(ref rectangle);
            rectangles.Add(rectangle);
            return rectangle;
        }

        private void OptimizeLocation(ref Rectangle rectangle)
        {
            OptimizeOneCoordinate(ref rectangle, (Rectangle rect) => center.X - rect.X, new Point(Math.Sign(rectangle.X - center.X) * -1, 0));
            OptimizeOneCoordinate(ref rectangle, (Rectangle rect) => center.Y - rect.Y, new Point(0, Math.Sign(rectangle.Y - center.Y) * -1));
        }

        private void OptimizeOneCoordinate(ref Rectangle rectangle, Func<Rectangle, int> currentDifference, Point delta)
        {
            var previousLocation = rectangle.Location;
            while (currentDifference(rectangle) != 0 && !IntersectsWithPrevious(rectangle))
            {
                previousLocation = rectangle.Location;
                rectangle.Move(delta.X, delta.Y);
            }
            rectangle.Location = previousLocation;
        }

        private bool IntersectsWithPrevious(Rectangle rectangle)
        {
            return rectangles.Any(previousRectangle => previousRectangle.IntersectsWith(rectangle));
        }
    }
}
