using System.Drawing;

namespace TagCloud;

public class SpiralCloudShaper : ICloudShaper
{
    private Point center;
    private Point previousPoint;
    
    private double currentAngle;
    private double coefficient;
    private double deltaAngle;

    public double Radius => coefficient * currentAngle;
    
    public SpiralCloudShaper(Point center, double coefficient = 1, double deltaAngle = 0.1)
    {
        this.center = center;
        if (coefficient <= 0)
            throw new ArgumentException("Spiral coefficient must be positive number");
        this.coefficient = coefficient;
        
        if (deltaAngle <= 0)
            throw new ArgumentException("Spiral delta angle must be positive number");
        this.deltaAngle = deltaAngle;
        
        currentAngle = 0;
        previousPoint = center;
    }
    public Point GetNextPossiblePoint()
    {
        currentAngle += deltaAngle;
        var position = CalculatePointByCurrentAngle();
        while (position == previousPoint)
        {
            currentAngle += deltaAngle;
            position = CalculatePointByCurrentAngle();
        }
        return position;
    }

    private Point CalculatePointByCurrentAngle()
    {
        return new Point(
            center.X + (int)(Radius * Math.Cos(currentAngle)), 
            center.Y + (int)(Radius * Math.Sin(currentAngle))
        );
    }
}