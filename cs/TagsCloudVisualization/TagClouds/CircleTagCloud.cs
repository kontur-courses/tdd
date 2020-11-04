using System.Drawing;
using TagsCloudVisualization.Layouters;

namespace TagsCloudVisualization.TagClouds
{
    public class CircleTagCloud : TagCloud
    {
        public Point Center { get; }
        public double StartRadius { get; }

        public CircleTagCloud(Point center, double startRadius = 0)
            : base(new CircularCloudLayouter(center, startRadius))
        {
            Center = center;
            StartRadius = startRadius;
        }
    }
}
