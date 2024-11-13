using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization.TagsCloudDistributions;

namespace TagsCloudVisualization.Tests;


[TestFixture]
public class CircularCloudLayouterTest
{
    private CircularCloudLayouter layouter;
    private const string ImagesPath = @"..\..\..\Images\";

    [SetUp]
    public void Setup()
    { 
        layouter = new CircularCloudLayouter(new SpiralDistribution());
    } 
    
    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed) 
            return;
        var fileName = $"{TestContext.CurrentContext.Test.FullName}.png";
        var render = TagsCloudRenderer.Create(layouter);
        render.Render();
        render.SaveToPath(fileName, ImagesPath);
        Console.WriteLine($"Tag cloud visualization saved to file /Images/{fileName}");
    }
    
    [Test]
    public void CircularCloudLayouter_Initialize()
    {
        layouter.GetRectangles().Count.Should().Be(0);
        layouter.GetCenter().Should().Be(new Point(0, 0));
    }
    
    [TestCase(-1, 2, TestName = "width < 0")]
    [TestCase(0, 2, TestName = "width == 0")]
    [TestCase(1, -2, TestName = "height < 0")]
    [TestCase(1, 0, TestName = "height == 0")]
    public void PutNextRectangle_ShouldThrowArgumentException_AfterInvalidRectangleSize(int width, int height)
    {
        var action = () => layouter.PutNextRectangle(new Size(width, height));
        
        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void PutNextRectangle_ShouldReturnCorrectRectangleSize()
    {
        var rectangle = layouter.PutNextRectangle(new Size(16, 18));

        rectangle.Size.Should().Be(new Size(16, 18));
    }
    
    [Test]
    public void PutNextRectangle_ShouldReturnRectangleInCloudCenter()
    {
        var rectangle = layouter.PutNextRectangle(new Size(16, 18));

        rectangle.Location.Should().Be(layouter.GetCenter());
    }
    
    [TestCase(16)]
    [TestCase(64)]
    [TestCase(256)]
    public void PutNextRectangle_GeneratesRectanglesWithoutIntersects(int rectanglesCount)
    {
        GenerateRectangles(rectanglesCount, new Size(15, 20));

        HasIntersectedRectangles(layouter.GetRectangles()).Should().Be(false);
    }

    [Test]
    public void GetCloudSize_ShouldThrowArgumentException_EmptyCloud()
    {
        var action = () => layouter.GetCloudSize();
        action.Should().Throw<ArgumentException>();
    }

    private void GenerateRectangles(int rectanglesCount, Size size)
    {
        for (var i = 0; i < rectanglesCount; i++)
            layouter.PutNextRectangle(size);
    }

    private bool HasIntersectedRectangles(List<Rectangle> rectangles)
    {
        for (var i = 0; i < rectangles.Count - 1; i++)
        {
            for (var j = i + 1; j < rectangles.Count; j++)
            {
                if (rectangles[i].IntersectsWith(rectangles[j]))
                    return true;
            }
        }
        return false;
    }
}