using System;
using System.Drawing;

namespace TagsCloudVisualisation
{
    public static class Extensions
    {
        public static Rectangle GetRectangle(this CircularCloudLayouter.CandidatePoint point, Size size) =>
            new Rectangle(point.Direction switch
            {
                CircularCloudLayouter.PointDirection.Up => new Point(point.X, point.Y - size.Height),
                CircularCloudLayouter.PointDirection.Down => new Point(point.X - size.Width, point.Y),
                CircularCloudLayouter.PointDirection.Left => new Point(point.X - size.Width, point.Y - size.Height),
                CircularCloudLayouter.PointDirection.Right => point,
                _ => throw new ArgumentOutOfRangeException(nameof(point.Direction))
            }, size);
    }
}