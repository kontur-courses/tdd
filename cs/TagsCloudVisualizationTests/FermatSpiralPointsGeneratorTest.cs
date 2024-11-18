using System;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using TagsCloudVisualization.PointsGenerators;

namespace TagsCloudVisualizationTests;

[TestFixture]
[TestOf(typeof(FermatSpiralPointsGenerator))]
public class FermatSpiralPointsGeneratorTest
{

    [TestCase(-1, 1, Description = "Negative radius")]
     [TestCase(0, 1, Description = "Radius is zero")]
     [TestCase(1, 0, Description = "AngleOffset is zero")]
     [TestCase(1, -1, Description = "Negative angleOffset")]
    public void ShouldThrowArgumentException_AfterWrongCreation(double radius, double angleOffset)
    {
        var creation = () => new FermatSpiralPointsGenerator(radius, angleOffset);
        creation.Should().Throw<ArgumentException>();
        
    }

    [TestCaseSource(nameof(GeneratePointsTestCases))]
    public Point GeneratePoints_ShouldReturnCorrectPoint(double radius, double angleOffset, int pointNumber)
    {
        var pointsGenerator = new FermatSpiralPointsGenerator(radius, angleOffset);
        var actualPoint = pointsGenerator
            .GeneratePoints(new Point(0, 0))
            .Skip(pointNumber)
            .First();
        return actualPoint;
    }

    public static TestCaseData[] GeneratePointsTestCases =
    [
        new TestCaseData(1, 125, 0).Returns(new Point(0, 0)),
        new TestCaseData(10, 125, 1).Returns(new Point(-2, 3)),
        new TestCaseData(1, 360, 1).Returns(new Point(1, 0)),
        new TestCaseData(2, 180, 1).Returns(new Point(-1, 0)),
        new TestCaseData(4, 90, 1).Returns(new Point(0, 1)),
        new TestCaseData(4, 300, 1).Returns(new Point(2, -3))
    ];
}