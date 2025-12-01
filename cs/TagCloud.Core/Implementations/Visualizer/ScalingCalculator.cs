using TagsCloudLib.Abstractions.Visualizer;
using TagsCloudLib.Models;

namespace TagsCloudLib.Implementations.Visualizer;

public class ScalingCalculator : IScalingCalculator
{
    public float CalculateScale(LayoutBounds bounds, CanvasSize canvas, TagCloudVisualizationConfig config)
    {
        if (!config.AutoResize)
        {
            return 1f;
        }
        var totalW = bounds.Width + 2 * config.Padding;
        var totalH = bounds.Height + 2 * config.Padding;

        var scaleX = (float)canvas.Width / totalW;
        var scaleY = (float)canvas.Height / totalH;

        return Math.Min(scaleX, scaleY);
    }
}