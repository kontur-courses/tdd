using System;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization;

[TestFixture]
public class CircularCloudLayouterTests
{
    CircularCloudLayouter circularCloudLayouter;

    [TearDown]
    public void TearDown()
    {
        var outputDirectory = $"{TestContext.CurrentContext.WorkDirectory}\\..\\..\\Failures";
        Directory.CreateDirectory(outputDirectory);

        if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
        {
            var testName = TestContext.CurrentContext.Test.Name;
            var imagePath = Path.Combine(outputDirectory, $"{testName}_.png");
            LayoutVisualizer.SaveLayoutImage(circularCloudLayouter, imagePath, 700, 700);

            TestContext.WriteLine($"Tag cloud visualization saved to file {imagePath}");
        }
    }

    [Test]
    public void Constructor_SetCenterCorrectly_WhenInitialized()
    {
        var center = new Point(-5, 2);
        circularCloudLayouter = new CircularCloudLayouter(center);
        var cloudCenter = new CircularCloudLayouter(center).Center;

        cloudCenter.Should().Be(center);
    }

    [Test]
    public void CloudSizeIsZero_WhenInitialized()
    {
        circularCloudLayouter = new CircularCloudLayouter(Point.Empty);

        var actualSize = circularCloudLayouter.Size();

        actualSize.Should().Be(Size.Empty);
    }

    [Test]
    public void CloudSizeEqualsFirstTagSize_WhenPuttingFirstTag()
    {
        circularCloudLayouter = new CircularCloudLayouter(Point.Empty);
        var rectangleSize = new Size(40, 20);

        circularCloudLayouter.PutNextRectangle(rectangleSize);

        var actualSize = circularCloudLayouter.Size();
        var expectedSize = rectangleSize;

        actualSize.Should().Be(expectedSize);
    }

    [Test]
    public void CloudSizeIsCloseToCircleShape_WhenPuttingManyTags()
    {
        circularCloudLayouter = new CircularCloudLayouter(Point.Empty);
        var rectangleSize = new Size(30, 12);

        for (var i = 0; i < 100; i++)
        {
            circularCloudLayouter.PutNextRectangle(rectangleSize);
        }

        var actualSize = circularCloudLayouter.Size();
        var aspectRatio = (double)actualSize.Width / actualSize.Height;

        aspectRatio.Should().BeInRange(0.5, 2.0);
    }

    [TestCase(0, TestName = "NoTags_WhenInitialized")]
    [TestCase(1, TestName = "SingleTag_WhenPuttingFirstTag")]
    [TestCase(10, TestName = "MultipleTags_WhenPuttingALotOfTags")]
    public void CloudContains(int rectangleCount)
    {
        circularCloudLayouter = new CircularCloudLayouter(Point.Empty);
        var rectangleSize = new Size(45, 17);

        for (var i = 0; i < rectangleCount; i++)
        {
            circularCloudLayouter.PutNextRectangle(rectangleSize);
        }

        circularCloudLayouter.Tags.Count.Should().Be(rectangleCount);
    }

    [TestCase(0, 1, TestName = "WidthIsZero")]
    [TestCase(1, 0, TestName = "HeightIsZero")]
    [TestCase(-1, 1, TestName = "WidthIsNegative")]
    [TestCase(1, -1, TestName = "HeightIsNegative")]
    public void PutNextRectangle_ThrowException_When(int width, int height)
    {
        circularCloudLayouter = new CircularCloudLayouter(Point.Empty);
        var incorrectRectangle = new Size(width, height);
        var putIncorrectRectangle = () => circularCloudLayouter.PutNextRectangle(incorrectRectangle);

        putIncorrectRectangle.Should().Throw<ArgumentException>();
    }

    [TestCase(100, 50, TestName = "LengthOfSidesRectangleIsEven")]
    [TestCase(117, 63, TestName = "LengthOfSidesRectangleIsOdd")]
    public void PutNextRectangle_PlacesFirstRectangleInCenter_When(int width, int height)
    {
        var center = new Point(3, -2);
        circularCloudLayouter = new CircularCloudLayouter(center);
        var rectangleSize = new Size(width, height);
        var firstRectangle = circularCloudLayouter.PutNextRectangle(rectangleSize);

        var rectangleCenter = new Point(
            firstRectangle.Left + firstRectangle.Width / 2,
            firstRectangle.Top - firstRectangle.Height / 2);

        rectangleCenter.Should().Be(center);
    }

    [Test]
    public void PutNextRectangle_CloudTagsIsNotIntersect_WhenPuttingALotOfTags()
    {
        circularCloudLayouter = new CircularCloudLayouter(Point.Empty);
        circularCloudLayouter.PutNextRectangle(new Size(50, 25));
        circularCloudLayouter.PutNextRectangle(new Size(60, 30));
        circularCloudLayouter.PutNextRectangle(new Size(40, 20));

        var tags = circularCloudLayouter.Tags;
        for (var i = 0; i < tags.Count; i++)
        {
            var currentRectangle = tags[i];
            tags
                .Where((_, j) => j != i)
                .All(otherRectangle => !currentRectangle.IntersectsWith(otherRectangle))
                .Should().BeTrue();
        }
    }
}