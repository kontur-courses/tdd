using System.Drawing;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization;

public class ArchimedeanSpiralIterator : ISpiralIterator
{
    private int currentAngle;
    private readonly int step;
    private readonly ISpiral spiral;

    public ArchimedeanSpiralIterator(ISpiral spiral, int startAngle = 0, int step = 1)
    {
        if (startAngle < 0)
        {
            throw new ArgumentException("Angle should be not negative", nameof(startAngle));
        }

        if (step <= 0)
        {
            throw new ArgumentException("Step should be positive", nameof(step));
        }

        this.spiral = spiral;
        currentAngle = startAngle;
        this.step = step;
    }

    public Point Next()
    {
        var nextPoint = spiral.GetCartesianPoint(currentAngle);
        currentAngle += step;
        return nextPoint;
    }
}