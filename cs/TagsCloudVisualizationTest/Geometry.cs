using System.Drawing;

namespace TagsCloudVisualizationTest;

public static class Geometry
{
    public static double GetNormalizedAngleDifference(double angle1, double angle2)
    {
        var result = angle1 - angle2;
        while (result <= -Math.PI)
            result += 2 * Math.PI;
        while (result > Math.PI)
            result -= 2 * Math.PI;
        return result;
    }

    public static double GetDistance(Point point1, Point point2)
    {
        var dx = point1.X - point2.X;
        var dy = point1.Y - point2.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    public static double GetAngle(Point point1, Point point2)
    {
        var dx = point1.X - point2.X;
        var dy = point1.Y - point2.Y;
        return Math.Atan2(dy, dx);
    }

    public static double FindMinRadius(List<Rectangle> rectangles, Point center)
    {
        var resultRadius = rectangles
            .Select(rectangle => new List<Point>()
            {
                new Point(rectangle.Left, rectangle.Top),
                new Point(rectangle.Right, rectangle.Top),
                new Point(rectangle.Left, rectangle.Bottom),
                new Point(rectangle.Right, rectangle.Bottom)
            })
            .Aggregate(0.0, (current, points) => points
                .Select(point => Geometry.GetDistance(point, center))
                .Prepend(current)
                .Max());
        return resultRadius;
    }

    public static bool AreRectanglesInsideCircle(List<Rectangle> rectangles, Point center, double radius)
    {
        return rectangles
            .Select(rect => new List<Point>
            {
                new Point(rect.Left, rect.Top),
                new Point(rect.Right, rect.Top),
                new Point(rect.Left, rect.Bottom),
                new Point(rect.Right, rect.Bottom)
            })
            .All(points => !points.Any(point => GetDistance(point, center) > radius));
    }

    public static double GetRectanglesPackingDensity(List<Rectangle> rectangles, double radius)
    {
        var rectangleTotalArea = (double)rectangles.Sum(rect => rect.Width * rect.Height);
        var circleArea = Math.PI * radius * radius;
        return rectangleTotalArea/circleArea;
    }


    public static bool HasIntersectingRectangles(List<Rectangle> rectangles)
    {
        for (var i = 0; i < rectangles.Count; i++)
        {
            for (var j = i + 1; j < rectangles.Count; j++)
            {
                if(rectangles[i].IntersectsWith(rectangles[j]))
                    return true;
            }
        }

        return false;
    }
}