using System.Drawing;
using TagsCloudLib.Abstractions.Visualizer;
using TagsCloudLib.Models;

public class TagCloudVisualizer(
    IBoundingBoxCalculator bboxCalc,
    ICanvasSizeCalculator canvasCalc,
    IScalingCalculator scaleCalc,
    ICoordinateTransformer transformer,
    IRender renderer)
{
    public void Draw(IEnumerable<Rectangle> rectangles, string outputPath, TagCloudVisualizationConfig config)
    {
        var rects = rectangles.ToList();

        if (rects.Count == 0)
        {
            renderer.Render(new TransformResult([], 1f), new CanvasSize(config.CanvasWidth, config.CanvasHeight),
                config,
                outputPath
            );
            return;
        }

        var bounds = bboxCalc.Calculate(rects);
        var canvas = canvasCalc.Calculate(bounds, config);
        var scale = scaleCalc.CalculateScale(bounds, canvas, config);

        var transformResult = transformer.Transform(rects.ToArray(), bounds, canvas, config, scale);

        renderer.Render(transformResult, canvas, config, outputPath);
    }
}