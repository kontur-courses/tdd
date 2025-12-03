using System.Drawing;
using System.Collections.Generic;
using System;
using TagsCloudVisualization.Interface;

namespace TagsCloudVisualization;

public class SpiralPointGenerator : IPointGenerator
{
    private readonly Point center;
    private readonly double density;
    private double angle = 0;
    private readonly double angleStep;
    
    public Point Center => center;

    public SpiralPointGenerator(Point center, double density, double stepAngle)
    {
        this.center = center;
        this.density = density;
        this.angleStep = stepAngle;
    }
    
    public IEnumerable<Point> GeneratePoints()
    {
        while (true)
        {
            var radius = density * angle;
            var x = (int)Math.Round(center.X + radius * Math.Cos(angle));
            var y = (int)Math.Round(center.Y + radius * Math.Sin(angle));
            
            angle += angleStep; 

            yield return new Point(x, y);
        }
    }
}