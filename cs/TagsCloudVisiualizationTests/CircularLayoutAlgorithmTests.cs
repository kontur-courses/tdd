using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisiualizationTests.Utils;
using TagsCloudVisualization.LayoutRectanglesInCloudAlgorithms;

namespace TagsCloudVisiualizationTests;

[TestFixture]
public class CircularLayoutAlgorithmTests
{
    [TestCase(0)]
    [TestCase(-5)]
    public void Constructor_ShouldArgumentException_WhenStepIncreasingRadiusHasInvalidValue(int stepIncreasingRadius)
    {
        var action = () =>
        {
            _ = new CircularLayoutAlgorithm(new Point(0, 0), stepIncreasingRadius: stepIncreasingRadius);
        };

        action
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("The parameter 'stepIncreasingRadius' is less than or equal to zero");
    }

    [Test]
    public void Constructor_ShouldArgumentException_WhenStepIncreasingAngleIsZero()
    {
        var action = () =>
        {
            _ = new CircularLayoutAlgorithm(new Point(0, 0), stepIncreasingAngle: 0);
        };

        action
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("The parameter 'stepIncreasingAngle' is zero");
    }

    [TestCase(0, 0)]
    [TestCase(-4, 5)]
    public void CalculateNextPoint_ShouldPointIsCenter_WhenCalculateFirstPoint(int centerCoordinateX, int centerCoordinateY)
    {
        var center = new Point(centerCoordinateY, centerCoordinateY);
        var circularLayoutAlgorithm = new CircularLayoutAlgorithm(center);

        var nextPoint = circularLayoutAlgorithm.CalculateNextPoint();

        nextPoint.Should().Be(center);
    }

    [TestCase(2)]
    [TestCase(7)]
    public void CalculateNextPoint_ShouldIncreaseRadius_WhenCalculateTwoPoints(int stepIncreasingRadius)
    {
        var circularLayoutAlgorithm =
            new CircularLayoutAlgorithm(new Point(0, 0), stepIncreasingRadius: stepIncreasingRadius);

        var firstPoint = circularLayoutAlgorithm.CalculateNextPoint();
        var secondPoint = circularLayoutAlgorithm.CalculateNextPoint();
        var distanceBetweenPoints = GeometryUtils.CalculateDistanceBetweenPoints(firstPoint, secondPoint);
        
        distanceBetweenPoints.Should().Be(stepIncreasingRadius);
    }

    [TestCase(Math.PI / 4)]
    [TestCase(Math.PI / 2)]
    public void CalculateNextPoint_ShouldIncreaseAngle_WhenCalculateThreePoints(double stepIncreasingAngle)
    {
        var stepIncreasingRadius = 2;
        var center = new Point(0, 0);
        var circularLayoutAlgorithm = new CircularLayoutAlgorithm(
            center,
            stepIncreasingRadius: stepIncreasingRadius,
            stepIncreasingAngle: stepIncreasingAngle
            );
        
        _ = circularLayoutAlgorithm.CalculateNextPoint();
        var secondPoint = circularLayoutAlgorithm.CalculateNextPoint();
        var thirdPoint = circularLayoutAlgorithm.CalculateNextPoint();
        var distanceBetweenCenterAndSecondPoint = GeometryUtils.CalculateDistanceBetweenPoints(center, secondPoint);
        var distanceBetweenCenterAndThirdPoint = GeometryUtils.CalculateDistanceBetweenPoints(center, thirdPoint);
        
        // проверяем, что точки не равны
        secondPoint.Should().NotBe(thirdPoint);
        // проверяем, что точки в пределах круга с одним радиусом
        distanceBetweenCenterAndSecondPoint.Should().BeLessThanOrEqualTo(stepIncreasingRadius);
        distanceBetweenCenterAndThirdPoint.Should().BeLessThanOrEqualTo(stepIncreasingRadius);
    }
}