﻿using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;
using TagsCloudVisualization.Utils;

namespace Tests;

[TestFixture]
public class CircularCloudLayouterTests
{
    [SetUp]
    public void Setup()
    {
        circularLayouter = new CircularCloudLayouter(new Point(500, 500));
    }

    [TearDown]
    public void SaveLayoutImage_WhenFailed()
    {
        var testResult = TestContext.CurrentContext.Result.Outcome;
        var testName = TestContext.CurrentContext.Test.FullName;

        if (!Equals(testResult, ResultState.Failure)
            && !Equals(testResult == ResultState.Error)) return;

        var cloud = circularLayouter.GetCloud();
        var bitmap = CloudDrawer.DrawCloud(cloud, 1000, 1000);
        var path = @$"{Environment.CurrentDirectory}..\..\..\..\FailedTestsLayouts\{testName}.jpg";
        bitmap.Save(path);
        Console.WriteLine($"Tag cloud visualization saved to file {path}");
    }

    private CircularCloudLayouter circularLayouter;


    private static IEnumerable<TestCaseData> PutNextRectangleArgumentException => new[]
    {
        new TestCaseData(new Size(0, 1)).SetName("WhenGivenNotPositiveWidth"),
        new TestCaseData(new Size(1, 0)).SetName("WhenGivenNotPositiveHeight"),
        new TestCaseData(new Size(0, 0)).SetName("WhenGivenNotPositiveHeightAndWidth")
    };

    [TestCaseSource(nameof(PutNextRectangleArgumentException))]
    public void PutNextRectangle_ShouldThrowArgumentException(Size rectangleSize)
    {
        Assert.Throws<ArgumentException>(() => circularLayouter.PutNextRectangle(rectangleSize));
    }

    [Test]
    public void PutNextRectangle_ShouldReturnRectangleOfTheSameSize()
    {
        var rectangleSizeToPut = new Size(10, 10);

        var resultRectangle = circularLayouter.PutNextRectangle(rectangleSizeToPut);

        resultRectangle.Size.Should().Be(rectangleSizeToPut);
    }

    [Test]
    public void PutNextRectangle_ShouldReturnRectangleThatDoesNotIntersectWithAlreadyPutOnes()
    {
        var firstPutRectangle = circularLayouter.PutNextRectangle(new Size(10, 10));
        var secondPutRectangle = circularLayouter.PutNextRectangle(new Size(5, 5));

        var doesRectanglesIntersect = firstPutRectangle.IntersectsWith(secondPutRectangle);

        doesRectanglesIntersect.Should().BeFalse();
    }

    [Test]
    public void PutNextRectangle_ShouldPutRectanglesAtDistanceLessThanOne()
    {
        var firstPutRectangle = circularLayouter.PutNextRectangle(new Size(10, 10));
        var secondPutRectangle = circularLayouter.PutNextRectangle(new Size(5, 5));

        var distanceBetweenRectangles = Utils.CalculateShortestDistance(firstPutRectangle, secondPutRectangle);

        distanceBetweenRectangles.Should().BeLessThan(1);
    }

    [Test]
    public void PutNextRectangle_ShouldPutRectangleWithCenterInTheCloudCenter()
    {
        var center = circularLayouter.GetCloud().Center;

        var firstRectangle = circularLayouter.PutNextRectangle(new Size(10, 10));
        var firstRectangleCenter = Utils.GetRectangleCenter(firstRectangle);

        firstRectangleCenter.Should().Be(center);
    }

    private static IEnumerable<TestCaseData> _putRectanglesArgumentException = new[]
    {
        new TestCaseData(new List<Size>()).SetName("WhenGivenEmptyList")
    };

    [TestCaseSource(nameof(_putRectanglesArgumentException))]
    public void PutRectangles_ThrowsArgumentNullException(List<Size> sizes)
    {
        Assert.Throws<ArgumentException>(() => circularLayouter.PutRectangles(sizes));
    }


    [Test]
    public void GetLayout_ReturnsAsMuchRectanglesAsWasPut()
    {
        circularLayouter.PutNextRectangle(new Size(10, 10));
        circularLayouter.PutNextRectangle(new Size(5, 5));
        var rectanglesAmount = circularLayouter.GetCloud().Rectangles.Count;

        rectanglesAmount.Should().Be(2);
    }

    [Test]
    public void Layout_ShouldContainRectanglesWhichDoNotIntersectWithEachOther()
    {
        var rectanglesAmount = 20;

        var rectanglesLayout = GetRectanglesLayout(rectanglesAmount);

        for (var i = 0; i < rectanglesAmount; i++)
        for (var j = 0; j < rectanglesAmount; j++)
        {
            if (i == j) continue;
            var doesRectanglesIntersect = rectanglesLayout[i].IntersectsWith(rectanglesLayout[j]);
            doesRectanglesIntersect.Should().BeFalse();
        }
    }

    [Test]
    public void Layout_ShouldContainTightPlacedRectangles()
    {
        var rectanglesLayout = GetRectanglesLayout(100);
        var rectanglesSpace = rectanglesLayout.Sum(rect => rect.Width * rect.Height);
        var circleRadiusOfHorizontalAxe = circularLayouter.GetCloud().GetWidth() / 2;
        var circleRadiusOfVerticalAxe = circularLayouter.GetCloud().GetHeight() / 2;
        var circleOccupiedSpace = Math.PI * circleRadiusOfHorizontalAxe * circleRadiusOfVerticalAxe;
        var freeSpace = circleOccupiedSpace - rectanglesSpace;
        var freeSpaceProportionInPercentage = freeSpace / circleOccupiedSpace * 100;

        freeSpaceProportionInPercentage.Should().BeLessThan(30);
    }

    private List<Rectangle> GetRectanglesLayout(int rectanglesAmount)
    {
        var sizes = Utils.GenerateSizes(rectanglesAmount);
        circularLayouter.PutRectangles(sizes);
        return circularLayouter.GetCloud().Rectangles;
    }
}