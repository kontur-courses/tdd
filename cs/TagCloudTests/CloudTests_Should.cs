using System.Drawing;
using FluentAssertions;
using TagCloud;

namespace TagCloudTests;

public class CloudTests_Should
{
    private CircularCloudLayouter layouter;
    private Point center = new Point(0, 0);

    [SetUp]
    public void Setup()
    {
        layouter = new CircularCloudLayouter(center);
    }

    [Test]
    public void ReturnEmptyList_WhenCreated()
    {
        layouter.Rectangles.Should().BeEmpty();
    }

    [Test]
    public void ReturnOneElementList_WhenAddOne()
    {
        layouter.PutNextRectangle(new Size(1, 1));
        layouter.Rectangles.Count().Should().Be(1);
    }

    [Test]
    public void ReturnTwoElementList_WhenAddTwo()
    {
        layouter.PutNextRectangle(new Size(1, 1));
        layouter.PutNextRectangle(new Size(1, 1));
        layouter.Rectangles.Count().Should().Be(2);
        NotIntersectedAssetration();
    }

    public void NotIntersectedAssetration()
    {
        layouter
            .Rectangles
            .HasIntersectedRectangles()
            .Should()
            .BeFalse();
    }
}