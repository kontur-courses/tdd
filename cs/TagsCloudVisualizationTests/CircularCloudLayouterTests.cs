using System.Drawing;
using Combinatorics.Collections;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

file static class Sources
{
    public static IEnumerable<IEnumerable<Size>> Source_ShouldPutRectanglesCorrectly()
    {
        yield return [new(5, 4)];
        yield return [new(1, 1), new(5, 4)];
        yield return [new(1, 1), new(5, 4), new(5, 3), new(12, 1), new(9, 3), new(27, 19)];
    }
}

[TestFixture]
public class CircularCloudLayouterTests
{
    private ICircularCloudLayouter _layouter;

    [SetUp]
    public void Setup()
    {
        _layouter = new CircularCloudLayouter(new Point(0, 0));
    }

    [TestCaseSource(typeof(Sources), nameof(Sources.Source_ShouldPutRectanglesCorrectly))]
    public void ShouldPutUncollidingRectangles(IEnumerable<Size> sizes)
    {
        List<Rectangle> rectangles = new();

        foreach (var size in sizes)
        {
            rectangles.Add(_layouter.PutNextRectangle(size));
        }

        TestHelper.CheckIfRectanglesIntersect(rectangles).Should().BeFalse("correctly put rectangles should not collide");
    }

    [Test]
    public void ShouldThrow_WhenRectangleSizeIsInvalid()
    {
        Size invalidSize = new(-1, -3);

        Action act = () => _layouter.PutNextRectangle(invalidSize);

        act.Should().Throw<ArgumentException>("invalid rectangle size was passed as an argument");
    }
}

internal static class TestHelper
{
    public static bool CheckIfRectanglesIntersect(IEnumerable<Rectangle> rectangles)
    {
        // есть только один прямоугольник - ему не с чем пересекаться
        if (!rectangles.Skip(1).Any()) return false;
        var rectangleCombinations = new Combinations<Rectangle>(rectangles, 2);
        foreach (var rectanglePair in rectangleCombinations)
        {
            if (rectanglePair[0].IntersectsWith(rectanglePair[1])) return true;
        }
        return false;
    }
}
