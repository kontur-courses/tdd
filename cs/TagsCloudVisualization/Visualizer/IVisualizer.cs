using System.Drawing;
using TagsCloudVisualization.TagClouds;

namespace TagsCloudVisualization.Visualizer
{
    public interface IVisualizer
    {
        TagCloud Cloud { get; }
        void Draw(Graphics graphics);
    }
}
