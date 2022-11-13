using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization;

public class CircularCloudLayouter
{
    //Попало много лишних файлов, давай их уберем путем добавления в gitignore!!!!!!!
    public List<Rectangle> Rectangles { get; }
    public Spiral Spiral { get; }

    public CircularCloudLayouter(Point center, Spiral spiral)
    {
        Rectangles = new List<Rectangle>();
        Spiral = spiral;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Height < 0 || rectangleSize.Width < 0)
            throw new ArgumentException("Wrong size of rectangle");
        var rect = Spiral.GetPoints()
            .Select(point =>
            {
                var coordinatesOfRectangle = RectangleCoordinatesCalculator.CalculateRectangleCoordinates(point, rectangleSize);
                return new Rectangle(coordinatesOfRectangle, rectangleSize);
            })
            .First(rectangle => !IntersectsWithOtherRectangles(rectangle));
        Rectangles.Add(rect);
        return rect;
    }

    private bool IntersectsWithOtherRectangles(Rectangle rectangle)
    {
        return Rectangles.Any(r => r.IntersectsWith(rectangle));
    }
}