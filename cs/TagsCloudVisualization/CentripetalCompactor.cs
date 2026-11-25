using System.Drawing;

public sealed class CentripetalCompactor 
{
    private readonly double step;       
    private readonly int maxIterations; // страховка от бесконечного цикла

    public CentripetalCompactor(double step = 0.5, int maxIterations = 10_000)
    {
        if (step <= 0) throw new ArgumentOutOfRangeException(nameof(step));
        if (maxIterations <= 0) throw new ArgumentOutOfRangeException(nameof(maxIterations));
        step = step;
        maxIterations = maxIterations;
    }

    public Rectangle Compact(Rectangle candidate, IReadOnlyList<Rectangle> placed, Point center)
    {
        throw new NotImplementedException();
    }
}