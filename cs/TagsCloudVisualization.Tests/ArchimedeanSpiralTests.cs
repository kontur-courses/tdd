using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization.Layout;

namespace TagsCloudVisualization.Tests;

[TestFixture]
public class ArchimedeanSpiralTests
{
    [TestCase(0, 0)]
    [TestCase(100, -50)]
    public void GetPoints_FirstPoint_IsSpiralCenter(int x, int y)
    {
        var center = new Point(x, y);
        var spiral = new ArchimedeanSpiral(center);

        var firstPoint = spiral.GetPoints().First();

        firstPoint.Should().BeEquivalentTo(center);
    }

    [TestCase(5)]
    [TestCase(21)]
    public void GetPoints_PointsOnSameRay_AreSpacedByTwoPiTimesStep(double spiralStep)
    {
        const int pointsCount = 20_000;
        const double maxRadialDeviation = 1.0;

        var center = new Point(0, 0);
        var spiral = new ArchimedeanSpiral(center, spiralStep);
        var turnDistance = 2 * Math.PI * spiralStep;

        var radii = spiral.GetPoints()
            .Take(pointsCount)
            .Where(p => p.Y == center.Y && p.X > center.X)
            .Select(p => (double)(p.X - center.X))
            .Where(radius => radius > turnDistance / 2)
            .ToList();

        radii.Should().NotBeEmpty();
        radii.Should().OnlyContain(radius => Math.Abs(radius - Math.Round(radius / turnDistance) * turnDistance) < maxRadialDeviation);
    }

    [TestCase(-1)]
    [TestCase(0)]
    public void Constructor_StepIsNotPositive_ThrowsArgumentOutOfRangeException(double spiralStep)
    {
        Action act = () => new ArchimedeanSpiral(Point.Empty, spiralStep: spiralStep);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestCase(-1)]
    [TestCase(0)]
    public void Constructor_PointSpacingIsNotPositive_ThrowsArgumentOutOfRangeException(double pointSpacing)
    {
        Action act = () => new ArchimedeanSpiral(Point.Empty, pointSpacing: pointSpacing);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
