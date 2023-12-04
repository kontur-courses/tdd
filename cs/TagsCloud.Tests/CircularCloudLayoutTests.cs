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
        var sizes = GetRandomLengthSizesArray().ToArray();
        _ = sizes.Select(size => layout.PutNextRectangle(size)).ToList();

        layout.PlacedRectangles.Should().HaveCount(sizes.Length);
    }

    [Test]
    public void PlacedRectangles_ShouldNot_HaveIntersections()
    {
        var sizes = GetRandomLengthSizesArray();
        _ = sizes.Select(size => layout.PutNextRectangle(size)).ToList();

        HasIntersections(layout.PlacedRectangles).Should().Be(false);
    }

    private IEnumerable<Size> GetRandomLengthSizesArray()
    {
        return Enumerable
            .Range(0, random.Next(1, 500))
            .Select(rect => new Size(random.Next(1, 150), random.Next(1, 150)))
            .ToArray();
    }

    private static bool HasIntersections(IList<RectangleF> rects)
    {
        return (from current in rects
            from another in rects
            where current != another
            where current.IntersectsWith(another)
            select current).Any();
    }
}