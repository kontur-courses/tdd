using System.Drawing;

namespace TagsCloudVisualization
{
    public static class VisualizatorExtensions
    {
        public static RectangleF ConvertRectangleToRectangleF(this Rectangle rectangle)
        {
            var location = ConvertPointToPointF(rectangle.Center);
            var size = ConvertSizeToSizeF(rectangle.Size);

            return new RectangleF(location, size);
        }

        public static PointF ConvertPointToPointF(this Point point)
            => new PointF((float)point.X, (float)point.Y);

        public static SizeF ConvertSizeToSizeF(this Size size)
            => new SizeF((float)size.Width, (float)size.Height);
    }
}
