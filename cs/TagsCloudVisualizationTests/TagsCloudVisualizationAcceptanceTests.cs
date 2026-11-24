using FluentAssertions;
using System.Drawing;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class TagsCloudVisualizationAcceptanceTests
{
    private CircularCloudLayouter _layouter;
    private readonly Point _center = new(50, 50);

    [SetUp]
    public void Setup()
    {
        _layouter = new CircularCloudLayouter(_center);
    }

    [TearDown]
    public void SaveTagCloudWhenTestFailed()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
        {
            var testName = TestContext.CurrentContext.Test.Name;
            var fileName = $"FailedTest_{testName}_{DateTime.Now:HH_mm_ss}.png";
            var path = Path.Combine(TestContext.CurrentContext.WorkDirectory, fileName);
            TagCloudSaver.Save(_layouter, path, 100, 100);
            Console.WriteLine($"Tag cloud visualization saved to file {path}");
        }
    }

    [Test]
    public void PutNextRectangle_ReturnsFirstRectangleInCenter()
    {
        _layouter.PutNextRectangle(new Size(10, 15))
            .Should().BeEquivalentTo(new Rectangle((int)(50 - 10 / 2.0), (int)(50 - 15 / 2.0), 10, 15));
    }

    [Test]
    public void PutNextRectangle_WhenSecondRectangle_ReturnsRectangleNotIntersectsWithFirst()
    {
        var rect1 = _layouter.PutNextRectangle(new Size(10, 15));
        var rect2 = _layouter.PutNextRectangle(new Size(13, 7));
        rect1.IntersectsWith(rect2).Should().BeFalse();
        rect2.IntersectsWith(rect1).Should().BeFalse();
    }

    [Test]
    public void WithRectangle_ReturnsSameLayouter()
    {
        var layouter = _layouter.WithRectangle(new Size(10, 15));
        layouter.Should().BeSameAs(_layouter);
    }

    [Test]
    public void PutNextRectangle_WhenManyRectangles_ReturnsRectanglesNotIntersects()
    {
        var sizes = new[]
        {
            new Size(10, 15),
            new Size(13, 7),
            new Size(5, 5),
            new Size(20, 10),
            new Size(8, 12),
            new Size(1, 1),
            new Size(2, 2)
        };
        var rectangles = sizes.Select(_layouter.PutNextRectangle).ToList();
        for (int i = 0; i < rectangles.Count; i++)
        {
            for (int j = i + 1; j < rectangles.Count; j++)
            {
                rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
                rectangles[j].IntersectsWith(rectangles[i]).Should().BeFalse();
            }
        }
    }

    [Test]
    public void PutNextRectangle_WhenManyRectangles_ReturnsRectanglesFormCircle()
    {
        var sizes = new[]
        {
            new Size(10, 15),
            new Size(13, 7),
            new Size(5, 5),
            new Size(20, 10),
            new Size(8, 12),
            new Size(1, 1),
            new Size(2, 2)
        };
        var rectangles = sizes.Select(_layouter.PutNextRectangle).ToList();
        var minX = rectangles.Min(r => r.Left);
        var minY = rectangles.Min(r => r.Top);
        var maxX = rectangles.Max(r => r.Right);
        var maxY = rectangles.Max(r => r.Bottom);
        var cloudWidth = maxX - minX;
        var cloudHeight = maxY - minY;
        ((double)(Math.Max(cloudWidth, cloudHeight) / (double)Math.Min(cloudWidth, cloudHeight))).Should().BeLessThan(2);
    }

    [Test]
    public void PutNextRectangle_WhenManyRectangles_CloudBeCompact()
    {
        var sizes = new[]
        {
            new Size(10, 15),
            new Size(13, 7),
            new Size(5, 5),
            new Size(20, 10),
            new Size(8, 12),
            new Size(1, 1),
            new Size(2, 2)
        };
        var rectangles = sizes.Select(_layouter.PutNextRectangle).ToList();
        var rectangleAreas = rectangles.Sum(r => r.Width * r.Height);
        var minX = rectangles.Min(r => r.Left);
        var minY = rectangles.Min(r => r.Top);
        var maxX = rectangles.Max(r => r.Right);
        var maxY = rectangles.Max(r => r.Bottom);
        var cloudWidth = maxX - minX;
        var cloudHeight = maxY - minY;
        var cloudRectangleArea = cloudWidth * cloudHeight;

        (cloudRectangleArea / (double)rectangleAreas).Should().BeLessThan(2);
    }
}
