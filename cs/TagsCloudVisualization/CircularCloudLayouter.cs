using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

internal class CircularCloudLayouter
{
    private readonly List<Rectangle> rectangles;
    private readonly Point center;

    public CircularCloudLayouter(Point center)
    {
        rectangles = new List<Rectangle>();
        this.center = center;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Height < 0 || rectangleSize.Width < 0) throw new ArgumentException();
        foreach (var point in GetNextSpiralPoint(0.1, 0.001))
        {
            var coordinatesOfRectangle = FindCoordinatesOfRectangle(point, rectangleSize);
            var rectangle = new Rectangle(coordinatesOfRectangle, rectangleSize);
            if (IntersectsWithOtherRectangles(rectangle)) continue;
            rectangles.Add(rectangle);
            return rectangle;
        }

        throw new Exception();
    }

    private Point FindCoordinatesOfRectangle(Point point, Size size)
    {
        return new Point(point.X - size.Width / 2, point.Y - size.Height / 2);
    }

    private bool IntersectsWithOtherRectangles(Rectangle rectangle)
    {
        return rectangles.Where(r => r.IntersectsWith(rectangle)).ToList().Count != 0;
    }

    public IEnumerable<Point> GetNextSpiralPoint(double angleOffset, double radiusOffset)
    {
        double angle = 0;

        double radius = 0;
        if (angle == 0 && radius == 0) yield return center;
        while (true)
        {
            yield return new Point(center.X + (int) Math.Round(radius * Math.Cos(angle)),
                center.Y + (int) Math.Round(radius * Math.Sin(angle)));
            radius += radiusOffset;
            angle += angleOffset;
        }
    }
}