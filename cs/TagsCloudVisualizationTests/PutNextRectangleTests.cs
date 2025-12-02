using System.Drawing;
using System.Runtime.Versioning;
using FluentAssertions;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;
using static TagsCloudVisualization.CloudVisualizer;

namespace TagsCloudVisualizationTests;

[TestFixture]
[SupportedOSPlatform("windows")]
public class PutNextRectangleEdgeCasesTests
{
    private List<Size> generatedRectSizes;

    [SetUp]
    public void SetUp()
    {
        generatedRectSizes = new List<Size>();
    }

    [TearDown]
    public void TearDown()
    {
        var context = TestContext.CurrentContext;
        if (context.Result.Outcome.Status != TestStatus.Failed) return;

        try
        {
            var dir = Path.Combine(context.WorkDirectory, "failures");
            Directory.CreateDirectory(dir);
            var fileName = $"{context.Test.Name}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            var fullPath = Path.Combine(dir, fileName);

            GenerateRectanglesCloud(fullPath, generatedRectSizes);

            TestContext.Out.WriteLine($"Tag cloud visualization saved to file {fullPath}");
        }
        catch (Exception e)
        {
            TestContext.Out.WriteLine("Failed to save visualization: " + e);
        }
    }

    [Test]
    public void PutNextRectangle_ShouldThrow_OnZeroWidth()
    {
        var layouter = new CircularCloudLayouter(new Point(0, 0));
        var size = new Size(0, 10);

        Action act = () => layouter.PutNextRectangle(size);
        generatedRectSizes.Add(size);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Rectangle width must be positive");
    }

    [Test]
    public void PutNextRectangle_ShouldThrow_OnZeroHeight()
    {
        var layouter = new CircularCloudLayouter(new Point(0, 0));
        var size = new Size(10, 0);

        Action act = () => layouter.PutNextRectangle(size);
        generatedRectSizes.Add(size);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*Rectangle height must be positive*");
    }

    [Test]
    public void PutNextRectangle_ShouldThrow_OnNegativeWidthOrHeight()
    {
        var layouter = new CircularCloudLayouter(new Point(0, 0));

        var badSizes = new[] { new Size(-1, 5), new Size(5, -1), new Size(-10, -10) };
        foreach (var s in badSizes)
        {
            Action act = () => layouter.PutNextRectangle(s);
            generatedRectSizes.Add(s);
            act.Should().Throw<ArgumentException>();
        }
    }

    [Test]
    public void PutNextRectangle_ManyRandomSizes_ShouldNotIntersectEachOther()
    {
        var rnd = new Random(0);
        var layouter = new CircularCloudLayouter(new Point(200, 200));
        var sizes = new List<Size>();

        for (var i = 0; i < 150; i++)
        {
            var w = rnd.Next(5, 60);
            var h = rnd.Next(5, 60);
            var s = new Size(w, h);
            sizes.Add(s);
            generatedRectSizes.Add(s);

            var rect = layouter.PutNextRectangle(s);
                
            rect.Width.Should().Be(w);
            rect.Height.Should().Be(h);
        }

        var placed = layouter.PlacedRectangles.ToArray();
        for (var i = 0; i < placed.Length; i++)
        for (var j = i + 1; j < placed.Length; j++)
            placed[i].IntersectsWith(placed[j]).Should().BeFalse();
    }

    [Test]
    public void PutNextRectangle_UniquePositions_WithVariedSizes()
    {
        var layouter = new CircularCloudLayouter(new Point(0, 0));
        var sizes = new[]
        {
            new Size(10,10),
            new Size(20,5),
            new Size(5,20),
            new Size(30,15),
            new Size(15,30)
        };

        for (var i = 0; i < 200; i++)
        {
            var s = sizes[i % sizes.Length];
            generatedRectSizes.Add(s);
            layouter.PutNextRectangle(s);
        }

        var positions = layouter.PlacedRectangles.Select(r => (r.X, r.Y)).ToList();
        positions.Should().OnlyHaveUniqueItems();
    }

