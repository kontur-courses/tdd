using System.Drawing;
using TagsCloudLib.Models;

namespace TagsCloudLib.Abstractions.Visualizer;

public interface ICoordinateTransformer
{
    TransformResult Transform(Rectangle[] rects, LayoutBounds bounds, CanvasSize canvas, TagCloudVisualizationConfig config, float scale);
}
