using System.Drawing;

namespace TagsCloudVisualization.Extensions
{
    public static class GraphicsExtensions
    {
        public static void DrawCircle(this Graphics graphics, Pen pen, Point center, float radius)
        {
            graphics.DrawEllipse(pen, center.X - radius, center.Y - radius,
                radius + radius, radius + radius);
        }
    }
}