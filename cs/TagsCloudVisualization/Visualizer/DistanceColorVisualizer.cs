using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.TagClouds;

namespace TagsCloudVisualization.Visualizer
{
    public class DistanceColorVisualizer : IVisualizer
    {
        private readonly CircleTagCloud cloud;
        private readonly Color fromColor;
        private readonly double fromRadius;
        private readonly Color toColor;
        private readonly double toRadius;

        public DistanceColorVisualizer(
            CircleTagCloud cloud,
            Color fromColor,
            double fromRadius,
            Color toColor,
            double toRadius)
        {
            this.cloud = cloud;
            this.fromColor = fromColor;
            this.fromRadius = fromRadius;
            this.toColor = toColor;
            this.toRadius = toRadius;
        }

        public void Draw(Graphics graphics)
        {
            var leftUpBound = cloud.LeftUpBound;
            graphics.TranslateTransform(-leftUpBound.X, -leftUpBound.Y);
            foreach (var rectangle in cloud)
            {
                var distance = cloud.Center.DistanceBetween(rectangle.Center());
                using var brush = GetBrush(LinearMath.LinearInterpolate(fromRadius, toRadius, distance));
                graphics.FillRectangle(brush, rectangle);
            }
        }

        private SolidBrush GetBrush(double fraction)
        {
            var a = GetColorComponent(fromColor.A, toColor.A, fraction);
            var r = GetColorComponent(fromColor.R, toColor.R, fraction);
            var g = GetColorComponent(fromColor.G, toColor.G, fraction);
            var b = GetColorComponent(fromColor.B, toColor.B, fraction);
            return new SolidBrush(Color.FromArgb(a, r, g, b));
        }

        private int GetColorComponent(int from, int to, double fraction)
        {
            return (int)((to - from) * fraction + from);
        }
    }
}
