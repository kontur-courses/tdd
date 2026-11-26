using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization;

public class UnwindingSpiral
{
    private const double DefaultDistanceBetweenTurns = 1;
    private const double DefaultAngleStep = 0.1;

    private readonly double distanceBetweenTurns;
    private readonly double angleStep;

    public UnwindingSpiral(
        double distanceBetweenTurns = DefaultDistanceBetweenTurns,
        double angleStep = DefaultAngleStep)
    {
        ValidatePositive(distanceBetweenTurns, nameof(distanceBetweenTurns));
        ValidatePositive(angleStep, nameof(angleStep));
        this.distanceBetweenTurns = distanceBetweenTurns;
        this.angleStep = angleStep;
    }

    public IEnumerable<Point> GeneratePoints(Point center)
    {
        throw new NotImplementedException();
    }

    private static void ValidatePositive(double value, string parameterName)
    {
        throw new NotImplementedException();
    }
}
