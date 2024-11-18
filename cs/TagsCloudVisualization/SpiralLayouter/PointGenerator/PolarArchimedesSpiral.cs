using System.Collections;
using System.Drawing;

namespace TagsCloudVisualization.SpiralLayouter.PointGenerator;

public class PolarArchimedesSpiral : IPointGenerator
{
    public double Radius { get; }
    public double AngleOffset { get; }
    

    public PolarArchimedesSpiral(double radius, double angleOffset)
    {
        if (radius <= 0 || angleOffset <= 0)
        {
            var argName = radius <= 0 ? nameof(radius) : nameof(angleOffset);
            throw new ArgumentException("Spiral params should be positive.", argName);
        }
        
        Radius = radius;
        AngleOffset = angleOffset * Math.PI / 180;
    }

    public IEnumerable<Point> StartFrom(Point startPoint)
    {
        var currentAngle = 0.0;
        while (true)
        {
            var polarCoordinate = Radius / (2 * Math.PI) * currentAngle;
            
            var xOffset = (int)Math.Round(polarCoordinate * Math.Cos(currentAngle));
            var yOffset = (int)Math.Round(polarCoordinate * Math.Sin(currentAngle));
            
            yield return startPoint + new Size(xOffset, yOffset);
            
            currentAngle += AngleOffset;
        }
    }
}