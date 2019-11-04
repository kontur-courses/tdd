using System;
using System.Drawing;

namespace TagCloud
{
    public interface ISpiralLayouter
    {
        Rectangle PutNextRectangle(Size rectangleSize, Predicate<Rectangle> isIntersect);
    }
}