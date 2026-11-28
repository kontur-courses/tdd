using System.Drawing;

namespace TagsCloudVisualization;

public class ArchimedeanSpiral : ICenterPointProvider
{
    private readonly double spiralAngleStepRadians;
    private readonly double spiralRadiusCoefficient;
    private readonly double startRadius;
    private readonly Point center;

    private double currentAngle = 0.0;

    public ArchimedeanSpiral(
        double spiralAngleStepRadians,
        double spiralRadiusCoefficient,
        double startRadius,
        Point center)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(spiralAngleStepRadians);
        ArgumentOutOfRangeException.ThrowIfNegative(spiralRadiusCoefficient);
        ArgumentOutOfRangeException.ThrowIfNegative(startRadius);

        this.spiralAngleStepRadians = spiralAngleStepRadians;
        this.spiralRadiusCoefficient = spiralRadiusCoefficient;
        this.startRadius = startRadius;
        this.center = center;
    }

    public Point GetNextCenterPosition()
    {
        var currentRadius = startRadius + spiralRadiusCoefficient * currentAngle;
        
        var x = currentRadius * Math.Cos(currentAngle);
        var y = currentRadius * Math.Sin(currentAngle);

        currentAngle += spiralAngleStepRadians;
        
        return new Point((int)x + center.X, (int)y + center.Y);
    }
}