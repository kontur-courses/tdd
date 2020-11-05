using System.Drawing;
using System.Linq;
using TagsCloudVisualization.TagClouds;

namespace TagsCloudVisualization.Visualizer
{
    public class RectangleVisualizer : IVisualizer
    {
        private readonly SolidBrush brush = new SolidBrush(Color.SlateGray);
        private readonly ImmutableTagCloud cloud;

        public RectangleVisualizer(ImmutableTagCloud cloud)
        {
            this.cloud = cloud;
        }

        public TagCloud Cloud => cloud;

        public void Draw(Graphics graphics)
        {
            var leftUpBound = cloud.LeftUpBound;
            graphics.TranslateTransform(-leftUpBound.X, -leftUpBound.Y);
            if (cloud.Count > 0)
                graphics.FillRectangles(brush, cloud.ToArray());
        }
    }
}
