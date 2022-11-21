namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    public Rectangle PutNextRectangle(List<Rectangle> rectangles, Size rectangleSize)
    {
        if (rectangles.Count == 0)
            throw new ArgumentException("Rectangles list should be not empty");
        if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
            throw new ArgumentException("Height and Width much be positive");
        Rectangle newRectangle;
        if (!TryToPutRectangleInsideSpiral(rectangles, rectangleSize, out newRectangle))
            throw new ArgumentException("Rectangle size so much");
        rectangles.Add(newRectangle);
        return newRectangle;
    }

    private bool TryToPutRectangleInsideSpiral(List<Rectangle> rectangles, Size rectangleSize,
        out Rectangle newRectangle)
    {
        var center = rectangles[0].GetCenter();
        var x = 0;
        var y = 0;
        double angel = 0;
        var rectPos = new Point(x - rectangleSize.Width / 2 + center.X,
            y - rectangleSize.Height / 2 + center.Y);
        var rectangle = new Rectangle(rectPos, rectangleSize);
        newRectangle = rectangle;
        var stepLength = (int)(7 / (2 * Math.PI));
        while (CheckIntersectsWithCircle(center, rectangle))
        {
            newRectangle = rectangle;
            if (rectangles.Count == 0) return true;
            var isContain = rectangles.Any(x => x.Contains(rectangle.GetCenter()));
            var isIntersect = rectangles.Any(x => x.IntersectsWith(rectangle));
            if (!isIntersect && !isContain)
            {
                newRectangle = CorrectPosition(rectangle, center, rectangles);
                return true;
            }

            angel += Math.PI / 180;
            x = (int)(angel * Math.Cos(angel) * stepLength);
            y = (int)(angel * Math.Sin(angel) * stepLength);
            rectPos = new Point(x - rectangleSize.Width / 2 + center.X,
                y - rectangleSize.Height / 2 + center.Y);
            rectangle = new Rectangle(rectPos, rectangleSize);
        }

        return false;
    }

    private Rectangle CorrectPosition(Rectangle rectangle, Point center, List<Rectangle> rectangles)
    {
        var rect = rectangle;
        rect = BinSearchHorizontal(rect, center, rectangles);
        rect = BinSearchVertical(rect, center, rectangles);
        return rect;
    }

    private Rectangle BinSearchHorizontal(Rectangle rectangle, Point center, List<Rectangle> rectangles)
    {
        var requestedRect = rectangle;
        var maxHorizontal = Math.Max(center.X, rectangle.GetCenter().X);
        var minHorizontal = Math.Min(center.X, rectangle.GetCenter().X);
        while (maxHorizontal >= minHorizontal)
        {
            var mid = (maxHorizontal + minHorizontal) / 2;
            var left = mid - (rectangle.Width / 2);
            if (CheckIntersectAfterMove(left, rectangle.X, rectangle, rectangles))
            {
                requestedRect.MoveTo(left, rectangle.Y);
                if (maxHorizontal > center.X) maxHorizontal = mid - 1;
                else minHorizontal = mid + 1;
            }
            else
            {
                if (maxHorizontal < center.X) maxHorizontal = mid - 1;
                else minHorizontal = mid + 1;
            }
        }

        return requestedRect;
    }
    private Rectangle BinSearchVertical(Rectangle rectangle, Point center, List<Rectangle> rectangles)
    {
        var requestedRect = rectangle;
        var maxVertical = Math.Max(center.Y, rectangle.GetCenter().Y);
        var minVertical = Math.Min(center.Y, rectangle.GetCenter().Y);
        while (maxVertical >= minVertical)
        {
            var mid = (maxVertical + minVertical) / 2;
            var top = mid - (rectangle.Height / 2);
            if (CheckIntersectAfterMove(rectangle.X, top, requestedRect, rectangles))
            {
                requestedRect.MoveTo(rectangle.X, top);
                if (maxVertical > center.Y) maxVertical = mid - 1;
                else minVertical = mid + 1;
            }
            else
            {
                if (maxVertical < center.Y) maxVertical = mid - 1;
                else minVertical = mid + 1;
            }
        }

        return requestedRect;
    }

    private bool CheckIntersectAfterMove(int x, int y, Rectangle buffer, List<Rectangle> rectangles)
    {
        var rect = buffer;
        rect.MoveTo(x, y);
        return !rectangles.Any(r => r.IntersectsWith(rect));
    }

    private bool CheckIntersectsWithCircle(Point center, Rectangle rect)
    {
        int radius = Math.Min(center.X, center.Y);
        var horizontalCoordinate = new int[] { rect.X, rect.Right };
        var verticalCoordinate = new int[] { rect.Y, rect.Bottom };
        for (var i = 0; i < 4; i++)
        {
            var x = horizontalCoordinate[i % 2] - center.X;
            var y = verticalCoordinate[i / 2] - center.Y;
            if (Math.Sqrt(x * x + y * y) >= radius)
                return false;
        }

        return true;
    }
}