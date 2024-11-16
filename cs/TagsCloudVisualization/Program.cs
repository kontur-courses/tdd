using System.Drawing;
using TagsCloudVisualization.Base;
using TagsCloudVisualization.CloudLayouters;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Visualizers;

namespace TagsCloudVisualization;

public static class Program
{
    private const int CountRectangles = 500;
    private const int MinLength = 15;
    private const int MaxLength = 30;
    private const string ImagesDirectory = "images";
    private static readonly Random Random = new();
    
    public static void Main()
    {
        var cloudLayouter = new CircularCloudLayouter(new Point(), 1, 0.1);
        var rectangles = Enumerable
            .Range(0, CountRectangles)
            .Select(_ => Random.NextSize(MinLength, MaxLength))
            .Select(size => cloudLayouter.PutNextRectangle(size))
            .ToList();

        var rectanglesWindow = new RectanglesWindow(rectangles);
        var visualizer = new CartesianVisualizer(new Size(rectanglesWindow.Width, rectanglesWindow.Height));
        using var bitmap = visualizer.CreateBitmap(rectangles);
        
        Directory.CreateDirectory(ImagesDirectory);
        var path = GetPathToTagsCloudImage();
        bitmap.Save(path);
    }

    private static string GetPathToTagsCloudImage() => 
        Path.Combine(ImagesDirectory, $"{CountRectangles}_{MinLength}_{MaxLength}_TagsCloud.png");
}