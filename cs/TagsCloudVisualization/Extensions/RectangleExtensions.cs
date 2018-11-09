using System.Drawing;

namespace TagsCloudVisualization.Extensions
{
    public static class RectangleExtensions
    {
        public static Point[] Vertices(this Rectangle r)
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
}
