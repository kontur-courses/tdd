using System.Drawing;

namespace TagsCloudVisualization;

public interface IVisualizer
{
    string SaveVisualization(IEnumerable<Rectangle> rectangles, Point center, TagCloudVisualizationConfig config);
}