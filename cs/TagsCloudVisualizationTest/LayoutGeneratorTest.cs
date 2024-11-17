using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTest;

public class LayoutGeneratorTests
{
    private LayoutGenerator generator;
    private Point center;
    private Size imageSize;
    private string outputDirectory;

    [SetUp]
    public void Setup()
    {
        center = new Point(0, 0);
        imageSize = new Size(800, 600);
        generator = new LayoutGenerator(center, imageSize);
        outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "TestOutputs");

        Directory.CreateDirectory(outputDirectory);
    }

    [TearDown]
    public void TearDown()
    {
        if (Directory.Exists(outputDirectory))
            Directory.Delete(outputDirectory, true);
    }

    [Test]
    public void GenerateLayout_ShouldCreateImageFile()
    {
        var outputFileName = Path.Combine(outputDirectory, "layout_test.png");
        generator.GenerateLayout(outputFileName, 10, random => new Size(50, 20));

        File.Exists(outputFileName).Should().BeTrue("файл изображения должен быть создан");
    }

    [Test]
    public void GenerateLayout_ShouldGenerateCorrectNumberOfRectangles()
    {
        var outputFileName = Path.Combine(outputDirectory, "layout_test.png");
        const int rectangleCount = 50;
        generator.GenerateLayout(outputFileName, rectangleCount, random => new Size(50, 20));

        var layouter = new CircularCloudLayouter(center);
        for (var i = 0; i < rectangleCount; i++)
        {
            layouter.PutNextRectangle(new Size(50, 20));
        }

        layouter.GetRectangles().Count.Should().Be(rectangleCount, "должно быть сгенерировано правильное количество прямоугольников");
    }
}