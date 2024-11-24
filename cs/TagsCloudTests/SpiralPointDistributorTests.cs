using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudTests;

public class SpiralPointDistributorTests
{
    [TestCase(0, 1)]
    [TestCase(1, 0)]
    public void SpiralPointDistributor_Throws_WithInvalidParameters(float angleStep, float radiusStep)
    {
        var action = () => new SpiralPointDistributor(new PointF(0, 0), angleStep, radiusStep);
        action.Should().Throw<ArgumentException>();
    }
    
    [Test]
    public void GetNextPoint_Returns_DifferentPoints()
    {
        var center = new PointF(0, 0);
        var distributor = new SpiralPointDistributor(center);
        var points = new List<PointF>();
        for (var i = 0; i < 100; i++)
        {
            var point = distributor.GetNextPoint();
            points.Contains(point).Should().BeFalse();
            points.Add(point);
        }
    }

    [Test]
    [Description("Возвращаемые точки должны соответствовать распределению")]
    public void GetNextPoint_Returns_CorrectPoints()
    {
        var center = new PointF(0, 0);
        var expectedPoints = new List<PointF>
        {
            new(0, 0),
            new(1, 0),
            new(0.9998477f, 0.017452406f),
            new(0.99939084f, 0.034899496f),
            new(0.9986295f, 0.05233596f),
            new(0.9975641f, 0.06975647f),
            new(0.9961947f, 0.08715574f),
            new(0.9945219f, 0.10452846f),
            new(0.99254614f, 0.12186934f),
            new(0.99026805f, 0.1391731f)
        };
        var distributor = new SpiralPointDistributor(center);
        for (var i = 0; i < 10; i++)
            distributor.GetNextPoint().Should().Be(expectedPoints[i]);
    }
}