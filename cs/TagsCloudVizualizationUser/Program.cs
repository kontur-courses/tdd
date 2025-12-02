using System.Drawing;
using System.Runtime.Versioning;
using TagsCloudVisualization;

namespace TagsCloudVizualizationUser;
[SupportedOSPlatform("windows")]
public static class TestProgram
{
    public static void Main()
    {
        var outputDir = @"C:\Users\Владислав\tddHometask\cs\TagsCloudVizualizationUser\clouds";
        var file1 = Path.Combine(outputDir, "example1.png");
        var file2 = Path.Combine(outputDir, "example2.png");
        var file3 = Path.Combine(outputDir, "example3.png");
        
        var sizes1 = Enumerable
            .Range(1, 10000)
            .Select(i => new Size(i % 40 + 10, i % 30 + 10))
            .ToList();
        var sizes2 = Enumerable
            .Range(1, 100)
            .Select(i => new Size(i % 60 + 10, i % 20 + 10))
            .ToList();
        var sizes3 = Enumerable
            .Range(1, 50)
            .Select(i => new Size(i % 40 + 10, i % 70 + 10))
            .ToList();
        
        CloudVisualizer.GenerateRectanglesCloud(file1,sizes1, rectangleBorderColor: Color.White, 
            baseRectangleColor: Color.Aqua, backgroundColor: Color.White, outputDirectory:outputDir);
        CloudVisualizer.GenerateRectanglesCloud(file2, sizes2, outputDirectory: outputDir, baseRectangleColor: Color.Chartreuse);
        CloudVisualizer.GenerateRectanglesCloud(file3, sizes3,  outputDirectory: outputDir,  baseRectangleColor: Color.HotPink);
    }
}
