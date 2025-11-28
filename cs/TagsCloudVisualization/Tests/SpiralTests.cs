using NUnit.Framework;
using System.Drawing;
using System.Linq;
using FluentAssertions;

namespace TagCloud.Tests;
    
[TestFixture]
public class SpiralTests
{
    private Spiral spiral = new Spiral(new(), 0.1, 1.0);

    [SetUp]
    public void SetUp()
    {
        spiral = new Spiral(new Point(400, 400), 0.1, 1.0);
    }

    [Test]
    [TestCase(0, 1.0, TestName = "WithZeroDeltaAngle")]
    [TestCase(0.1, -1.0, TestName = "WithNegativeDeltaRadius")]
    public void Constructor_OnWrongArguments_ShouldThrow(double deltaAngle, double deltaRadius)
    {
        var act = () => new Spiral(new(), deltaAngle, deltaRadius);
        
        act.Should().Throw();
    }

    [Test]
    public void GetPointsOnSpiral_FirstPoint_ShouldBeAtCenter()
    {
        var firstPoint = spiral.GetPointsOnSpiral().First();

        firstPoint.Should().Be(new Point(400, 400));
    }

    [Test]
    public void GetPointsOnSpiral_Points_ShouldMoveAwayFromCenter()
    {
        var points = spiral.GetPointsOnSpiral().Take(5).ToList();
        var distances = points.Select(p => Math.Sqrt(p.X * p.X + p.Y * p.Y)).ToList();
        
        for (int i = 1; i < distances.Count; i++)
        {
            distances[i].Should().BeGreaterThan(distances[i - 1]);
        }
    }

    [Test]
    public void ConvertFromPolarCoordinates_WithZeroRadius_ShouldReturnZeroPoint()
    {
        var point = Spiral.ConvertFromPolarCoordinates(1.0, 0.0);

        point.Should().Be(new Point(0, 0));
    }

    [Test]
    public void ConvertFromPolarCoordinates_With45DegreeAngle_ShouldReturnEqualCoordinates()
    {
        var angle = Math.PI / 4; // 45°
        var radius = 10.0;

        var point = Spiral.ConvertFromPolarCoordinates(angle, radius);

        point.X.Should().Be(8);
        point.Y.Should().Be(8);
    }

    [Test]
    public void GetPointsOnSpiral_ShouldNotProduceDuplicatePoints()
    {
        var spiral = new Spiral(new(), 0.5, 2.0);

        var points = spiral.GetPointsOnSpiral().Take(100).ToList();

        var distinctPoints = points.Distinct();
        points.Count.Should().Be(distinctPoints.Count());
    }
}