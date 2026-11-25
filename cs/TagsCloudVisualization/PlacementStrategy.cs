using System.Drawing;
using TagsCloudVisualization.Abstractions;

public sealed class PlacementStrategy : IRectanglePlacementStrategy
{
    private readonly int maxSpiralSteps;

    public PlacementStrategy(int maxSpiralSteps = 100_000)
    {
        if (maxSpiralSteps <= 0) throw new ArgumentOutOfRangeException(nameof(maxSpiralSteps));
        this.maxSpiralSteps = maxSpiralSteps;
    }

    Rectangle FindPosition(
        Size size,
        List<Rectangle> placed,
        ISpiral spiral,
        Point center)
    {
        throw new NotImplementedException();
    }

    //Rectangle IRectanglePlacementStrategy.FindPosition(Size size, List<Rectangle> placed, ISpiral spiral, Point center)
    //{
    //     throw new NotImplementedException();
    //}
}