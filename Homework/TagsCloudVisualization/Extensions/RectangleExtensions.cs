using System.Drawing;

namespace TagsCloudVisualization.Extensions
{
    internal static class RectangleExtensions
    {
        public static bool Intersects(this Rectangle firstRect, Rectangle secondRect)
        {
            return !(firstRect.X + firstRect.Width < secondRect.X ||
                     firstRect.X > secondRect.X + secondRect.Width ||
                     firstRect.Y > secondRect.Y + secondRect.Height ||
                     firstRect.Y + firstRect.Height < secondRect.Y);
        }
    }
}