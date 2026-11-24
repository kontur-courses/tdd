using System.Drawing;

public sealed class NonOverlappingPlacementStrategy 
{
    private readonly int _maxSpiralSteps;

    public NonOverlappingPlacementStrategy(int maxSpiralSteps = 100_000)
    {
        if (maxSpiralSteps <= 0) throw new ArgumentOutOfRangeException(nameof(maxSpiralSteps));
        _maxSpiralSteps = maxSpiralSteps;
    }

    public Rectangle FindPosition(
        Size size,
        List<Rectangle> placed,
        ArchimedeanSpiral spiral,
        Point center)
    {
        throw new NotImplementedException();
    }
}