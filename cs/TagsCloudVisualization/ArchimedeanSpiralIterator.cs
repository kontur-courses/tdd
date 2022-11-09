using System.Drawing;

namespace TagsCloudVisualization;

public class ArchimedeanSpiralIterator : ISpiralIterator
{
    private int _currentAngle;
    private int _step;
    private readonly ISpiral _spiral;

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

        _spiral = spiral;
        _currentAngle = startAngle;
        _step = step;
    }

    public Point Next()
    {
        var nextPoint = _spiral.GetCartesianPoint(_currentAngle);
        _currentAngle += _step;
        return nextPoint;
    }
}