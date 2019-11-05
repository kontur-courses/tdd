using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Linq;
using Newtonsoft.Json.Bson;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly Point center;
        private readonly List<Rectangle> rectangles;
        private readonly RoundSpiralPositionGenerator positionGenerator;

        public CircularCloudLayouter(Point center)
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
                throw new ArgumentException("Rectangle cannot have negative size");
            }
            var nextPosition = positionGenerator.Next();
            var rectangle = new Rectangle(nextPosition, rectangleSize);
            while (IntersectsWithPrevious(rectangle))
            {
                nextPosition = positionGenerator.Next();
                rectangle.MoveToPosition(nextPosition);
            }
            if (rectangles.Count > 0)
                OptimizeLocation(ref rectangle);
            rectangles.Add(rectangle);
            return rectangle;
        }

        private void OptimizeLocation(ref Rectangle rectangle)
        {
            Func<Rectangle, int> getDeltaYByCenterCoordinate = (Rectangle rect) => center.Y - rect.Y;
            Func<Rectangle, int> getDeltaXByCenterCoordinate = (Rectangle rect) => center.X - rect.X;
            for (var i = 0; i < 5; i++)
            {
                OptimizeOneCoordinate(ref rectangle, getDeltaYByCenterCoordinate, 0, Math.Sign(rectangle.Y - center.Y) * -1);
                OptimizeOneCoordinate(ref rectangle, getDeltaXByCenterCoordinate, Math.Sign(rectangle.X - center.X) * -1, 0);
            }
        }

        private void OptimizeOneCoordinate(ref Rectangle rectangle, Func<Rectangle, int> getDeltaByCenterCoordinate, int deltaXByStep, int deltaYByStep)
        {
            var onCenter = false;
            while (!IntersectsWithPrevious(rectangle))
            {
                if (getDeltaByCenterCoordinate(rectangle) == 0)
                {
                    onCenter = true;
                    break;
                }
                rectangle.Move(deltaXByStep, deltaYByStep);
            }
            if (!onCenter) 
                rectangle.Move(-deltaXByStep, -deltaYByStep);
        }

        private bool IntersectsWithPrevious(Rectangle rectangle)
        {
            return rectangles.Any(previousRectangle => previousRectangle.IntersectsWith(rectangle));
        }

        public Rectangle GetSizeOfImage()
        {
            var maxX = rectangles.Max(rectangle => rectangle.Right);
            var maxY = rectangles.Max(rectangle => rectangle.Bottom);
            var minX = rectangles.Min(rectangle => rectangle.Left);
            var minY = rectangles.Min(rectangle => rectangle.Top);
            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        public List<Rectangle> GetRectangles()
        {
            return this.rectangles;
        }
    }
}
