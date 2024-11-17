using System.Drawing;

namespace TagsCloudVisualization.CloudLayouter;

public static class ICircularCloudLayouterExtensions 
{
    public static void GenerateCloud(
        this ICircularCloudLayouter cloudLayouter, 
        int rectanglesNumber = 1000,
        int minRectangleSize = 10,
        int maxRectangleSize = 50) 
    { 
        var random = new Random();
        new Rectangle[rectanglesNumber]
            .Select(x => new Size(
                random.Next(minRectangleSize, maxRectangleSize),
                random.Next(minRectangleSize, maxRectangleSize)))
            .Select(size => cloudLayouter.PutNextRectangle(size))
            .ToArray();
    } 
}