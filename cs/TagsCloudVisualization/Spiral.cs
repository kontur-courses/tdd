using System.Drawing;

namespace TagsCloudVisualization;

public class Spiral
{
    private double angle;
    private readonly Point _pastPoint;
    private readonly double _step;
    
    public Spiral(Point start, double step)
    {
        _pastPoint = start;
        _step = step;
    }
    
    public Point NextPoint()
    {
        var newX = (int)(_pastPoint.X + _step * angle * Math.Cos(angle));
        var newY = (int)(_pastPoint.Y + _step * angle * Math.Sin(angle));
        angle += Math.PI / 50;
        
        return new Point(newX, newY);
    }
}