using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization;

public class CircularCloudLayouter : ICircularCloudLayouter
{
    private readonly Point center;
    private readonly List<Rectangle> rectangles = new();
    private const double SpiralStep = 0.5;
    private const double AngleStep = 0.1;

    public IReadOnlyList<Rectangle> Rectangles => rectangles;

    public CircularCloudLayouter(Point center)
    {
        this.center = center;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException("Size must be positive", nameof(rectangleSize));

        if (rectangles.Count == 0)
        {
            var firstRect = CreateRectangle(center, rectangleSize);
            rectangles.Add(firstRect);
            return firstRect;
        }

        var maxRadius = CalculateMaxSearchRadius(rectangleSize);
        double angle = 0;
        double radius = 0;

        for (; radius <= maxRadius; radius += SpiralStep)
        {
            angle += AngleStep;
            var x = center.X + (int)(radius * Math.Cos(angle));
            var y = center.Y + (int)(radius * Math.Sin(angle));
            var candidateRectangle = CreateRectangle(new Point(x, y), rectangleSize);

            if (IntersectsWithAny(candidateRectangle))
                continue;

            var compactedRect = CompactToCenter(candidateRectangle);
            rectangles.Add(compactedRect);
            return compactedRect;
        }
        throw new InvalidOperationException("Unable to find a place for rectangle");
    }

    private double CalculateMaxSearchRadius(Size rectangleSize)
    {
        var maxExistingRadius = rectangles.SelectMany(r => new[]
            {
            new Point(r.Left, r.Top),
            new Point(r.Right, r.Top),
            new Point(r.Right, r.Bottom),
            new Point(r.Left, r.Bottom)
            })
            .Max(p => Distance(center, p));

        var newRectangleRadius = Math.Sqrt(
            rectangleSize.Width * rectangleSize.Width +
            rectangleSize.Height * rectangleSize.Height) / 2;

        return maxExistingRadius + newRectangleRadius + SpiralStep;
    }

    private static double Distance(Point first, Point second)
    {
        var dx = (double)first.X - second.X;
        var dy = (double)first.Y - second.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    private Rectangle CreateRectangle(Point centerPoint, Size size)
    {
        return new Rectangle(centerPoint.X - size.Width / 2, centerPoint.Y - size.Height / 2, size.Width,size.Height);
    }

    private bool IntersectsWithAny(Rectangle rect)
    {
        return rectangles.Any(r => r.IntersectsWith(rect));
    }

    private Rectangle CompactToCenter(Rectangle rectangle)
    {
        var currentRectangle = rectangle;
        var dx = Math.Sign(center.X - currentRectangle.X - currentRectangle.Width / 2);
        var dy = Math.Sign(center.Y - currentRectangle.Y - currentRectangle.Height / 2);
        while (dx != 0 || dy != 0)
        {
            var nextRectangle = new Rectangle(
                currentRectangle.X + dx,
                currentRectangle.Y + dy,
                currentRectangle.Width,
                currentRectangle.Height);

            if (IntersectsWithAny(nextRectangle))
                break;

            currentRectangle = nextRectangle;
            dx = Math.Sign(center.X - currentRectangle.X - currentRectangle.Width / 2);
            dy = Math.Sign(center.Y - currentRectangle.Y - currentRectangle.Height / 2);
        }
        return currentRectangle;
    }
}