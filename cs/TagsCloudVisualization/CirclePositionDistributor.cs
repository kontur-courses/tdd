using System;
using System.Drawing;

namespace TagsCloudVisualization;

public class CirclePositionDistributor
{
    public Point Center { get; }
    public int Radius { get; private set; }

    private const int DeltaRadius = 5;
    private int currentPositionDegrees;


    public  CirclePositionDistributor(Point center)
    {
        Center = center;
        Radius = 5;
    } 

    public Point GetNextPosition()
    {
        currentPositionDegrees += 1;
        if (currentPositionDegrees > 360)
        {
            currentPositionDegrees = 0;
            Radius += DeltaRadius;
        }
        var positionAngleInRadians = currentPositionDegrees * Math.PI / 180.0;
        return new Point(
            Center.X + (int)Math.Ceiling(Radius * Math.Cos(positionAngleInRadians)), 
            Center.Y + (int)Math.Ceiling(Radius * Math.Sin(positionAngleInRadians)));
    }
}