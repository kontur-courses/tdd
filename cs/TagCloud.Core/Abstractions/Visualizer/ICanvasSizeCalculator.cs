using TagsCloudLib.Models;

namespace TagsCloudLib.Abstractions.Visualizer;

public interface ICanvasSizeCalculator
{
    CanvasSize Calculate(LayoutBounds bounds, TagCloudVisualizationConfig config);
}