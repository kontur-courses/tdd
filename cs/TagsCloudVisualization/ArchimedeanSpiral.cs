using System.Drawing;

namespace TagsCloudVisualization;

public class ArchimedeanSpiral
{
    private readonly Point center;
    private readonly double spiralStep;

    public ArchimedeanSpiral(Point center, double spiralStep = 5)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(spiralStep);

        this.center = center;
        this.spiralStep = spiralStep;
    }

    /// <summary>
    /// Returns an infinite sequence of points along the spiral, starting at the center.
    /// </summary>
    /// <remarks>
    /// The sequence never ends, so limit it with <c>Take</c>, <c>First</c> or <c>FirstOrDefault</c>.
    /// Calling <c>ToList</c>, <c>Count</c> or <c>Last</c> will hang.
    /// </remarks>
    public IEnumerable<Point> GetSpiralPoints()
    {
        var angle = 0D;

        while (true)
        {
            var r = spiralStep * angle;
            var x = center.X + r * Math.Cos(angle);
            var y = center.Y + r * Math.Sin(angle);

            yield return new Point((int)Math.Round(x), (int)Math.Round(y));

            angle += 1 / Math.Max(r, 1);
        }
    }
}