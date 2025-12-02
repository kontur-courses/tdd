using System.Drawing;

namespace TagsCloudVisualization;

// Function: r=aθ
public class ArchimedeanSpiral : ISpiral
{
    private const double defaultStep = Math.PI / 4;
    private const double defaultParameterA = 1;
    private IEnumerator<Point> enumerator;

    /// <param name="parameterA">The larger this parameter, the greater the distance between the turns of the spiral, and vice versa.</param>
    public ArchimedeanSpiral(Point center = default, double step = defaultStep, double parameterA = defaultParameterA)
    {
        enumerator = GenerateArchimedeanSpiral(center, step, parameterA).GetEnumerator();
    }

    public Point GetNextPoint()
    {
        enumerator.MoveNext();
        return enumerator.Current;
    }
    
    private static IEnumerable<Point> GenerateArchimedeanSpiral(Point center = default, double step = defaultStep, double parameterA = defaultParameterA)
    {
        var theta = 0d;
        while (true)
        {
            var r = parameterA * theta;
            var x = (int)Math.Round(center.X + r * Math.Cos(theta));
            var y = (int)Math.Round(center.Y + r * Math.Sin(theta));
            yield return new Point(x, y);
            theta += step;
        }
        // ReSharper disable once IteratorNeverReturns
    }
}