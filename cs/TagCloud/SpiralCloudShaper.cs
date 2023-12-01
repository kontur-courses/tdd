using System.Drawing;

namespace TagCloud;

public class SpiralCloudShaper : ICloudShaper
{
    private Point center;
    private double currentAngle;
    private double coefficient;
    private double deltaAngle;

    public double Radius => coefficient * currentAngle;
    
    public SpiralCloudShaper(Point center, double coefficient = 1, double deltaAngle = 0.1)
    {
        this.center = center;
        this.coefficient = coefficient;
        this.deltaAngle = deltaAngle;
        currentAngle = 0;
    }
    public Rectangle GetNextPossibleRectangle(Size size)
    {
        currentAngle += deltaAngle;
        var position = new Point(
            center.X + (int)(Radius * Math.Cos(currentAngle)), 
            center.Y + (int)(Radius * Math.Sin(currentAngle))
        );
        return new Rectangle(position, size);
    }
}