using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class SpiralCloudLayouter : ICircularCloudLayouter
    {
        private readonly Point center;
        private readonly ISpiral spiral;
        private readonly List<Rectangle> rectangles;

        public SpiralCloudLayouter(Point center, ISpiral spiral)
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

        private Point OffsetPointToRectangleCenter(Point point, Size rectangleSize)
        {
            point.Offset(center);
            point.Offset(-rectangleSize.Width / 2, -rectangleSize.Height / 2);
            return point;
        }

        private Rectangle GetPossibleRectangle(Size rectangleSize)
        {
            return new Rectangle(
                OffsetPointToRectangleCenter(Point.Round(spiral.GetNextPoint()), rectangleSize),
                rectangleSize);
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
                var squareDistanceToCenter = RectangleUtils.GetSquareDistanceToPoint(currentRect, center);
                if (squareDistanceToCenter >= minDistanceSquare || IsIntersectsWithOthersRectangles(currentRect))
                    continue;
                minDistanceSquare = squareDistanceToCenter;
                shiftedRect = currentRect;
                foreach (var adjacentRectangle in RectangleUtils.GetAdjacentRectangles(currentRect))
                    queue.Enqueue(adjacentRectangle);
            }

            return shiftedRect;
        }
    }
}