using System.Collections.Generic;
using System;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    public readonly Point Center;
    public List<Rectangle> Tags { get; }
    private readonly ArchimedeanSpiral spiral;

    public CircularCloudLayouter(Point center)
    {
        Center = center;
        Tags = new List<Rectangle>();
        spiral = new ArchimedeanSpiral(center);
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
        {
            throw new ArgumentException("Width and height of the rectangle must be positive.");
        }

        Rectangle newRectangle;

        do
        {
            var location = spiral.GetNextPoint();
            location.Offset(-rectangleSize.Width / 2, rectangleSize.Height / 2);
            newRectangle = new Rectangle(location, rectangleSize);
        }
        while (Tags.IsIntersectsWithAny(newRectangle));

        newRectangle = ShiftRectangleToCenter(newRectangle);
        Tags.Add(newRectangle);
        return newRectangle;
    }

    public Size Size()
    {
        if (Tags.Count == 0)
            return System.Drawing.Size.Empty;

        var left = Tags.Min(rectangle => rectangle.Left);
        var right = Tags.Max(rectangle => rectangle.Right);
        var top = Tags.Min(rectangle => rectangle.Top);
        var bottom = Tags.Max(rectangle => rectangle.Bottom);

        return new Size(right - left, bottom - top);
    }

    private Rectangle ShiftRectangleToCenter(Rectangle rectangle)
    {
        var directionToCenter = GetDirectionToCenter(rectangle);
        while (directionToCenter != Point.Empty)
        {
            var nextRectangle = MoveRectangle(rectangle, directionToCenter);
            if (Tags.IsIntersectsWithAny(nextRectangle))
                break;

            rectangle = nextRectangle;
            directionToCenter = GetDirectionToCenter(rectangle);
        }

        return rectangle;
    }

    private Point GetDirectionToCenter(Rectangle rectangle)
    {
        var rectangleCenter = new Point(
            rectangle.Left + rectangle.Width / 2,
            rectangle.Top - rectangle.Height / 2);

        return new Point(
            Math.Sign(Center.X - rectangleCenter.X),
            Math.Sign(Center.Y - rectangleCenter.Y)
        );
    }

    private Rectangle MoveRectangle(Rectangle rectangle, Point direction)
    {
        return new Rectangle(
            new Point(rectangle.X + direction.X, rectangle.Y + direction.Y),
            rectangle.Size);
    }
}