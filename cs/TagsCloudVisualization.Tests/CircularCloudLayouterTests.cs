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

    [Test]
    public void PutNextRectangle_FirstRectangle_PlacesAtCenter()
    {
        _layouter = new CircularCloudLayouter(new Point(0, 0));
        var size = new Size(60, 40);

        var rectangle = _layouter.PutNextRectangle(size);

        rectangle.Should().Be(new Rectangle(-30, -20, 60, 40));
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

        var path = Path.Combine(context.WorkDirectory, $"{Guid.NewGuid()}.png");
        image.Save(path, ImageFormat.Png);

        TestContext.Out.WriteLine($"Tag cloud visualization saved to file {path}");
    }
}
