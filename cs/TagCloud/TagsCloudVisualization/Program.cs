using System.Drawing;

namespace TagsCloudVisualization.ConsoleApp;

class Program
{
    static void Main()
    {
        var imageSaver = new ImageSaver();
        var visualizer = new TagCloudVisualizer(imageSaver);
        var sizeProvider = new RandomRectangleSizeProvider();
        
        var layouterFactory = (Point center) =>
            new CircularCloudLayouter(center, new SpiralPointsProvider(center));

        var tagCloudGenerator = new TagCloudGenerator(
            visualizer,
            layouterFactory,
            sizeProvider);

        Console.WriteLine("Generating tag cloud visualizations...");
        
        List<TagCloudGenerationConfig> generationConfigs = 
        [
            new TagCloudGenerationConfig(
                center: new Point(400, 300),
                rectangleCount: 30,
                minSize: new Size(20, 10),
                maxSize: new Size(50, 30)
            ),
            new TagCloudGenerationConfig(
                center: new Point(500, 400),
                rectangleCount: 100,
                minSize: new Size(30, 15),
                maxSize: new Size(80, 40)
            ),
            new TagCloudGenerationConfig(
                center: new Point(600, 450),
                rectangleCount: 150,
                minSize: new Size(25, 12),
                maxSize: new Size(120, 60)
            )
        ];
        
        List<TagCloudVisualizationConfig> visualizationConfigs = 
        [
            new TagCloudVisualizationConfig(
                outputFileName: "cloud_small.png",
                imageSize: new Size(800, 600),
                backgroundColor: Color.White,
                rectangleColor: Color.Blue,
                centerColor: Color.Red
            ),
            new TagCloudVisualizationConfig(
                outputFileName: "cloud_medium.png",
                imageSize: new Size(1000, 800),
                backgroundColor: Color.White,
                rectangleColor: Color.Green,
                centerColor: Color.Red
            ),
            new TagCloudVisualizationConfig(
                outputFileName: "cloud_large.png",
                imageSize: new Size(1200, 900),
                backgroundColor: Color.Black,
                rectangleColor: Color.Yellow,
                centerColor: Color.Red
            )
        ];
        
        var results = tagCloudGenerator.GenerateMultiple(generationConfigs, visualizationConfigs);
        
        foreach (var result in results)
        {
            Console.WriteLine(result.StartsWith("ERROR") ? $"  - {result}" : $"- Created cloud at {result}");
        }
    }
}