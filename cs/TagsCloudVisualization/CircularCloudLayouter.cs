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
        int x = 0;
        int y = 0;
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
            ;
            if (!isIntersect && !isContain)
            {
                CorrectPosition(rectangle, center, rectangles);
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

    private void CorrectPosition(Rectangle buffer, Point center, List<Rectangle> rectangles)
    {
        var horizontalOffset = true;
        var verticalOffset = true;
        int count = 0;
        while ((horizontalOffset || verticalOffset) && count < 1000)
        {
            horizontalOffset = buffer.GetCenter().X < center.X
                ? TryToOffset(1, 0, buffer, rectangles)
                : TryToOffset(-1, 0, buffer, rectangles);
            verticalOffset = buffer.GetCenter().Y < center.Y
                ? TryToOffset(0, 1, buffer, rectangles)
                : TryToOffset(0, -1, buffer, rectangles);
            count += 1;
        }
    }

    private bool TryToOffset(int x, int y, Rectangle buffer, List<Rectangle> rectangles)
    {
        buffer.Offset(x, y);
        if (rectangles.Any(x => x.IntersectsWith(buffer)))
        {
            buffer.Offset(-x, -y);

            return false;
        }

        return true;
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