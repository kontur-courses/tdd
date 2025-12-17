using System.Drawing;

namespace TagCloudVisualization;

public class Spiral
{
    private readonly Point center;
    private double angle;
    private readonly double step;

    public Spiral(Point center, double step)
    {
        if (step <= 0)
            throw new ArgumentException("Step must be positive", nameof(step));
        
        this.center = center;
        this.step = step;
        angle = 0;
    }

    public IEnumerable<Point> GetPoints()
    {
        while (true)
        {
            var radius = step * angle;
            var x = center.X + (int)Math.Round(radius * Math.Cos(angle));
            var y = center.Y + (int)Math.Round(radius * Math.Sin(angle));
            
            yield return new Point(x, y);
            
            angle += step;
        }
    }
}