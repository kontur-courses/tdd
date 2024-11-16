using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Base;
using TagsCloudVisualization.PointsGenerators;

namespace TagsCloudVisualizationTests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class ArchimedeanSpiralPointsGeneratorTests
{
    private const int Seed = 875556434;
    private readonly Random random = new(Seed);
    
    [TestCase(-1, 2, TestName = "RadiusLessThanZero")]
    [TestCase(4, 0, TestName = "AngleOffsetEqualZero")]
    public void ShouldThrowArgumentException(double radius, double angleOffset)
    {
        var pointsGeneratorConstructor = 
            () => new ArchimedeanSpiralPointsGenerator(radius, angleOffset);
        
        pointsGeneratorConstructor.Should().Throw<ArgumentException>();
    }
    
    [Test]
    [Repeat(5)]
    public void GeneratePoints_ShouldReturnPointsInSpiral()
    {
        var radius = random.Next(1, 100);
        var angleOffset = random.Next(1, 100);
        var pointsGenerator = new ArchimedeanSpiralPointsGenerator(radius, angleOffset);
        
        var actualPoints = pointsGenerator
            .GeneratePoints(new Point())
            .Take(1000);

        AssertPointsGeneratingInSpiral(radius, angleOffset, actualPoints);
    }

    private static void AssertPointsGeneratingInSpiral(double radius, double angleOffset, IEnumerable<Point> actualPoints)
    {
        var angle = 0d;
        
        foreach (var actualPoint in actualPoints)
        {
            var expectedRadius = radius * angle / 360;
            var expectedSector = PolarMath.GetSectorOfCircleFromDegrees(angle);
            var (actualRadius, actualAngle) = PolarMath.ConvertToPolarCoordinateSystem(actualPoint);
            var actualSector = PolarMath.GetSectorOfCircleFromRadians(actualAngle);
            var sectorOffset = actualSector - expectedSector;
            
            actualRadius.Should().BeApproximately(expectedRadius, 0.99);
            sectorOffset.Should().BeInRange(0, 1);
            
            angle += angleOffset;
        }
    }
}