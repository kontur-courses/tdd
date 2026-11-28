using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization.Layout;

namespace TagsCloudVisualization.Tests;

[TestFixture]
public class RectangleCompactorTests
{
    private readonly RectangleCompactor compactor = new();

    [Test]
    public void MoveTowardsCenter_NoObstacles_ReachesCenter()
    {
        var rectangle = new Rectangle(4, -5, 2, 2);

        var result = compactor.MoveTowardsCenter(rectangle, [], Point.Empty);

        result.Should().Be(rectangle with{ Location = new Point(-1, -1)});
    }

    [Test]
    public void MoveTowardsCenter_AlreadyCentered_StaysInPlace()
    {
        var rectangle = new Rectangle(-1, -1, 2, 2);

        var result = compactor.MoveTowardsCenter(rectangle, [], Point.Empty);

        result.Should().Be(rectangle);
    }

    [Test]
    public void MoveTowardsCenter_VerticalWall_StopsAtWallAndContinuesAlongIt()
    {
        var rectangle = new Rectangle(4, 4, 2, 2);
        var wall = new Rectangle(1, -2, 1, 10);

        var result = compactor.MoveTowardsCenter(rectangle, [wall], Point.Empty);

        result.Should().Be(rectangle with{ Location = new Point(2, -1)});
    }

    [Test]
    public void MoveTowardsCenter_ObstacleBlocksHorizontalMovementInitially_ReachesCenter()
    {
        var rectangle = new Rectangle(4, 4, 2, 2);
        var obstacle = new Rectangle(3, 4, 1, 1);

        var result = compactor.MoveTowardsCenter(rectangle, [obstacle], Point.Empty);

        result.Should().Be(rectangle with{ Location = new Point(-1, -1)});
    }

    [Test]
    public void MoveTowardsCenter_BothDirectionsBlocked_StaysInPlace()
    {
        var rectangle = new Rectangle(4, 4, 2, 2);
        var leftObstacle = new Rectangle(3, 4, 1, 2);
        var topObstacle = new Rectangle(4, 3, 2, 1);

        var result = compactor.MoveTowardsCenter(rectangle, [leftObstacle, topObstacle], Point.Empty);

        result.Should().Be(rectangle);
    }
}
