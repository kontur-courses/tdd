using System.Drawing;
namespace TagsCloudVisualization;

public static class Program
{
    public static void Main()
    {
        var rectanglesCounts = new int[3] { 50, 100, 200 };
        
        foreach (var count in rectanglesCounts)
        {
            var cloud = CloudGenerator
                .GenerateRandomCloudWithCenter(new(0, 0), count);
            var visualiser = new CloudVisualiser();
            visualiser.VisualiseAndSave(cloud.GetRectangles(), GetPathToFile(count));
        }
    }

    private static string GetPathToFile(int rectanglesCount)
    {
        var root = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.ToString();
        var samplesDirectory = Path.Combine(root, "Samples");
        var fileName = $"{rectanglesCount}_tags_cloud.png";

        if (!Directory.Exists(samplesDirectory))
            Directory.CreateDirectory(samplesDirectory);

        return Path.Combine(samplesDirectory, fileName);
    }
}
