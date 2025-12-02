using System.Drawing;

namespace TagCloud;

public class Spiral(Point center, double angleIncrement = 0.005)
{
    private const double RadiusPerRadian = 0.5;
    private double currentAngle;
    private int placementFailureStreak;
    private int currentAngleStepMultiplier = 1;
    
    private const int MaxAngleStepMultiplier = 1000;

    public Point GetNextPoint()
    {
        placementFailureStreak = 0;
        currentAngleStepMultiplier = 1;
        return AdvanceByCurrentStep();
    }
    
    public Point RetryGetNextPoint()
    {
        placementFailureStreak++;

        currentAngleStepMultiplier = Math.Min(1 << Math.Min(placementFailureStreak, 10), MaxAngleStepMultiplier);

        return AdvanceByCurrentStep();
    }

    private Point AdvanceByCurrentStep()
    {
        var deltaAngle = angleIncrement * currentAngleStepMultiplier;

        currentAngle += deltaAngle;
        var radius = RadiusPerRadian * currentAngle;

        var x = center.X + (int)Math.Round(radius * Math.Cos(currentAngle));
        var y = center.Y + (int)Math.Round(radius * Math.Sin(currentAngle));

        return new Point(x, y);
    }
}