    [Test]
    public void PutNextRectangle_CenterWithNegativeCoordinates_WorksCorrectly()
    {
        var center = new Point(-1000, -500);
        var layouter = new CircularCloudLayouter(center);

        var first = layouter.PutNextRectangle(new Size(50, 50));
        generatedRectSizes.Add(new Size(50, 50));
        var centerPoint = new Point(first.X + first.Width / 2, first.Y + first.Height / 2);
        centerPoint.Should().Be(center);

        for (var i = 0; i < 50; i++)
        {
            var s = new Size(10 + i % 5, 8 + i % 7);
            generatedRectSizes.Add(s);
            var r = layouter.PutNextRectangle(s);
            r.IntersectsWith(first).Should().BeFalse();
        }
    }

    [Test]
    public void PutNextRectangle_LargeRectangleThenManySmall_ShouldNotOverlapFirst()
    {
        var center = new Point(500, 500);
        var layouter = new CircularCloudLayouter(center);

        var big = new Size(400, 300);
        generatedRectSizes.Add(big);
        var rBig = layouter.PutNextRectangle(big);

        for (var i = 0; i < 60; i++)
        {
            var s = new Size(10 + i % 10, 6 + i % 7);
            generatedRectSizes.Add(s);
            var r = layouter.PutNextRectangle(s);
            r.IntersectsWith(rBig).Should().BeFalse();
        }
    }

    [Test]
    public void PutNextRectangle_PlacedRectanglesCount_ShouldMatchNumberOfCalls()
    {
        var layouter = new CircularCloudLayouter(new Point(0, 0));
        var sizes = new[] { new Size(10, 10), new Size(20, 20), new Size(5, 7) };

        var total = 37;
        for (var i = 0; i < total; i++)
        {
            var s = sizes[i % sizes.Length];
            generatedRectSizes.Add(s);
            layouter.PutNextRectangle(s);
        }

        layouter.PlacedRectangles.Count.Should().Be(total);
    }

    [Test]
    public void PutNextRectangle_RectanglesStayWithinReasonableBounds()
    {
        var layouter = new CircularCloudLayouter(new Point(0, 0));
        var rnd = new Random(1);

        for (var i = 0; i < 200; i++)
        {
            var s = new Size(rnd.Next(1, 100), rnd.Next(1, 100));
            generatedRectSizes.Add(s);
            var r = layouter.PutNextRectangle(s);

            Math.Abs((long)r.X).Should().BeLessThan(1_000_000_000L);
            Math.Abs((long)r.Y).Should().BeLessThan(1_000_000_000L);
        }
    }

    [Test]
    public void PutNextRectangle_RectangleShouldBeShiftedCloserToCenter_OnPlacement()
    {
        var center = new Point(1000, 1000);
        var layouter = new CircularCloudLayouter(center);

        var large = new Size(500, 200);
        var small = new Size(30, 30);

        generatedRectSizes.Add(large);
        var rLarge = layouter.PutNextRectangle(large);

        generatedRectSizes.Add(small);
        var rSmall = layouter.PutNextRectangle(small);

        var smallCenterDistance = DistanceBetweenPoints(new Point(rSmall.X + rSmall.Width/2, rSmall.Y + rSmall.Height/2), center);
        var largeCenterDistance = DistanceBetweenPoints(new Point(rLarge.X + rLarge.Width/2, rLarge.Y + rLarge.Height/2), center);

        smallCenterDistance.Should().BeLessThanOrEqualTo(largeCenterDistance + Math.Max(large.Width, large.Height));
        var smallCenterPoint = new Point(rSmall.X + rSmall.Width / 2, rSmall.Y + rSmall.Height / 2);
        smallCenterPoint.Should().NotBe(center);
    }

    private static double DistanceBetweenPoints(Point a, Point b)
    {
        var dx = a.X - b.X;
        var dy = a.Y - b.Y;
        return Math.Sqrt(dx * (long)dx + dy * (long)dy);
    }
}