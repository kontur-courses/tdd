using System.Drawing;
using AwesomeAssertions;

namespace TagsCloudVisualization.Tests;

public class ArchimedeanSpiralTests
{
    private const double DefaultSpiralAngleStep = Math.PI / 12;
    private const double DefaultSpiralRadiusCoefficient = 5.0;
    private const double DefaultStartRadius = 0;
    
    private readonly Point defaultCenterPoint = new Point(5, 10);
    
    [TestCase(0,0)]
    [TestCase(10,20)]
    [TestCase(-10,-20)]
    public void ArchimedeanSpiral_ReturnsCenter_WhenGettingFirstPoint(int centerX, int centerY)
    {
        var center = new Point(centerX, centerY);
        var spiral = new ArchimedeanSpiral(DefaultSpiralAngleStep, DefaultSpiralRadiusCoefficient, DefaultStartRadius, center);

        var firstPoint = spiral.GetNextCenterPosition();

        firstPoint.Should().Be(center);
    }

    [Test]
    public void ArchimedeanSpiral_ChangesSecondPoint_AfterGettingFirstPoint()
    {
        var spiral = new ArchimedeanSpiral(
            DefaultSpiralAngleStep, 
            DefaultSpiralRadiusCoefficient, 
            DefaultStartRadius, 
            defaultCenterPoint); 
        
        var firstPoint = spiral.GetNextCenterPosition();
        var secondPoint = spiral.GetNextCenterPosition();

        secondPoint.Should().NotBe(firstPoint);
    }

    [TestCase(-1, 5, 0)]
    [TestCase(1, -5, 0)]
    [TestCase(1, 5, -1)]
    public void Constructor_ThrowsArgumentOutOfRangeException_WhenArgumentIsNegative(
        double angleStep,
        double radiusCoefficient,
        double startRadius)
    {
        var action = () => new ArchimedeanSpiral(
            angleStep,
            radiusCoefficient,
            startRadius,
            defaultCenterPoint);

        action.Should().Throw<ArgumentOutOfRangeException>();
    }
    
    private static IEnumerable<TestCaseData> PointCases()
    {
        yield return new TestCaseData(
            new ArchimedeanSpiral(Math.PI / 2, 5, 0, new Point(5, 6)),
            new List<Point>
            {
                new Point(5, 6),
                new Point(5, 13)
            }
        ).SetName("StartRadius = 0, AngleStep = PI/2, RadiusCoefficient = 5, Center = (5, 6)");
        
        yield return new TestCaseData(
            new ArchimedeanSpiral(Math.PI / 2, 2, 1, new Point(5, 6)),
            new List<Point>
            {
                new Point(6, 6),
                new Point(5, 10),
                new Point(-2, 6)
            }
        ).SetName("StartRadius = 1, AngleStep = PI/2, RadiusCoefficient = 2, Center = (5, 6)");
    }
    
    [TestCaseSource(nameof(PointCases))]
    public void ArchimedeanSpiral_CalculatesPointsCorrectly(
        ArchimedeanSpiral spiral, List<Point> expectedPoints)
    {
        foreach (var expectedPoint in expectedPoints)
        {
            var actualPoint = spiral.GetNextCenterPosition();
            actualPoint.Should().Be(expectedPoint);
        }
    }
}