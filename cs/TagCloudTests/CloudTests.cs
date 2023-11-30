using System.Drawing;
using FluentAssertions;
using TagCloud;

namespace TagCloudTests;

public class CloudTests
{
    private CircularCloudLayouter layouter;
    private Point center = new Point(0, 0);

    [SetUp]
    public void Setup()
    {
        layouter = new CircularCloudLayouter(center);
    }

    [Test]
    public void ShouldReturnEmptyList_WhenCreated()
    {
        layouter.Rectangles.Should().BeEmpty();
    }

    [Test]
    public void ShouldReturnOneElementList_WhenAddOne()
    {
        layouter.PutNextRectangle(new Size(1, 1));
        layouter.Rectangles.Count().Should().Be(1);
    }

    [Test]
    public void ShouldReturnTwoElementList_WhenAddTwo()
    {
        layouter.PutNextRectangle(new Size(1, 1));
        layouter.PutNextRectangle(new Size(1, 1));
        layouter.Rectangles.Count().Should().Be(2);
    }
}