using System.Drawing;
using System.Linq;
using TagsCloudVisualization.TagClouds;

namespace TagsCloudVisualization.Visualizer
{
    public class SolidVisualizer : IVisualizer
    {
        private readonly SolidBrush brush;

        public SolidVisualizer(TagCloud cloud, Color color)
        {
            Cloud = cloud;
            Color = color;
            brush = new SolidBrush(color);
        }

        public TagCloud Cloud { get; }
        public Color Color { get; }

        public void Draw(Graphics graphics)
        {
            var leftUpBound = Cloud.LeftUpBound;
            graphics.TranslateTransform(-leftUpBound.X, -leftUpBound.Y);
            if (Cloud.Count > 0)
                graphics.FillRectangles(brush, Cloud.ToArray());
        }
    }
}
