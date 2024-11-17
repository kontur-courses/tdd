using System.Collections.ObjectModel;
using System.Drawing;

namespace TagsCloudVisualization.Layouters;

public class CircularCloudLayouter(Point center = new()) : ICloudLayouter
{
    private double angle;
    private double radius;
    private readonly List<Rectangle> rectangles = [];
    private const double AngleStep = 0.1;
    private const double RadiusStep = 0.1;

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Height < 0 || rectangleSize.Width < 0)
        {
            throw new ArgumentOutOfRangeException($"{rectangleSize} меньше 0");
        }

        var rectangle = new Rectangle(FindNextLocation(rectangleSize), rectangleSize);

        rectangles.Add(rectangle);

        return rectangle;
    }

    private Point FindNextLocation(Size rectangleSize)
    {
        var location = center;
        Rectangle guessRectangle;

        if (rectangles.Count == 0) return location;

        do
        {
            location = GetPointOnSpiral();
            guessRectangle = new Rectangle(location, rectangleSize);
            radius += RadiusStep;
            angle += AngleStep;
        } while (rectangles.Any(rect => rect.IntersectsWith(guessRectangle)));

        return location;
    }

    private Point GetPointOnSpiral()
    {
        var (sin, cos) = Math.SinCos(angle);
        var x = (int)(radius * cos) + center.X;
        var y = (int)(radius * sin) + center.Y;
        return new Point(x, y);
    }
}