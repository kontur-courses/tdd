using System;
using System.Drawing;

namespace TagsCloudVisualisation
{
    public static class Extensions
    {
        public static Rectangle PlaceRectangle(this CircularCloudLayouter.CandidatePoint point, Size size) =>
            new Rectangle(point.Direction switch
            {
                CircularCloudLayouter.PointDirection.Up => new Point(point.X, point.Y - size.Height - 1),
                CircularCloudLayouter.PointDirection.Down => new Point(point.X - size.Width, point.Y + 1),
                CircularCloudLayouter.PointDirection.Left => new Point(point.X - size.Width - 1, point.Y - size.Height),
                CircularCloudLayouter.PointDirection.Right => new Point(point.X + 1, point.Y),
                _ => throw new ArgumentOutOfRangeException(nameof(point.Direction))
            }, size);
    }
}