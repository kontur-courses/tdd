using System.Drawing;
using Microsoft.Extensions.DependencyInjection;
using TagsCloudLib;
using TagsCloudLib.Abstractions;
using TagsCloudLib.Abstractions.Factories;


class Program
{
    private static readonly Random Random = new();
    private static TagCloudVisualizationConfig config;
    private static CircularCloudLayouterBase layouter;
    private static TagCloudVisualizer visualizer;

    static void Main()
    {
        var services = new ServiceCollection();
        var exeFolder = AppContext.BaseDirectory;
        var projectRoot = Path.GetFullPath(Path.Combine(exeFolder, @"..\..\.."));
        var folderName = Path.Combine(projectRoot, "Images");
        Directory.CreateDirectory(folderName);
        
        config = new TagCloudVisualizationConfig
        {
            CanvasBackgroundColor = Color.Black,
            ShapeBorderColor = Color.Cyan,    
            ShapeBorderThickness = 3,
            ShapeFillColor = Color.FromArgb(80, Color.Cyan),
            AutoResize = true,
        };
        
        services.AddTagCloudServices(config);
        
        var center = new Point(0, 0);
        var provider = services.BuildServiceProvider();
        var layouterFactory = provider.GetRequiredService<ILayouterFactory>();
        layouter = layouterFactory.Create(center);
        visualizer = provider.GetRequiredService<TagCloudVisualizer>();
        
        GenerateExample("cloud1", 100, folderName);
        GenerateExample("cloud2", 250, folderName);
        GenerateExample("cloud3", 1000, folderName);

        Console.WriteLine($"Изображения сохранены по пути {folderName}");
    }


    private static void GenerateExample(string fileName, int count, string folderName)
    {
        for (var i = 0; i < count; i++)
        {
            var size = new Size(Random.Next(5, 50), Random.Next(5, 50));

            layouter.PutNextRectangle(size);
        }

        var path = Path.Combine(folderName, $"{fileName}_success.png");
        visualizer.Draw(layouter.Rectangles, path, config);
    }

}