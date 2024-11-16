using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualizationTests;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class PointExtensionsTests
{
    private const int MinIntValue = -100;
    private const int MaxIntValue = 100;
    private const int Seed = 23478932;
    private readonly Random random = new(Seed);
    
    [Test]
    [Repeat(10)]
    public void Subtract_ShouldReturnExpectedPoint()
    {
        var point1 = GetRandomPoint();
        var point2 = GetRandomPoint();
        
        var actualPoint = point1.Subtract(point2);
        var expectedPoint = new Point(point1.X - point2.X, point1.Y - point2.Y);
        
        actualPoint.Should().BeEquivalentTo(expectedPoint);
    }

    [Test]
    [Repeat(10)]
    public void Abs_ShouldReturnExpectedPoint()
    {
        var point = GetRandomPoint();
        
        var actualPoint = point.Abs();
        var expectedPoint = new Point(Math.Abs(point.X), Math.Abs(point.Y));
        
        actualPoint.Should().BeEquivalentTo(expectedPoint);
    }

    private Point GetRandomPoint() => 
        random.NextPoint(MinIntValue, MaxIntValue);
}