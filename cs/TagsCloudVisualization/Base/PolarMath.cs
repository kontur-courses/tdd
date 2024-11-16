using System.Drawing;

namespace TagsCloudVisualization.Base;

public static class PolarMath
{
    public static double ConvertToRadians(double degrees) =>
        degrees * Math.PI / 180;

    public static double ConvertToDegrees(double radians) =>
        radians * 180 / Math.PI;

    public static double GetSquareOfCircle(double radius) => 
        Math.PI * radius * radius;
    
    public static int GetSectorOfCircleFromRadians(double radians) 
        => (int)((radians % Math.Tau) / (Math.PI / 2) + 1);
    
    public static int GetSectorOfCircleFromDegrees(double degrees) 
        => (int)((degrees % 360) / 90 + 1);
    
    public static double GetOffsetPerRadianForArchimedeanSpiral(double radius) =>
        radius / (2 * Math.PI);

    public static Point ConvertToCartesianCoordinateSystem(double polarRadius, double polarAngle)
    {
        var x = (int)Math.Round(polarRadius * Math.Cos(polarAngle));
        var y = (int)Math.Round(polarRadius * Math.Sin(polarAngle));
        
        return new Point(x, y);
    }
    
    public static (double polarRadius, double polarAngle) ConvertToPolarCoordinateSystem(Point point)
    {
        var polarRadius = Math.Sqrt(point.X * point.X + point.Y * point.Y);
        var polarAngle = Math.Atan2(point.Y, point.X);
        polarAngle = polarAngle < 0 ? 2 * Math.PI + polarAngle : polarAngle;
        
        return (polarRadius, polarAngle);
    }
}