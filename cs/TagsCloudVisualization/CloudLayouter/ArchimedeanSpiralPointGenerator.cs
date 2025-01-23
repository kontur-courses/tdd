using SixLabors.ImageSharp;

namespace TagsCloudVisualization.CloudLayouter;

public class ArchimedeanSpiralPointGenerator
{
    private readonly PointF center;
    private readonly double spiralStep;
    private readonly double distanceBetweenTurns;
    private double angle;

    public ArchimedeanSpiralPointGenerator(PointF center, double spiralStep = 0.01, double distanceBetweenTurns = 0.01)
    {
        this.center = center;
        this.spiralStep = spiralStep;
        this.distanceBetweenTurns = distanceBetweenTurns;
        angle = 0;
    }

    public PointF GetNextPoint()
    {
        var radius = distanceBetweenTurns * angle;
        var x = (float)(center.X + radius * Math.Cos(angle));
        var y = (float)(center.Y + radius * Math.Sin(angle));
        angle += spiralStep;
        return new PointF(x, y);
    }
}