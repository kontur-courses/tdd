using System.Drawing;
using System.Linq;
using TagsCloudVisualization.TagClouds;

namespace TagsCloudVisualization.Visualizer
{
    public class RectangleVisualizer : IVisualizer<ImmutableTagCloud>
    {
        private readonly SolidBrush brush = new SolidBrush(Color.SlateGray);

        public RectangleVisualizer(ImmutableTagCloud cloud)
        {
            VisualizeTarget = cloud;
        }

        public ImmutableTagCloud VisualizeTarget { get; }

        public void Draw(Graphics graphics)
        {
            var leftUpBound = VisualizeTarget.LeftUpBound;
            graphics.TranslateTransform(-leftUpBound.X, -leftUpBound.Y);
            if (VisualizeTarget.Count > 0)
                graphics.FillRectangles(brush, VisualizeTarget.ToArray());
        }
    }
}
