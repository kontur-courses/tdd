using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagCloudVisualization.UnitTests;

[TestFixture]
public class CircularCloudLayouterTests
{
    [TestCase(-1, -1)]
    [TestCase(-1, 10)]
    [TestCase(10, -1)]
    [TestCase(0, 0)]
    public void Constructor_Throws(int centerX, int centerY)
    {
        var action = () => { _ = new CircularCloudLayouter(new Point(centerX, centerY)); };

        action.Should().Throw<ArgumentException>();
    }
}