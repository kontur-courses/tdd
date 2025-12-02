using System.Runtime.Versioning;

namespace TagsCloudVisualization;

class Program
{
    [SupportedOSPlatform("Windows")]
    private static void Main()
    {
        var imageSize = 1000;
        var spiralStep = Math.PI / 180;
        var spiralParameterA = 0.1f;
        Visualizer.GenerateImageCloudLayouter(imageSize, "Cloud-1.png", spiralStep, spiralParameterA);
        Visualizer.GenerateImageCloudLayouter(imageSize, "Cloud-2.png", spiralStep, spiralParameterA);
        Visualizer.GenerateImageCloudLayouter(imageSize, "Cloud-3.png", spiralStep, spiralParameterA);
    }
}