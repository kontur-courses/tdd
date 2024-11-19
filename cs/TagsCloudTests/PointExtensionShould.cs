using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization.Extensions;

namespace TagsCloudTests;

[TestFixture]
public class PointExtensionShould
{
    [TestCase(0, 0, 1, 2)]
    [TestCase(0, 0, 1, 0)]
    [TestCase(3, 5, 0, 5)]
    public void PointShiftTo_ReturnedMovedPoint_WhenSet(int pointX, int pointY, int shiftX, int shiftY)
    {
        var point = new Point(pointX, pointY);
        var movementDirection = new Size(shiftX, shiftY);

        var movedPoint = Point.Add(point, movementDirection);

        MoveTo_CheckShift(point, movementDirection, movedPoint);
    }

    [Test]
    public void MoveTo_ReturnedNotMovedPoint_WhenSetZeroDirection()
    {
        var point = new Point(5, 6);

        MoveTo_CheckShift(point, new Size(0, 0), point);
    }

    private static void MoveTo_CheckShift(Point point, Size movementDirection, Point expectedPoint)
    {
        var movedPoint = point.MoveTo(movementDirection);

        movedPoint
            .Should()
            .BeEquivalentTo(expectedPoint);
    }
}

