using System.Drawing;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization.TagsCloudDistributions;

public class SpiralDistribution : ICloudDistribution
{
    private const double AngleStep = 0.02;
    private const double RadiusStep = 0.01;
    private double angle;
    private double radius;
    private readonly Point cloudCenter = new();
    
    public Point GetNextPoint()
    {
        var nextPoint = ConvertPolarToCartesian();
        angle += AngleStep;
        radius += RadiusStep;
        return nextPoint;
    }
    
    public Point GetCenter() => cloudCenter;

    private Point ConvertPolarToCartesian()
    {
        var cartesian = new Point((int)(radius * Math.Cos(angle)), (int)(radius * Math.Sin(angle)));
        cartesian.Offset(cloudCenter);
        return cartesian;
    }
}