using System.Drawing;
using System.Drawing.Imaging;

namespace TagCloudVisualization;

public static class Demonstration
{
    public static void GenerateDemoImages(string outputDirectory = "TagCloudImages")
    {
        var outputPath = EnsureOutputDirectory(outputDirectory);
        
        GenerateExample("example1", outputPath, center: new Point(400, 300), rectangleCount: 15);
        GenerateExample("example2", outputPath, center: new Point(100, 100), rectangleCount: 50);
        GenerateExample("example3", outputPath, center: new Point(500, 500), rectangleCount: 100);
    }
    
    private static string EnsureOutputDirectory(string directoryName)
    {
        var projectRoot = Directory.GetCurrentDirectory();
        var outputPath = Path.Combine(projectRoot, directoryName);
        
        if (!Directory.Exists(outputPath))
            Directory.CreateDirectory(outputPath);
        
        return outputPath;
    }
    
    private static void GenerateExample(
        string name,
        string outputPath,
        Point center,
        int rectangleCount)
    {
        var layouter = new CircularCloudLayouter(center);
        var rectangles = GenerateRectangles(layouter, rectangleCount, name.GetHashCode());
        
        var visualizer = new TagCloudVisualizer();
        
        var filePath = Path.Combine(outputPath, $"{name}.png");
        visualizer.CreateVisualization(rectangles, center, filePath);
    }
    
    private static List<Rectangle> GenerateRectangles(CircularCloudLayouter layouter, int count, int seed)
    {
        var rectangles = new List<Rectangle>();
        var random = new Random(seed);
        
        for (int i = 0; i < count; i++)
        {
            var width = random.Next(20, 80);
            var height = random.Next(15, 50);
            rectangles.Add(layouter.PutNextRectangle(new Size(width, height)));
        }
        
        return rectangles;
    }
}