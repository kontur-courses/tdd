using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class PointExtensions
    {
        public static Rectangle GetRectangleFromCenterPoint(this Point center, Size size)
        {
            return new Rectangle(center.X - size.Width / 2, center.Y + size.Height / 2, size.Width, size.Height);
        }
    }
}