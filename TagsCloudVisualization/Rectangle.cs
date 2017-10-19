using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        public static IEnumerable<Rectangle> GetNeighbour(this Rectangle rectangle, Size size)
        {
            yield return new Rectangle(
                rectangle.X - rectangle.Width,
                rectangle.Y + rectangle.Height,
                size.Width, size.Height);
            yield return new Rectangle(
                rectangle.X - rectangle.Width, 
                rectangle.Y,
                size.Width,size.Height);
            yield return new Rectangle(
                rectangle.X - rectangle.Width, 
                rectangle.Y - rectangle.Height,
                size.Width, size.Height);
            yield return new Rectangle(
                rectangle.X + rectangle.Width, 
                rectangle.Y, 
                size.Width, size.Height);
            yield return new Rectangle(
                rectangle.X + rectangle.Width, 
                rectangle.Y + rectangle.Height,
                size.Width, size.Height);
            yield return new Rectangle(
                rectangle.X + rectangle.Width, 
                rectangle.Y - rectangle.Height, 
                size.Width, size.Height);
            yield return new Rectangle(
                rectangle.X, 
                rectangle.Y + rectangle.Height, 
                size.Width, size.Height);
            yield return new Rectangle(
                rectangle.X, 
                rectangle.Y - rectangle.Height, 
                size.Width, size.Height);
        }

    }
}