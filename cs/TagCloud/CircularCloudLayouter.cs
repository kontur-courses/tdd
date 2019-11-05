using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloud;

namespace TagCloud
{
    public class ArchimedeanSpiral
    {
        public ArchimedeanSpiral(int parameter = 1) => this.parameter = parameter;
        private readonly int parameter;
        public double Angle { get; set; }
        public double Step = 1.0;

        public void IncrementAngle() => Angle += Step;
        public int GetCurrentX() => (int) Math.Round(parameter * Angle * Math.Sin(Angle));
        public int GetCurrentY() => (int) Math.Round(parameter * Angle * Math.Cos(Angle));
    }
}

public class CircularCloudLayouter
{
    public readonly ArchimedeanSpiral Spiral;
    public readonly HashSet<Rectangle> Rectangles;
    public Point CurrentCoords;

    public CircularCloudLayouter(Point center)
    {
        Spiral = new ArchimedeanSpiral();
        CurrentCoords = center;
        Rectangles = new HashSet<Rectangle>();
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException();
        var rect = new Rectangle(CurrentCoords, rectangleSize);
        AddRectangleToCollection(rect);
        UpdateCoordinates();
        return rect;
    }

    private void UpdateCoordinates()
    {
        Spiral.IncrementAngle();
        CurrentCoords = new Point(Spiral.GetCurrentX(), Spiral.GetCurrentY());
    }

    private void AddRectangleToCollection(Rectangle rect)
    {
        if (RectangleDoesNotIntersect(rect))
            Rectangles.Add(rect);
    }

    private bool RectangleDoesNotIntersect(Rectangle rectToAdd) =>
        Rectangles.All(rectangle => Rectangle.Intersect(rectToAdd, rectangle).IsEmpty);
}