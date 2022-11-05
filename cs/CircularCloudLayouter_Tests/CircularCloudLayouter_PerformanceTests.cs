using System.Drawing;
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
    private readonly Random _random = new();

    [TestCase(1, 1, 1, 1, TestName = "Small equal values")]
    [TestCase(10_000_000, 10_000_000, 10_000_000, 10_000_000, TestName = "Big equal values")]
    [TestCase(1, 1, 10_000_000, 10_000_000, TestName = "Small width and big height equal values")]
    [TestCase(10_000_000, 10_000_000, 1, 1, TestName = "Big width and small height equal values")]
    [TestCase(1, 10_000_000, 1, 10_000_000, TestName = "Different values")]
    public void PutNextRectangle_WorksFast_OnAnySize(int minWidth, int maxWidth, int minHeight, int maxHeight)
    {
        Invoking(() => PutRects(minWidth, maxWidth, minHeight, maxHeight))
            .ExecutionTime().Should().BeLessThan(1.Seconds());
    }

    private void PutRects(int minWidth, int maxWidth, int minHeight, int maxHeight)
    {
        var layouter = new WeightedCircularCloudLayouter(new Point(0, 0));
        for (var i = 0; i < IterationsCount; i++)
        {
            var size = new Size(
                _random.Next(minWidth, maxWidth + 1),
                _random.Next(minHeight, maxHeight + 1)
            );
            layouter.PutNextRectangle(size);
        }
    }
}