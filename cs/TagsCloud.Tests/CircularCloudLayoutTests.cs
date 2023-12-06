using FluentAssertions;
using NUnit.Framework;
using SixLabors.ImageSharp;
using TagsCloudVisualization;
using static TagsCloud.Tests.TestConfiguration;

namespace TagsCloud.Tests;

[TestFixture]
public class CircularCloudLayoutTests
{
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        random = new Random();
    }

    [SetUp]
    public void SetUp()
    {
        var layoutFunction = new Spiral(random.Next(1, 25), random.NextSingle());
        var screenCenter = new PointF(WindowWidth / 2, WindowHeight / 2);
        layout = new Layout(layoutFunction, screenCenter);
    }

    private Layout layout;
    private Random random;

    [Test]
    public void PutNextRectangle_ShouldNot_SkipRectangles()
    {
        var rectCount = random.Next(1, 250);
        PutNRectanglesInLayout(rectCount);

        layout.PlacedFigures.Should().HaveCount(rectCount);
    }

    [Test]
    public void PlacedRectangles_ShouldNot_HaveIntersections()
    {
        var rectCount = random.Next(1, 250);
        PutNRectanglesInLayout(rectCount);

        PlacedRectanglesHaveIntersections().Should().Be(false);
    }

    private void PutNRectanglesInLayout(int amount)
    {
        for (var i = 0; i < amount; i++)
        {
            var currentSize = new Size(random.Next(1, 250), random.Next(1, 250));
            layout.PutNextRectangle(currentSize);
        }
    }

    private bool PlacedRectanglesHaveIntersections()
    {
        var rects = layout.PlacedFigures;

        return (from current in rects
            from another in rects
            where current != another
            where current.IntersectsWith(another)
            select current).Any();
    }
}