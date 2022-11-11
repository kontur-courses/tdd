﻿using System.Drawing;

namespace TagsCloudVisualization.Geometry;

public class Circle
{
    private readonly double radius;
    private readonly Point center;

    public Circle(double radius, Point center)
    {
        if (radius <= 0)
            throw new ArgumentException("Radius must be positive");
        this.radius = radius;
        this.center = center;
    }

    public bool ContainsRectangle(Rectangle rectangle)
    {
        var dx = Math.Max(Math.Abs(center.X - rectangle.Left), Math.Abs(rectangle.Right - center.X));
        var dy = Math.Max(Math.Abs(center.Y - rectangle.Top), Math.Abs(rectangle.Bottom - center.Y));
        var rad = radius * radius;
        var b = dx * dx + dy * dy;
        return rad >= b;
    }
}