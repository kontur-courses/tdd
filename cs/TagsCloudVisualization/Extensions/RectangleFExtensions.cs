using System.Drawing;

namespace TagsCloudVisualization
{
    public static class RectangleFExtensions
    {
        public static PointF GetCenter(this RectangleF rect) =>
            new PointF(rect.X + rect.Width / 2f, 
                rect.Y + rect.Height / 2f);

        public static RectangleF GetRectangleByCenter(SizeF sz, PointF point) =>
            new RectangleF(new PointF
                (point.X - sz.Width / 2f, point.Y - sz.Height / 2f), sz);
    }
}
