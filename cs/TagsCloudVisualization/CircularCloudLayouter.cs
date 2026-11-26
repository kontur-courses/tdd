using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private readonly Point center;
    private readonly UnwindingSpiral spiral;
    private readonly List<Rectangle> placedRectangles = [];

    public CircularCloudLayouter(Point center)
        : this(center, new UnwindingSpiral())
    {
    }

    public CircularCloudLayouter(Point center, UnwindingSpiral spiral)
    {
        this.center = center;
        this.spiral = spiral;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        throw new NotImplementedException();
    }

    private Rectangle FindFreePlaceOnSpiral(Size rectangleSize)
    {
        throw new NotImplementedException();
    }

    private Rectangle ShiftTowardsCenter(Rectangle rectangle)
    {
        throw new NotImplementedException();
    }

    private static Rectangle CreateRectangleWithCenter(Point rectangleCenter, Size rectangleSize)
    {
        throw new NotImplementedException();
    }

    private static void ValidateSize(Size rectangleSize)
    {
        throw new NotImplementedException();
    }
}
