using FluentAssertions;
using NUnit.Framework.Interfaces;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization.Tests;

[TestFixture]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class CircularCloudLayouterTests
{
    private CircularCloudLayouter _layouter = null!;

    [TestCase(0, 0, 60, 40)]
    [TestCase(100, 200, 20, 80)]
    [TestCase(-100, -200, 40, 20)]
    [TestCase(50, -50, 100, 100)]
    public void PutNextRectangle_FirstRectangle_PlacesAtCenter(
        int x, int y, int width, int height)
    {
        _layouter = new CircularCloudLayouter(new Point(x, y));
        var size = new Size(width, height);

        var rectangle = _layouter.PutNextRectangle(size);

        rectangle.Size.Should().Be(size);
        rectangle.Location.Should().Be(
            new Point(x - width / 2, y - height / 2));
    }

    [TestCase(0, 20)]
    [TestCase(20, 0)]
    [TestCase(-10, 20)]
    [TestCase(20, -10)]
    [TestCase(0, 0)]
    public void PutNextRectangle_InvalidSize_ThrowsException(int width, int height)
    {
        _layouter = new CircularCloudLayouter(new Point(0, 0));
        var size = new Size(width, height);

        var act = () => _layouter.PutNextRectangle(size);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Test]
    public void PutNextRectangle_RectanglesDoNotIntersect()
    {
        _layouter = new CircularCloudLayouter(new Point(0, 0));
        Size[] sizes = [new(60, 40), new(30, 20), new(80, 30), new(20, 20), new(40, 60)];

        var rectangles = sizes
            .Select(_layouter.PutNextRectangle)
            .ToArray();

        for (var i = 0; i < rectangles.Length; i++)
            for (var j = i + 1; j < rectangles.Length; j++)
                rectangles[i].IntersectsWith(rectangles[j])
                    .Should().BeFalse();
    }

    [Test]
    public void PutNextRectangle_PreservesRectangleSize()
    {
        _layouter = new CircularCloudLayouter(Point.Empty);

        Size[] sizes = [new(60, 40), new(31, 21), new(80, 15)];

        var rectangles = sizes
            .Select(_layouter.PutNextRectangle)
            .ToArray();

        rectangles.Select(r => r.Size).Should().Equal(sizes);
    }

    [TearDown]
    public void SaveLayoutOnFailure()
    {
        var context = TestContext.CurrentContext;

        if (context.Result.Outcome.Status != TestStatus.Failed || _layouter is null)
            return;

        var padding = 10;
        var renderer = new CloudRenderer(
            padding,
            Pens.Black,
            Brushes.LightGray,
            Color.White);

        using var image = renderer.Render(_layouter.Rectangles);

        var directory = Path.Combine(context.WorkDirectory, "FailedLayouts");
        Directory.CreateDirectory(directory);

        var fileName = $"{context.Test.MethodName}_{DateTime.Now:HHmmssfff}.png";
        var path = Path.Combine(directory, fileName);
        image.Save(path, ImageFormat.Png);

        TestContext.Out.WriteLine($"Tag cloud visualization saved to file {path}");
    }
}
