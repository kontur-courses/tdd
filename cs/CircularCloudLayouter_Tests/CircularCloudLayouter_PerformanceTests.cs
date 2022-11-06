using System.Drawing;
using CircularCloudLayouter;
using CircularCloudLayouter.WeightedLayouter;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;
using static FluentAssertions.FluentActions;

namespace TagsCloudVisualization_Tests;

[TestFixture]
public class CircularCloudLayouter_PerformanceTests
{
    private const int IterationsCount = 10_000;
    private static readonly Random Random = new();

    [TestCase(1, 1, TestName = "Small width and height")]
    [TestCase(100_000, 100_000, TestName = "Big width and height")]
    [TestCase(1, 100_000, TestName = "Small width and big height")]
    [TestCase(100_000, 1, TestName = "Big width and small height")]
    public void PutNextRectangle_WorksFast_OnEqualSizes(int width, int height)
    {
        var layouter = new WeightedCircularCloudLayouter(new Point(0, 0));
        var size = new Size(width, height);
        Invoking(() => PutEqualsRects(layouter, size)).ExecutionTime().Should().BeLessThan(10.Seconds());
    }

    private static void PutEqualsRects(ICircularCloudLayouter layouter, Size size)
    {
        for (var i = 0; i < IterationsCount; i++)
            layouter.PutNextRectangle(size);
    }

    [Test]
    public void PutNextRectangle_WorksFast_OnRandomSizes()
    {
        var layouter = new WeightedCircularCloudLayouter(new Point(0, 0));
        Invoking(() => PutRandomRects(layouter)).ExecutionTime().Should().BeLessThan(1.Seconds());
    }

    private static void PutRandomRects(ICircularCloudLayouter layouter)
    {
        for (var i = 0; i < IterationsCount; i++)
        {
            var size = new Size(
                Random.Next(1, 100_000),
                Random.Next(1, 100_000)
            );
            layouter.PutNextRectangle(size);
        }
    }
}