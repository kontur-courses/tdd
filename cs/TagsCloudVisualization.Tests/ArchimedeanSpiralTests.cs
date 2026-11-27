using System.Drawing;
using FluentAssertions;

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

        var firstPoint = spiral.GetSpiralPoints().First();

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

        var radii = spiral.GetSpiralPoints()
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
    public void Constructor_StepIsZeroOrNegative_ThrowsArgumentOutOfRangeException(double spiralStep)
    {
        var center = new Point(0, 0);

        Action act = () => new ArchimedeanSpiral(center, spiralStep);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
