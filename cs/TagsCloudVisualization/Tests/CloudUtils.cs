using System.Drawing;

namespace TagsCloudVisualization.Tests;

public static class CloudUtils
{
    public static bool HasAnyIntersection(IReadOnlyList<Rectangle> rectangles)
    {
        for (var i = 0; i < rectangles.Count; i++)
        {
            for (var j = i + 1; j < rectangles.Count; j++)
            {
                if (rectangles[i].IntersectsWith(rectangles[j]))
                    return true;
            }
        }

        return false;
    }

    public static Rectangle GetBoundingBox(IReadOnlyCollection<Rectangle> rectangles)
    {
        if (rectangles.Count == 0)
            return Rectangle.Empty;

        var minX = rectangles.Min(r => r.Left);
        var minY = rectangles.Min(r => r.Top);
        var maxX = rectangles.Max(r => r.Right);
        var maxY = rectangles.Max(r => r.Bottom);

        return Rectangle.FromLTRB(minX, minY, maxX, maxY);
    }

    public static double GetMaxDistanceFromCenter(IReadOnlyCollection<Rectangle> rectangles, Point center)
    {
        if (rectangles.Count == 0)
            return 0;

        return rectangles
            .SelectMany(GetCorners)
            .Max(corner => Distance(corner, center));
    }

    public static double GetIdealCircleRadius(IReadOnlyCollection<Rectangle> rectangles)
    {
        var totalArea = rectangles.Sum(r => (double)r.Width * r.Height);
        return Math.Sqrt(totalArea / Math.PI);
    }

    public static double GetCirclePackingDensity(IReadOnlyCollection<Rectangle> rectangles, Point center)
    {
        if (rectangles.Count == 0)
            return 0;

        var enclosingRadius = GetMaxDistanceFromCenter(rectangles, center);
        if (enclosingRadius <= 0)
            return 0;

        var totalRectanglesArea = rectangles.Sum(r => (double)r.Width * r.Height);
        var enclosingCircleArea = Math.PI * enclosingRadius * enclosingRadius;
        return totalRectanglesArea / enclosingCircleArea;
    }

    public static double GetAspectRatio(Rectangle boundingBox)
    {
        if (boundingBox.Width <= 0 || boundingBox.Height <= 0)
            return 0;

        return (double)Math.Min(boundingBox.Width, boundingBox.Height)
               / Math.Max(boundingBox.Width, boundingBox.Height);
    }

    public static double GetGapBetween(Rectangle a, Rectangle b)
    {
        var dx = Math.Max(0, Math.Max(a.Left - b.Right, b.Left - a.Right));
        var dy = Math.Max(0, Math.Max(a.Top - b.Bottom, b.Top - a.Bottom));
        return Math.Sqrt(dx * dx + dy * dy);
    }

    public static double Distance(PointF a, PointF b)
    {
        var dx = a.X - b.X;
        var dy = a.Y - b.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    private static IEnumerable<Point> GetCorners(Rectangle rectangle)
    {
        yield return new Point(rectangle.Left, rectangle.Top);
        yield return new Point(rectangle.Right, rectangle.Top);
        yield return new Point(rectangle.Left, rectangle.Bottom);
        yield return new Point(rectangle.Right, rectangle.Bottom);
    }
}