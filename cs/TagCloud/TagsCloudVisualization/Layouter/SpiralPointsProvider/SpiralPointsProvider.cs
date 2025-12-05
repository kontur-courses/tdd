using System.Drawing;

namespace TagsCloudVisualization;

public class SpiralPointsProvider : ISpiralPointsProvider
{
    private readonly Point _center;
    private readonly double _angleStep;
    private readonly double _radiusStepFactor;
    
    private double _currentRadius;
    private double _currentAngle;

    public SpiralPointsProvider(Point center, double angleStep = 0.05, double radiusStepFactor = 0.05)
    {
        _center = center;
        _angleStep = angleStep;
        _radiusStepFactor = radiusStepFactor;
        _currentRadius = 0;
        _currentAngle = 0;
    }

    public IEnumerable<Point> GetSpiralPoints(int minDimension)
    {
        while (true)
        {
            var point = PolarMath.PolarToCartesian(_currentRadius, _currentAngle, _center);
            
            _currentAngle += _angleStep;
            _currentRadius += minDimension * _radiusStepFactor;

            yield return point;
        }
    }
}