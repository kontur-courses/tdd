using FluentAssertions;
using NUnit.Framework;
using SixLabors.ImageSharp;
using System.Reflection;
using TagsCloudVisualization;
using static TagsCloud.Tests.TestConfiguration;

namespace TagsCloud.Tests;

[TestFixture]
public class LayoutTests
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

    [TearDown]
    public void TearDown()
    {
        var context = TestContext.CurrentContext;
        var writer = TestContext.Out;

        if (context.Result.FailCount == 0)
            return;

        var fileName = $"{TestContext.CurrentContext.Test.MethodName}-fail.png";
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

        var rects = typeof(Layout)
            .GetField("placedRectangles", BindingFlags.Instance | BindingFlags.NonPublic)!
            .GetValue(layout) as IList<RectangleF>;

        LayoutVisualizer.CreateVisualization(
            rects!,
            ImageCenter,
            Color.Red,
            1f,
            Color.White,
            filePath);

        writer.WriteLine($"Tag cloud visualization saved to file <{filePath}>");
    }

    private Layout layout;
    private Random random;

    [Test]
    public void PutNextRectangle_ShouldNot_SkipRectangles()
    {
        var rectCount = random.Next(1, 250);
        PutNRectanglesInLayout(rectCount);

        layout.RectangleCount.Should().Be(rectCount);
    }

    [Test]
    public void PlacedRectangles_ShouldNot_HaveIntersections()
    {
        var rectCount = random.Next(1, 250);
        var rects = PutNRectanglesInLayout(rectCount);

        RectanglesHaveIntersections(rects).Should().Be(false);
    }

    private IList<RectangleF> PutNRectanglesInLayout(int amount)
    {
        var rects = new List<RectangleF>(amount);

        for (var i = 0; i < amount; i++)
        {
            var size = new SizeF(random.Next(1, 250), random.Next(1, 250));
            rects.Add(layout.PutNextRectangle(size));
        }

        return rects;
    }

    private static bool RectanglesHaveIntersections(IList<RectangleF> rectangles)
    {
        return (from current in rectangles
            from another in rectangles
            where current != another
            where current.IntersectsWith(another)
            select current).Any();
    }
}