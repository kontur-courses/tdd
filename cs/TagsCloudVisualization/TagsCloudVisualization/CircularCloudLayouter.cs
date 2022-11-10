﻿using System.Drawing;
using System.Numerics;
using System.Runtime.Intrinsics.X86;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private const float alpha = 0.1f;

    private Point _center;
    private List<Rectangle> _rectangles;
    private Spiral _spiral;

    public List<Rectangle> Rectangles
    {
        get => _rectangles;
    }

    public Point Center
    {
        get => _center;
    }

    public CircularCloudLayouter(Point center)
    {
        _center = center;
        _rectangles = new();
        _spiral = new(alpha, center);
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        _spiral.SetNewRandomStartAngle();
        Rectangle rect = new(_center, rectangleSize);
        while (IsIntersectedWithExistingRectangles(rect))
            rect = UpdateCoord(rect);

        UpdateRectangleList(rect);
        return rect;
    }

    public Rectangle UpdateCoord(Rectangle r)
    {
        _spiral.IncreaseAngle(Math.Min(r.Width, r.Height));
        return new Rectangle(
            _spiral.ToCartesian(),
            r.Size
        );
    }

    public bool IsIntersectedWithExistingRectangles(Rectangle rect)
    {
        return _rectangles
            .Any(r => r.IntersectsWith(rect));
    }

    private void UpdateRectangleList(Rectangle rect)
    {
        _rectangles.Add(rect);
    }
}