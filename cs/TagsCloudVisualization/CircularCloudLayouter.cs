using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private Point center;
    private double radius;
    private Rectangle buffer;
    public List<Rectangle> rectangles = new List<Rectangle>();
    public CircularCloudLayouter(Point center)
    {
        if (center.X <= 0 || center.Y <= 0)
            throw new ArgumentException("X and Y much be positive");
        this.center = center;
        radius = Math.Min(center.X, center.Y);
    }
    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
            throw new ArgumentException("Height and Width much be positive");
        int angel = 0;
        int x = 0;
        int y = 0;
        while (GetDiagonal(x, y) < radius)
        {
            var centerRect = new Point(x, y);
            var xRect = x - (int)(rectangleSize.Width / 2);
            var yRect = y - (int)(rectangleSize.Height / 2);
            buffer = new Rectangle(xRect, yRect, rectangleSize.Width, rectangleSize.Height);
            var stepLength = GetStepLengthToSpiral(10);
            angel += 10;
            x = (int)(angel * Math.Cos(angel) * stepLength);
            y = (int)(angel * Math.Sin(angel) * stepLength);
            if(rectangles.Count == 0) break;
            if (CheckIntersect(buffer) && CheckInsideRectangles(centerRect) && rectangles.Count > 0)
            {
                var horizontalOffset = true;
                var verticalOffset = true;
                int count = 0;
                while ((horizontalOffset || verticalOffset) && count < 100)
                {
                    horizontalOffset = buffer.GetCenter().X < 0
                        ? TryToOffset(1, 0)
                        : TryToOffset(-1, 0);
                    verticalOffset = buffer.GetCenter().Y < 0
                        ? TryToOffset(0, 1)
                        : TryToOffset(0, -1);
                    count += 1;
                }
                break;
            }
        }

        if (GetDiagonal(x, y) > radius ||
            GetDiagonal(rectangleSize.Height, rectangleSize.Width) > 2 * radius)
            throw new ArgumentException("Rectangle size so much");
        rectangles.Add(buffer);
        return buffer;
    }

    private bool TryToOffset(int x, int y)
    {
        buffer.Offset(x, y);
        if (rectangles.Any(x => x.IntersectsWith(buffer)))
        {
            buffer.Offset(-x, -y);
            return false;
        }
        return true;
    }
    private int GetStepLengthToSpiral(int stepLength)
    {
        return (int)(stepLength / (2 * Math.PI));
    }
    private bool CheckInsideRectangles(Point point)
    {
        return !rectangles.Any(x => x.Contains(point));
    }
    private bool CheckIntersect(Rectangle rect)
    {
        return !rectangles.Any(x => x.IntersectsWith(rect));
    }
    private double GetDiagonal(int x, int y)
    {
        return Math.Sqrt(x * x + y * y);
    }
}