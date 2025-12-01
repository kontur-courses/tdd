using TagsCloudLib.Models;

namespace TagsCloudLib.Abstractions.Visualizer;

public interface IScalingCalculator
{
    float CalculateScale(LayoutBounds bounds, CanvasSize canvas, TagCloudVisualizationConfig config);
}