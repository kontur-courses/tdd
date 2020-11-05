using System.Drawing;
using System.Linq;
using TagsCloudVisualization.TagClouds;

namespace TagsCloudVisualization.Visualizer
{
    public class SolidVisualizer : IVisualizer
    {
        private readonly TagCloud cloud;
        private readonly SolidBrush brush;

        public SolidVisualizer(TagCloud cloud, Color color)
        {
            this.cloud = cloud;
            brush = new SolidBrush(color);
        }

        public void Draw(Graphics graphics)
        {
            var leftUpBound = cloud.LeftUpBound;
            graphics.TranslateTransform(-leftUpBound.X, -leftUpBound.Y);
            graphics.FillRectangles(brush, cloud.ToArray());
        }
    }
}
