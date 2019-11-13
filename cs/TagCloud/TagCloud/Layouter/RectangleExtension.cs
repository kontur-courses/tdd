using System;
using System.Drawing;

namespace TagCloud.Layouter
{
    public static class RectangleExtension
    {
        public static Rectangle Shift(this Rectangle rect, Point shiftPoint)
        {
            return new Rectangle(new Point(rect.X + shiftPoint.X, rect.Y + shiftPoint.Y), rect.Size);     
        }
    }
}