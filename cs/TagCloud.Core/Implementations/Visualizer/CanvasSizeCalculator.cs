using TagsCloudLib.Abstractions.Visualizer;
using TagsCloudLib.Models;

namespace TagsCloudLib.Implementations.Visualizer;

public class CanvasSizeCalculator : ICanvasSizeCalculator
{
    public CanvasSize Calculate(LayoutBounds bounds, TagCloudVisualizationConfig config)
    {
        if (!config.AutoResize)
        {
            return new CanvasSize(config.CanvasWidth, config.CanvasHeight);
        }

        var needW = bounds.Width + 2 * config.Padding;
        var needH = bounds.Height + 2 * config.Padding;

        return new CanvasSize(
            Math.Clamp(needW, config.MinCanvasWidth, config.MaxCanvasWidth),
            Math.Clamp(needH, config.MinCanvasHeight, config.MaxCanvasHeight)
        );
    }
}