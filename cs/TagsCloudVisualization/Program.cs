using System.Drawing;

namespace TagsCloudVisualization;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
public static class Program
{
    public static void Main(string[] args)
    {
        const string tempBmpFile = "temp.bmp";
        var cloudLayouter = new CircularCloudLayouter(new Point(-5, 5));
        var sizesGenerator = new RandomSizesGenerator();
        var sizes = sizesGenerator
            .GenerateSize()
            .Take(500);
        var tagCloud = TagCloudCreator.Create(sizes, cloudLayouter);

        TagCloudVisualization.SaveTagCloudAsBitmap(tagCloud, tempBmpFile);
    }
}