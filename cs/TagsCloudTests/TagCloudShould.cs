using System.Drawing;
using FluentAssertions;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudTests;

[TestFixture]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class TagCloudShould
{
    private readonly Random _random = new();
    private readonly Point _defaultCenter = new(1, 1);
    private readonly string _failedTestsPictureFolder = "FailedPictures";
    private readonly ICloudLayouter _defaultLayouter;
    private TagCloud _defaultTagCloud;

    public TagCloudShould()
    {
        _defaultLayouter = new CircularCloudLayouter(_defaultCenter);
        _defaultTagCloud = new TagCloud(_defaultLayouter);
    }

    [TearDown]
    public void TearDown()
    {
        var context = TestContext.CurrentContext;
        if (context.Result.Outcome.Status != TestStatus.Failed)
            return;

        Directory.CreateDirectory(_failedTestsPictureFolder);
        var fileName = $"{context.Test.MethodName}{_random.Next()}";
        var filePath = Path.Combine(_failedTestsPictureFolder, fileName);

        TagCloudVisualization.SaveTagCloudAsBitmap(_defaultTagCloud, filePath + ".bmp");

        TestContext.WriteLine($"Tag cloud visualization saved to file {filePath}");
    }

    [Test]
    [Repeat(5)]
    public void PlacedInSelectedCenter_AfterCreation()
    {
        var x = _random.Next(0, 10);
        var y = _random.Next(0, 10);
        var center = new Point(x, y);
        var firstRectangleSize = new Size(2, 2);
        var circularCloudLayouter = new CircularCloudLayouter(center);
        var tagCloud = new TagCloud(circularCloudLayouter);

        tagCloud.AddNextRectangleWith(firstRectangleSize);

        tagCloud.Rectangles[0]
            .GetCentralPoint()
            .Should()
            .Be(center);
    }

    [Test]
    public void HaveZeroSize_AfterCreation()
    {
        _defaultTagCloud.Width
            .Should()
            .Be(0);
        _defaultTagCloud.Height
            .Should()
            .Be(0);
    }

    [Test]
    [Repeat(5)]
    public void HaveExpectedWidth_AfterAddedFirstRectangle()
    {
        var width = _random.Next(1, 100);
        var newSize = new Size(width, 2);

        _defaultTagCloud.AddNextRectangleWith(newSize);

        _defaultTagCloud.Width
            .Should()
            .Be(width);
    }

    [Test]
    [Repeat(5)]
    public void HaveExpectedHeight_AfterAddedFirstRectangle()
    {
        var height = _random.Next(1, 100);
        var newSize = new Size(2, height);

        _defaultTagCloud.AddNextRectangleWith(newSize);

        _defaultTagCloud.Height
            .Should()
            .Be(height);
    }

    [Test]
    [Repeat(5)]
    public void HaveExpectedWidth_AfterAddedManyRectangles()
    {
        var width = _random.Next(1, 100);
        var newSize = new Size(width, 2);
        var rectanglesCount = _random.Next(5, 40);
        var expectedWidth = rectanglesCount * width;

        for (var i = 0; i < rectanglesCount; i++)
            _defaultTagCloud.AddNextRectangleWith(newSize);

        _defaultTagCloud.Width
            .Should()
            .BeLessThanOrEqualTo(expectedWidth);
    }

    [Test]
    [Repeat(5)]
    public void HaveExpectedHeight_AfterAddedManyRectangles()
    {
        var height = _random.Next(1, 100);
        var newSize = new Size(2, height);
        var rectanglesCount = _random.Next(5, 40);
        var expectedHeight = rectanglesCount * height;

        for (var i = 0; i < rectanglesCount; i++)
            _defaultTagCloud.AddNextRectangleWith(newSize);

        _defaultTagCloud.Height
            .Should()
            .BeLessThanOrEqualTo(expectedHeight);
    }

    [Test]
    [Repeat(5)]
    public void AddNextRectangleWith_AddedExpectedNumberOfRectangles_AfterManyExecutions()
    {
        var expectedCount = _random.Next(0, 10);
        var unionSize = new Size(2, 2);

        for (var i = 0; i < expectedCount; i++)
            _defaultTagCloud.AddNextRectangleWith(unionSize);

        _defaultTagCloud.Rectangles
            .Should()
            .HaveCount(expectedCount);
    }
}