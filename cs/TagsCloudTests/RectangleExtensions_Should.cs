using FluentAssertions;
using NUnit.Framework;
using SixLabors.ImageSharp;
using TagsCloudVisualization.CloudLayouter;

namespace TagsCloudTests;

[TestFixture]
public class RectangleExtensions_Should : CircularCloudLayouterTestsBase
{
    [Test]
    public void IntersectsWithAny_ShouldReturnTrue_WhenRectangleIntersectsWithAnyInCollection()
    {
        var rectangleToCheck = new RectangleF(50, 50, 100, 100);
        var existingRectangles = new List<RectangleF>
        {
            new RectangleF(30, 30, 50, 50),
            new RectangleF(200, 200, 50, 50)
        };

        var result = rectangleToCheck.IntersectsWithAny(existingRectangles);

        result.Should().BeTrue();
    }

    [Test]
    public void IntersectsWithAny_ShouldReturnFalse_WhenNoIntersectionWithAnyInCollection()
    {
        var rectangleToCheck = new RectangleF(50, 50, 100, 100);
        var existingRectangles = new List<RectangleF>
        {
            new RectangleF(200, 200, 50, 50),
            new RectangleF(300, 300, 50, 50)
        };

        var result = rectangleToCheck.IntersectsWithAny(existingRectangles);

        result.Should().BeFalse();
    }

    [Test]
    public void IntersectsWithAny_ShouldReturnFalse_WhenCollectionIsEmpty()
    {
        var rectangleToCheck = new RectangleF(50, 50, 100, 100);
        var existingRectangles = new List<RectangleF>();

        var result = rectangleToCheck.IntersectsWithAny(existingRectangles);

        result.Should().BeFalse();
    }
}