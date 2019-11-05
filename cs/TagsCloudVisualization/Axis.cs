using System.Drawing;

namespace TagsCloudVisualization
{
    enum Axis
    {
        X,
        Y
    }

    static class PointExtension
    {
        public static int SelectCoordinatePointAlongAxis(this Point point, Axis axis) => axis == Axis.X ? point.X : point.Y;

        public static Point Add(this Point first, Point second) => new Point(first.X + second.X, first.Y + second.Y);
    }
}
