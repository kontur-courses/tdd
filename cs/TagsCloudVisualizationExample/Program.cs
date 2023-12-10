using Aspose.Drawing;
using TagsCloudVisualization;

namespace TagsCloudVisualizationExample;

static class Program
{
    public static void Main()
    {
        GenerateExample(
            new Point(100, 100), 
            64,
            new VisualizerParams(200, 200));

        var visualizerParams = new VisualizerParams();
        visualizerParams.BgColor = Color.Cyan;
        visualizerParams.RectangleColor = Color.Black;
        
        GenerateExample(
            new Point(250, 250), 
            512,
            visualizerParams);
    }

    private static void GenerateExample(Point center, int rectanglesAmount, VisualizerParams visualizerParams)
    {
        visualizerParams.FileName = $"Example{rectanglesAmount}.png";
        
        var layouter = GetLayouter(center, rectanglesAmount);
        var visualizer = new CircularCloudVisualizer(visualizerParams);
        
        visualizer.Visualize(layouter);
    }
    
    private static CircularCloudLayouter GetLayouter(Point center, int rectanglesAmount)
    {
        var rnd = new Random();
        var layouter = new CircularCloudLayouter(center);

        for (var i = 0; i < rectanglesAmount; i++)
            layouter.PutNextRectangle(new Size(rnd.Next(5, 15), rnd.Next(5, 15)));

        return layouter;
    }
}







    
