using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Layouter;

namespace TagsCloudVisualization.Tests;

[TestFixture]
public class RectangleExtension_Should
{
    [Test]
    public void GetPoints_ShouldCalculate_CentredPointsCorrectly()
    {
        var expected = new[] { new Point(0, 2), new Point(0, 0), new Point(2, 2), new Point(2, 0) };
        var rectangle = new Rectangle(0, 0, 2, 2);
        rectangle.GetPoints().Should().BeEquivalentTo(expected);
    }

    [Test]
    public void GetPoints_ShouldCalculate_NegativeLocationPoint()
    {
        var expected = new[] { new Point(-51, -51), new Point(-51, -53), new Point(-49, -51), new Point(-49, -53) };
        var rectangle = new Rectangle(-51, -53, 2, 2);
        rectangle.GetPoints().Should().BeEquivalentTo(expected);
    }

    [TestCase(0, 1, TestName = "zero width")]
    [TestCase(1, 0, TestName = "zero height")]
    [TestCase(-1, 1, TestName = "negative width")]
    [TestCase(1, -1, TestName = "negative height")]
    public void GetPoints_ShouldThrowException_WithIncorrectSize(int width, int height)
    {
        var rectangle = new Rectangle(0, 0, width, height);
        var action = () => rectangle.GetPoints();
        action.Should().Throw<IncorrectSizeException>();
    }
}