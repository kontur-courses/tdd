using NUnit.Framework;
using FluentAssertions;
using TagsCloudVisualization.RectangleGenerator;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class RectangleGeneratorTest
{
    [Test]
    public void GenerateRandomRectangles_ReturnGeneratedRectanglesNumberMatchesRequested()
    {
        var rectangles = RectangleGenerator.GenerateRandomRectangles(10).ToList();

        rectangles.Count.Should().Be(10);
    }
    
    [Test]
    public void GenerateRandomRectangles_IsRandomRectangles_RectanglesAreGeneratedRandomly()
    {
        var rectangles1 = RectangleGenerator.GenerateRandomRectangles(10).ToList();
        var rectangles2 = RectangleGenerator.GenerateRandomRectangles(10).ToList();

        rectangles1.Should().NotBeEquivalentTo(rectangles2);
    }
}