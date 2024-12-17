using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagCloudTests;

public class CirclePositionDistributorTests
{
    private CirclePositionDistributor currentLayer;

    [SetUp]
    public void SetUp()
    {
        var center = new Point(5, 5);
        currentLayer = new CirclePositionDistributor(center);
    }

    [Test]
    public void GetNextRectanglePosition_ShouldTryPutRectangleOnCircle()
    {
        var firstRectangleLocation = currentLayer.GetNextPosition();
        var secondRectangleLocation = currentLayer.GetNextPosition();

        var radius = currentLayer.Center.CalculateDistanceBetween(firstRectangleLocation);

        radius.Should().Be(currentLayer.Center.CalculateDistanceBetween(secondRectangleLocation));
    }

    [Test]
    public void GetNextRectanglePosition_MustExpandCircle_WhenTryPutRectangleOnFullCircle()
    {
        var firstPoint = currentLayer.GetNextPosition();
        for (int i = 0; i < 360; i++)
        {
            currentLayer.GetNextPosition();
        }

        var finalPoint = currentLayer.GetNextPosition();

        finalPoint.CalculateDistanceBetween(currentLayer.Center).Should()
            .BeGreaterThan(firstPoint.CalculateDistanceBetween(currentLayer.Center));
    }
}