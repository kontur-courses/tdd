using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly Point center;
        private readonly ISpiral spiral;
        private readonly List<Rectangle> rectangles;

        public CircularCloudLayouter(Point center, ISpiral spiral)
        {
            this.center = center;
            this.spiral = spiral;
            rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rect = GetNextRectangle(rectangleSize);
            var shiftedRect = ShiftRectangleToCenter(rect);
            rectangles.Add(shiftedRect);
            return shiftedRect;
        }

        public IEnumerable<Rectangle> Rectangles()
        {
            foreach (var rectangle in rectangles)
            {
                yield return rectangle;
            }
        }

        private Rectangle GetNextRectangle(Size rectangleSize)
        {
            Rectangle possibleRect;
            do
            {
                possibleRect = GetPossibleRectangle(rectangleSize);
            } while (IsIntersectsWithOthersRectangles(possibleRect));

            return possibleRect;
        }

        private bool IsIntersectsWithOthersRectangles(Rectangle rect)
        {
            return rectangles.Any(other => Rectangle.Intersect(rect, other) != Rectangle.Empty);
        }

        private Rectangle GetPossibleRectangle(Size rectangleSize)
        {
            var nextSpiralPoint = Point.Round(spiral.GetNextPoint());
            nextSpiralPoint.Offset(center);
            nextSpiralPoint.Offset(-rectangleSize.Width / 2, -rectangleSize.Height / 2);
            var possibleRect = new Rectangle(nextSpiralPoint, rectangleSize);
            return possibleRect;
        }

        private Rectangle ShiftRectangleToCenter(Rectangle rect)
        {
            var minDistanceSquare = int.MaxValue;
            var shiftedRect = rect;
            var queue = new Queue<Rectangle>();
            queue.Enqueue(rect);
            while (queue.Count != 0)
            {
                var currentRect = queue.Dequeue();
                var squareDistanceToCenter = GetSquareDistanceToCenter(currentRect);
                if (squareDistanceToCenter >= minDistanceSquare || IsIntersectsWithOthersRectangles(currentRect))
                    continue;
                minDistanceSquare = squareDistanceToCenter;
                shiftedRect = currentRect;
                foreach (var adjacentRectangle in GetAdjacentRectangles(currentRect))
                    queue.Enqueue(adjacentRectangle);
            }

            return shiftedRect;
        }

        private int GetSquareDistanceToCenter(Rectangle currentRect)
        {
            var rectCenter = new Point(
                currentRect.X + currentRect.Width / 2,
                currentRect.Y + currentRect.Height / 2);
            var squareDistanceToCenter = (rectCenter.X - center.X) * (rectCenter.X - center.X) +
                                         (rectCenter.Y - center.Y) * (rectCenter.Y - center.Y);
            return squareDistanceToCenter;
        }

        private IEnumerable<Rectangle> GetAdjacentRectangles(Rectangle rect)
        {
            yield return new Rectangle(new Point(rect.X - 1, rect.Y), rect.Size);
            yield return new Rectangle(new Point(rect.X + 1, rect.Y), rect.Size);
            yield return new Rectangle(new Point(rect.X, rect.Y - 1), rect.Size);
            yield return new Rectangle(new Point(rect.X, rect.Y + 1), rect.Size);
        }
    }
}