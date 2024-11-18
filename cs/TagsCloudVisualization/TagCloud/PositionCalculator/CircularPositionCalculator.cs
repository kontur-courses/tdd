using System.Drawing;

namespace TagsCloudVisualization.TagCloud;

public class CircularPositionCalculator(Point center, double offsetDelta = 2.0, double angleDelta = 0.1) : IPositionCalculator
{
    private double currentAngle = 0.0;
    private double currentOffset = 0.0;
    private const double fullRoundAngle = Math.PI * 2;

    public IEnumerable<Rectangle> CalculateNextPosition (Size nextRectangleSize)
    {
        while (true)
        {
            var newRectangle = MakeRectangleFromSize(nextRectangleSize);
            currentAngle += angleDelta;
            if (currentAngle >= fullRoundAngle)
            {
                currentAngle = 0;
                currentOffset += offsetDelta;
            }

            yield return newRectangle;
        }
    }

    private Rectangle MakeRectangleFromSize(Size nextRectangleSize)
    {
        var x = (int)(center.X + currentOffset * Math.Cos(currentAngle)); 
        var y = (int)(center.Y + currentOffset * Math.Sin(currentAngle));
        var newRectangle = new Rectangle(new Point(x, y), nextRectangleSize);
        return newRectangle;
    }

    public bool IsRectanglePositionValid(List<Rectangle> rectangles, Rectangle currentRectangle)
    {
        return !rectangles.Any(r => r.IntersectsWith(currentRectangle));
    }
}