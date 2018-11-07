using System;
using System.Drawing;

namespace TagsCloudVisualization
{

    public static class RectangleExtensions
    {
        public static Point[] Vertexes(this Rectangle r)
        {
            return new[]
            {
                r.Location,
                new Point(r.Location.X + r.Width, r.Location.Y),
                r.Location + r.Size,
                new Point(r.Location.X, r.Location.Y + r.Height)
            };
        }
    }

    public static class SizeEtansions
    {
        public static Size SetRandom(this Size size, int maxWidth, int maxHeight)
        {
            var r = new Random();
            size.Width = r.Next(3, maxWidth);
            size.Height = r.Next(3, maxHeight);
            return size;
        }
    }
}
