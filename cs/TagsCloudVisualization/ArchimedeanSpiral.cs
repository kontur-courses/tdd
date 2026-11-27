using System.Drawing;

namespace TagsCloudVisualization;

public class ArchimedeanSpiral
{
    private readonly Point center;
    private readonly double spiralStep;
    private readonly double pointSpacing;

    public ArchimedeanSpiral(Point center, double spiralStep = 5, double pointSpacing = 2)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(spiralStep);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pointSpacing);

        this.center = center;
        this.spiralStep = spiralStep;
        this.pointSpacing = pointSpacing;
    }

    /// <summary>
    /// Returns an infinite sequence of points along the spiral, starting at the center.
    /// </summary>
    /// <remarks>
    /// The sequence never ends, so limit it with <c>Take</c>, <c>First</c> or <c>FirstOrDefault</c>.
    /// Calling <c>ToList</c>, <c>Count</c> or <c>Last</c> will hang.
    /// </remarks>
    public IEnumerable<Point> GetPoints()
    {
        var angle = 0.0;
        Point? previousPoint = null;

        while (true)
        {
            var radius = spiralStep * angle;

            var point = new Point(
                (int)Math.Round(center.X + radius * Math.Cos(angle)),
                (int)Math.Round(center.Y + radius * Math.Sin(angle)));

            if (point != previousPoint)
            {
                previousPoint = point;
                yield return point;
            }

            angle += pointSpacing / Math.Sqrt(
                radius * radius + spiralStep * spiralStep);
        }
    }
}