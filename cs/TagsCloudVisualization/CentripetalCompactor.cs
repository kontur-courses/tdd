using System.Drawing;
using TagsCloudVisualization.Abstractions;

public sealed class CentripetalCompactor : ICentripetalCompactor
{
    private readonly double step;       
    private readonly int maxIterations; 
    public CentripetalCompactor(double step = 0.5, int maxIterations = 10_000)
    {
        if (step <= 0) throw new ArgumentOutOfRangeException(nameof(step));
        if (maxIterations <= 0) throw new ArgumentOutOfRangeException(nameof(maxIterations));
        step = step;
        maxIterations = maxIterations;
    }

    public Rectangle Compact(Rectangle candidate,
        List<Rectangle> placed,
        Point center)
    {
        throw new NotImplementedException();
    }
}