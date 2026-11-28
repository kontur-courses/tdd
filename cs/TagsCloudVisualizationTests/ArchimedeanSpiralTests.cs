using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

public class ArchimedeanSpiralTests
{
    private ArchimedeanSpiral sut;
    
    [SetUp]
    public void Setup()
    {
        sut = new  ArchimedeanSpiral();
    }
    
    [Test]
    public void Generate_ShouldReturnStart_WhenFirstCall()
    {
        var center = new Point(100, 80);

        var actual = sut.Generate(center).First();

        actual.Should().Be(center);
    }
    
    [TestCase(9, 100, 81)]
    [TestCase(32, 97, 80)]
    [TestCase(47, 100, 76)]
    [TestCase(63, 105, 80)]
    public void Generate_ShouldReturnExpectedPoint(
        int index, int expectedX, int expectedY)
    {
        var center = new Point(100, 80);
        var expected = new Point(expectedX, expectedY);

        var actual = sut.Generate(center).ElementAt(index);

        actual.Should().Be(expected);
    }
    
    [TestCase(8, 80)]
    [TestCase(9, 81)]
    public void Generate_ShouldRoundToNearestInteger_WhenNumberIsFraction(int index, int expectedY)
    {
        var center = new Point(100, 80);

        var actual = sut.Generate(center).ElementAt(index);

        actual.Y.Should().Be(expectedY);
    }
}