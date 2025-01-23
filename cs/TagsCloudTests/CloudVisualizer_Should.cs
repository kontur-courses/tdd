using FluentAssertions;
using NUnit.Framework;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace TagsCloudTests;

public class CloudVisualizer_Should : CircularCloudLayouterTestsBase
{
    [Test]
    public void SaveVisualization_ShouldCreateImageWithDefaultSize_WhenSmallRectangles()
    {
        Layouter.PutNextRectangle(new SizeF(10, 10));
        var filePath = Path.Combine(ImagesDirectory, "test_visualization.png");
        Visualizer.SaveVisualization(Layouter.Rectangles, filePath);

        using (var image = Image.Load<Rgba32>(filePath))
        {
            image.Width.Should().Be(1200);
            image.Height.Should().Be(900);
        }
        File.Delete(filePath);
    }

    [Test]
    public void SaveVisualization_ShouldCreateImageWithBiggerSize_WhenManyRectangles()
    {
        for(int i = 0; i < 13; i++)
            Layouter.PutNextRectangle(new SizeF(300, 300));

        var filePath = Path.Combine(ImagesDirectory, "test_visualization.png");
        Visualizer.SaveVisualization(Layouter.Rectangles, filePath);

        using (var image = Image.Load<Rgba32>(filePath))
        { 
            image.Width.Should().BeGreaterThan(1200);
            image.Height.Should().BeGreaterThan(900);
        }
        File.Delete(filePath);
    }
    
    [Test]
    public void SaveVisualization_ShouldThrowDirectoryNotFoundException_WhenInvalidPath()
    {
        var invalidPath = @"M:\NonExistingDirectory\test_visualisation.png";
        var func = () => Visualizer.SaveVisualization(Layouter.Rectangles, invalidPath);

        func.Should().Throw<DirectoryNotFoundException>();
    }
}