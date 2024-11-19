using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization.Extensions;

namespace TagsCloudTests;

[TestFixture]
public class RectangleExtensionShould
{
    [Test]
    [Repeat(5)]
    public void GetCentralPoint_ReturnExpectedCenter_AfterExecutionWithRandomRectangle()
    {
        var rectangleLocation = new Point(0, 0);
        var random = new Random();
        var rectangleSize = new Size(random.Next(1, 100), random.Next(1, 100));
        var rectangle = new Rectangle(rectangleLocation, rectangleSize);
        var expectedCentralPoint = new Point(rectangleLocation.X + rectangleSize.Width / 2,
            rectangleLocation.Y - rectangleSize.Height / 2);

        rectangle
            .GetCentralPoint()
            .Should()
            .Be(expectedCentralPoint);
    }
}