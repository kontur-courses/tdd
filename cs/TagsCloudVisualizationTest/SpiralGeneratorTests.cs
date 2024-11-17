using NUnit.Framework;
using FluentAssertions;
using System.Drawing;
using TagsCloudVisualization;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualizationTest;

public class SpiralGeneratorTests
{
    private Point center;
    private SpiralGenerator generator;

    [SetUp]
    public void Setup()
    {
        center = new Point(0, 0);
        generator = new SpiralGenerator(center);
    }

    [Test]
    public void Constructor_WithNegativeSpiralStep_ThrowsArgumentException()
    {
        Action act = () => new SpiralGenerator(center, -1);
        
        act.Should().Throw<ArgumentException>()
           .WithMessage("Шаг спирали должен быть больше 0");
    }

    [Test]
    public void GetNextPoint_FirstPoint_ShouldBeAtCenter()
    {
        var firstPoint = generator.GetNextPoint();
        
        firstPoint.Should().Be(center);
    }
    
    [Test]
    public void GetNextPoint_ShouldGeneratePointsWithIncreasingDistanceFromCenter()
    {
        const int numberOfPoints = 50;
        var initialPoint = generator.GetNextPoint();
        var initialDistance = initialPoint.DistanceFromCenter(center);
        const int halfwayPoint = numberOfPoints / 2;
        
        for (var i = 0; i < halfwayPoint; i++)
        {
            generator.GetNextPoint();
        }
        
        var middlePoint = generator.GetNextPoint();
        var middleDistance = middlePoint.DistanceFromCenter(center);

     
        for (var i = halfwayPoint; i < numberOfPoints; i++)
        {
            generator.GetNextPoint();
        }
        
        var finalPoint = generator.GetNextPoint();
        var finalDistance = finalPoint.DistanceFromCenter(center);

        finalDistance.Should().BeGreaterThan(middleDistance)
            .And.BeGreaterThan(initialDistance);
    }

    [Test]
    public void GetNextPoint_ShouldNotGenerateConsecutiveDuplicatePoints()
    {
        const int numberOfPoints = 100;
        Point? previousPoint = null;

        for (var i = 0; i < numberOfPoints; i++)
        {
            var currentPoint = generator.GetNextPoint();
            if (previousPoint != null)
            {
                currentPoint.Should().NotBe(previousPoint, 
                    "последовательные точки не должны повторяться");
            }
            previousPoint = currentPoint;
        }
    }
}