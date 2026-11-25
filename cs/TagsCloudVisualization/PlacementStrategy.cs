using System.Drawing;

public sealed class PlacementStrategy 
{
    private readonly int maxSpiralSteps;

    public PlacementStrategy(int maxSpiralSteps = 100_000)
    {
        if (maxSpiralSteps <= 0) throw new ArgumentOutOfRangeException(nameof(maxSpiralSteps));
        this.maxSpiralSteps = maxSpiralSteps;
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