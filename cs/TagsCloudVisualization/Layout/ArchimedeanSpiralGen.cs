using System.Drawing;

namespace TagsCloudVisualization.Layout;

public class ArchimedeanSpiralGen : ISpiralGen
{
    public Point Center { get; }

    public double SpiralStep { get; }

    public double AngleStep { get; }

    public ArchimedeanSpiralGen(Point center, double spiralStep = 0.5, double angleStep = 0.05)
    {
        Center = center;
        SpiralStep = spiralStep;
        AngleStep = angleStep;
    }

    public IEnumerable<Point> GetPoints()
    {
        throw new NotImplementedException();
    }
}
