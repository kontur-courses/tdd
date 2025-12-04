using System.Drawing;

namespace TagCloud.SpiralGenerators;
public class ArchimedeanSpiralPointGenerator(
    Point center, 
    double angleStep = 0.1, 
    double radiusStep = 0.5) 
    : ISpiralPointGenerator
{
    private double _angle;
    
    public Point GetNextPointOnSpiral()
    {
        var radius = radiusStep * _angle;
        var x = center.X + (int)Math.Round(radius * Math.Cos(_angle));
        var y = center.Y + (int)Math.Round(radius * Math.Sin(_angle));
        _angle += angleStep;
        return new Point(x, y);
    }
}