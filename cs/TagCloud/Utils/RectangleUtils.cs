using SixLabors.ImageSharp;

namespace TagCloud.Utils;

public static class RectangleUtils
{
    public static Rectangle ShiftToCenter(Rectangle rectangle, Point center, List<Rectangle> otherRectangles)
    {
        var canMoveX = true;
        var canMoveY = true;

        while (canMoveX || canMoveY)
        {
            canMoveX = TryShiftToCenterByX(rectangle, center, otherRectangles, out var resultedMovementX);
            canMoveY = TryShiftToCenterByY(rectangle, center, otherRectangles, out var resultedMovementY);
            
            if (canMoveX)
                rectangle.Offset(resultedMovementX);
            if (canMoveY)
                rectangle.Offset(resultedMovementY);
        }
        
        return rectangle;
    }

    private static bool TryShiftToCenterByX(Rectangle rectangle, Point center, List<Rectangle> otherRectangles, out Point resultedMovement)
    {
        resultedMovement = new Point(0);
        
        var rectangleCenter = rectangle.X + rectangle.Width / 2;
        var directionSign = Math.Sign(center.X - rectangleCenter);
        
        if (directionSign == 0)
            return false;
        
        rectangle.X += directionSign;
        
        if (IsIntersectsWithCollection(rectangle, otherRectangles))
            return false;
        
        resultedMovement = new Point(directionSign, 0);
        return true;
    }
    
    private static bool TryShiftToCenterByY(Rectangle rectangle, Point center, List<Rectangle> otherRectangles, out Point resultedMovement)
    {
        resultedMovement = new Point(0);
        
        var rectangleCenter = rectangle.Y + rectangle.Height / 2;
        var directionSign = Math.Sign(center.Y - rectangleCenter);
        
        if (directionSign == 0)
            return false;
        
        rectangle.Y += directionSign;
        
        if (IsIntersectsWithCollection(rectangle, otherRectangles))
            return false;
        
        resultedMovement = new Point(0, directionSign);
        return true;
    }

    private static bool IsIntersectsWithCollection(Rectangle rectangle, List<Rectangle> otherRectangles) =>
        otherRectangles.Any(rectangle.IntersectsWith);
}