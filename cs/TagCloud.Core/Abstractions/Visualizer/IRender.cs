using TagsCloudLib.Models;

namespace TagsCloudLib.Abstractions.Visualizer;

public interface IRender
{
    void Render(TransformResult layout, CanvasSize canvas, TagCloudVisualizationConfig config, string outputPath);
}