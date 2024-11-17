using FluentAssertions;
using System.Drawing;
using TagsCloudVisualization.PointsGenerators;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests.SpiralPointsGeneratorTests;

[TestFixture, Parallelizable(ParallelScope.All)]
public class SpiralPointsGeneratorTests
{
    [TestCase(0, 1, TestName = "WhenStepIsZero")]
    [TestCase(1, 0, TestName = "WhenAngleOffsetIsZero")]
    public void Constructor_ShouldThrowArgumentException(double step, double angleOffset)
    {
        var act = () => new SpiralPointsGenerator(new Point(0, 0), step, angleOffset);

        act.Should().Throw<ArgumentException>();
    }

    [TestCaseSource(nameof(GeneratePointsTestCases))]
    public void GetNextPointPosition_ShouldReturnCorrectPoint(double step, double angleOffset, int pointNumber, Point expectedPoint)
    {
        var pointsGenerator = new SpiralPointsGenerator(new Point(0, 0), step, angleOffset);

        var actualPoint = pointsGenerator.GetNextPointPosition();
        for (var i = 0; i < pointNumber - 1; i++)
            actualPoint = pointsGenerator.GetNextPointPosition();

        actualPoint.Should().Be(expectedPoint);
    }

    public static TestCaseData[] GeneratePointsTestCases =
    {
        new TestCaseData(0.1, 0.1, 1, new Point(0, 0)),
        new TestCaseData(1, 1, 1, new Point(0, 0)),
        new TestCaseData(1, 1, 3, new Point(0, 1)),
        new TestCaseData(1, 1, 5, new Point(-2, -3)),
        new TestCaseData(3, 1, 3, new Point(-2, 5)),
        new TestCaseData(3, 1, 5, new Point(-7, -9)),
        new TestCaseData(5, 1, 3, new Point(-4, 9)),
        new TestCaseData(5, 1, 5, new Point(-13, -15)),
    };
}