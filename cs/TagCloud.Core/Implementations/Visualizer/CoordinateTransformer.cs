using System.Drawing;
using TagsCloudLib.Abstractions.Visualizer;
using TagsCloudLib.Models;

namespace TagsCloudLib.Implementations.Visualizer;

public class CoordinateTransformer : ICoordinateTransformer
{
    public TransformResult Transform(Rectangle[] rects, LayoutBounds bounds, CanvasSize canvas,
        TagCloudVisualizationConfig config, float scale)
    {
        var contentW = (bounds.Width + 2 * config.Padding) * scale;
        var contentH = (bounds.Height + 2 * config.Padding) * scale;

        var offsetX = (canvas.Width - contentW) / 2f;
        var offsetY = (canvas.Height - contentH) / 2f;

        var transformed = rects
            .Select(r => new RectangleF((r.X - bounds.MinX) * scale + offsetX + config.Padding * scale,
                (r.Y - bounds.MinY) * scale + offsetY + config.Padding * scale, r.Width * scale, r.Height * scale)
            )
            .ToList();

        return new TransformResult(transformed, scale);
    }
}