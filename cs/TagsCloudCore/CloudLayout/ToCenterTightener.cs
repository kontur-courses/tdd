using System.Drawing;
using TagsCloudCore.CloudLayout.Extensions;

namespace TagsCloudCore.CloudLayout;

public static class ToCenterTightener
{
    public static Rectangle Tighten(
        Rectangle rectangle,
        Point cloudCenter,
        IEnumerable<Rectangle> existingRectangles)
    {
        var existingRectanglesList = existingRectangles.ToList();
        var current = rectangle;

        while (TryMoveForward(current, cloudCenter, existingRectanglesList, out var moved))
        {
            current = moved;
        }

        return current;
    }

    private static bool TryMoveForward(
        Rectangle current,
        Point center,
        List<Rectangle> existingRectangles,
        out Rectangle moved)
    {
        moved = MoveOneStepTowardsCenter(current, center);

        var isRectangleStationary = moved == current;
        if (isRectangleStationary) return false;

        var wouldRectanglesIntersect = moved.IntersectsAny(existingRectangles);
        if (wouldRectanglesIntersect) return false;

        var doesPositionBecomeBetter = moved.GetDistanceToPoint(center) <= current.GetDistanceToPoint(center);
        return doesPositionBecomeBetter;
    }

    private static Rectangle MoveOneStepTowardsCenter(Rectangle rectangle, Point center)
    {
        var rectangleCenter = rectangle.Center();

        var dx = Math.Sign(center.X - rectangleCenter.X);
        var dy = Math.Sign(center.Y - rectangleCenter.Y);

        if (dx == 0 && dy == 0)
            return rectangle;

        return rectangle with { X = rectangle.X + dx, Y = rectangle.Y + dy };
    }
}