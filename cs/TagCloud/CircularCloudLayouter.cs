using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloud;

namespace TagCloud
{
    public class ArchimedeanSpiral
    {
        public ArchimedeanSpiral(Point center, int parameter = 1)
        {
            this.parameter = parameter;
            angle = center.X * center.X + center.Y * center.Y;
        }

        private readonly int parameter;
        private double angle;
        private int X => (int) Math.Round(parameter * angle * Math.Sin(angle));
        private int Y => (int) Math.Round(parameter * angle * Math.Cos(angle));

        public Point GetNewPoint()
        {
            Point result = new Point(X, Y);
            angle += Math.PI / 4;
            return result;
        }
    }
}

public class CircularCloudLayouter
{
    private readonly Point center;
    public readonly ArchimedeanSpiral Spiral;
    public readonly HashSet<Rectangle> Rectangles;

    public CircularCloudLayouter(Point center)
    {
        this.center = center;
        Spiral = new ArchimedeanSpiral(center);
        Rectangles = new HashSet<Rectangle>();
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        CheckCorrectSize(rectangleSize);
        var rect = new Rectangle(Spiral.GetNewPoint(), rectangleSize);
        while (!RectangleDoesNotIntersect(rect))
            rect = new Rectangle(Spiral.GetNewPoint(), rectangleSize);
        Rectangles.Add(rect);
        return rect;
    }

    private static void CheckCorrectSize(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException();
    }

    private bool RectangleDoesNotIntersect(Rectangle rectToAdd) =>
        Rectangles.All(rectangle => Rectangle.Intersect(rectToAdd, rectangle).IsEmpty);

    public double GetCloudFullnessPercent()
    {
        var radius = GetFurthestDistance();
        double cloudCircleSquare = Math.PI * radius * radius;
        double allRectanglesSquare = GetAllRectanglesSquare();
        return allRectanglesSquare / cloudCircleSquare;
    }

    public double GetFurthestDistance()
    {
        var furthestDistance = 0.0;
        foreach (var rect in Rectangles)
        {
            var currentDistance = GetDistanceToRectangle(center, rect);
            if (currentDistance > furthestDistance)
                furthestDistance = currentDistance;
        }

        return furthestDistance;
    }

    public double GetDistanceToRectangle(Point point, Rectangle rect)
    {
        var distances = new List<double>
        {
            GetDistanceBetweenPoints(point, new Point(rect.Left, rect.Top)),
            GetDistanceBetweenPoints(point, new Point(rect.Left, rect.Bottom)),
            GetDistanceBetweenPoints(point, new Point(rect.Right, rect.Top)),
            GetDistanceBetweenPoints(point, new Point(rect.Right, rect.Bottom))
        };
        return distances.Max();
    }

    public double GetDistanceBetweenPoints(Point from, Point to) =>
        Math.Sqrt((to.X - from.X) * (to.X - from.X) + (to.Y - from.Y) * (to.Y - from.Y));

    private double GetAllRectanglesSquare()
        => Rectangles.Sum(rect => rect.Width * rect.Height);
}