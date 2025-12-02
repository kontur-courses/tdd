using System.Drawing;
using FluentAssertions;
using TagsCloudCore.CloudLayout;
using TagsCloudCore.CloudLayout.Extensions;

namespace TagsCloudTests;

public class ToCenterTightenerTests
{
    [Test]
    public void ToCenterTightener_ShouldMoveRectangleCloserToCenter_WhenNoObstacles_Test()
    {
        var center = new Point(0, 0);
        var rectangle = new Rectangle(50, 50, 20, 20);

        var result = ToCenterTightener.Tighten(rectangle, center, []);

        result.GetDistanceToPoint(center)
            .Should()
            .BeLessThan(rectangle.GetDistanceToPoint(center));
    }

    [Test]
    public void ToCenterTightener_ShouldStopBeforeCollision_WhenObstacleIsPresent_Test()
    {
        var center = new Point(0, 0);
        var rectangle = new Rectangle(40, 0, 20, 20);
        var obstacle = new Rectangle(10, 0, 20, 20);

        var result = ToCenterTightener.Tighten(rectangle, center, [obstacle]);

        result.IntersectsAny([obstacle]).Should().BeFalse();
        result.GetDistanceToPoint(center).Should().BeGreaterThan(0);
    }
}