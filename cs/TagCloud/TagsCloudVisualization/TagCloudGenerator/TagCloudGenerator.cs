using System.Drawing;

namespace TagsCloudVisualization;

public class TagCloudGenerator
{
    private readonly IVisualizer _visualizer;
    private readonly Func<Point, ILayouter> _layouterFactory;
    private readonly IRectangleSizeProvider _sizeProvider;

    public TagCloudGenerator(
        IVisualizer visualizer,
        Func<Point, ILayouter> layouterFactory,
        IRectangleSizeProvider sizeProvider)
    {
        _visualizer = visualizer ?? throw new ArgumentNullException(nameof(visualizer));
        _layouterFactory = layouterFactory ?? throw new ArgumentNullException(nameof(layouterFactory));
        _sizeProvider = sizeProvider ?? throw new ArgumentNullException(nameof(sizeProvider));
    }

    public string Generate(TagCloudGenerationConfig generationConfig, TagCloudVisualizationConfig visualizationConfig)
    {
        var layouter = _layouterFactory(generationConfig.Center);
        var sizes = _sizeProvider.GetSizes(generationConfig.RectangleCount, generationConfig.MinSize, generationConfig.MaxSize);
        
        var placedRectangles = sizes.Select(size => layouter.PutNextRectangle(size));
        
        return _visualizer.SaveVisualization(
            rectangles: placedRectangles,
            center: generationConfig.Center,
            config: visualizationConfig
        );
    }
    
    public List<string> GenerateMultiple(
        List<TagCloudGenerationConfig> generationConfigs,
        List<TagCloudVisualizationConfig> visualizationConfigs)
    {

        if (generationConfigs.Count != visualizationConfigs.Count)
            throw new ArgumentException($"Number of generation configs ({generationConfigs.Count}) " +
                                        $"must match number of visualization configs ({visualizationConfigs.Count})");

        var results = new List<string>();
        
        for (var i = 0; i < generationConfigs.Count; i++)
        {
            try
            {
                var filePath = Generate(generationConfigs[i], visualizationConfigs[i]);
                results.Add(filePath);
            }
            catch (Exception ex)
            {
                results.Add($"ERROR for {visualizationConfigs[i].OutputFileName}: {ex.Message}");
            }
        }
        
        return results;
    }
}