using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization;

public class CircularCloudLayouter
{
    public readonly List<Rectangle> rectangles;
    private Spiral spiral;

    public CircularCloudLayouter(Point center)
    {
        rectangles = new List<Rectangle>();
        spiral = new Spiral(center);
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Height < 0 || rectangleSize.Width < 0)
            throw new ArgumentException("Wrong size of rectangle");
        var rect = spiral.GetPoints()
            .Select(point =>
            {
                var coordinatesOfRectangle = CalculateRectangleCoordinates(point, rectangleSize);
                return new Rectangle(coordinatesOfRectangle, rectangleSize);
            })
            .First(rectangle => !IntersectsWithOtherRectangles(rectangle));
        rectangles.Add(rect);
        return rect;
    }

    private Point CalculateRectangleCoordinates(Point rectangleCenter, Size rectangleSize)
    {
        return new Point(rectangleCenter.X - rectangleSize.Width / 2, rectangleCenter.Y - rectangleSize.Height / 2);
    }

    private bool IntersectsWithOtherRectangles(Rectangle rectangle)
    {
        return rectangles.Any(r => r.IntersectsWith(rectangle));
    }
}