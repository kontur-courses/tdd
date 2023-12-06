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
        distanceDelta = random.Next(1, 25);
        angleDelta = random.NextSingle();
        layout = new CircularCloudLayout(new PointF(WindowWidth, WindowHeight),
            distanceDelta, angleDelta);
    }

    private CircularCloudLayout layout;
    private Random random;

    private float distanceDelta, angleDelta;

    [Test]
    public void PutNextRectangle_ShouldNot_SkipRectangles()
    {
        var rectCount = random.Next(1, 250);
        PutNRectanglesInLayout(rectCount);

        layout.PlacedRectangles.Should().HaveCount(rectCount);
    }

    [Test]
    public void PlacedRectangles_ShouldNot_HaveIntersections()
    {
        var rectCount = random.Next(1, 250);
        PutNRectanglesInLayout(rectCount);

        PlacedRectanglesHasIntersections().Should().Be(false);
    }

    private void PutNRectanglesInLayout(int amount)
    {
        for (var i = 0; i < amount; i++)
        {
            var currentSize = new SizeF(random.Next(1, 250), random.Next(1, 250));
            layout.PutNextRectangle(currentSize);
        }
    }

    private bool PlacedRectanglesHasIntersections()
    {
        var rects = layout.PlacedRectangles;
        
        return (from current in rects
            from another in rects
            where current != another
            where current.IntersectsWith(another)
            select current).Any();
    }
}