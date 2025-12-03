using System.Drawing;
using FluentAssertions;
using NUnit.Framework;


namespace TagsCloudVisualization;

[TestFixture]
public class CircularCloudLayouterTest
{
    private CircularCloudLayouter layouter;
    private Point center;

    [SetUp]
    public void SetUp()
    {
        center = new Point(100, 100);
        layouter = new CircularCloudLayouter(center);
    }

    [Test]
    public void PutNextRectangle_PutsRectangleAroundCenter()
    {
        var size = new Size(20, 20);

        var rect = layouter.PutNextRectangle(size);

        var rectCenter = new Point(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);
        rectCenter.Should().Be(center);
    }

    [Test]
    public void PutNextRectangle_ReturnsDifferentRectangles()
    {
        var size = new Size(20, 20);

        var r1 = layouter.PutNextRectangle(size);
        var r2 = layouter.PutNextRectangle(size);

        r1.Should().NotBe(r2);
    }

    [Test]
    public void PutNextRectangle_ReturnsNotIntersectRectangles()
    {
        var size = new Size(20, 20);
        var placed = new List<Rectangle>();

        for (var i = 0; i < 100; i++)
        {
            var next = layouter.PutNextRectangle(size);
            foreach (var prev in placed)
                next.IntersectsWith(prev).Should().BeFalse();
            placed.Add(next);
        }
    }

    [Test]
    public void PutNextRectangle_CreateDenseRectangles()
    {
        var size = new Size(20, 20);
        var rects = Enumerable.Range(0, 100)
            .Select(_ => layouter.PutNextRectangle(size))
            .ToArray();

        var centers = rects.Select(GetCenter).ToArray();
        var avgX = centers.Average(p => p.X);
        var avgY = centers.Average(p => p.Y);

        Math.Abs(avgX - center.X).Should().BeLessThan(size.Width);
        Math.Abs(avgY - center.Y).Should().BeLessThan(size.Height);
    }

    [Test]
    public void PutNextRectangle_CreateSameRectangles_WhenSameSizes()
    {
        var layouter1 = new CircularCloudLayouter(center);
        var layouter2 = new CircularCloudLayouter(center);
        var sizes = new[]
        {
            new Size(10, 10),
            new Size(20, 10),
            new Size(15, 15),
            new Size(5, 30)
        };

        var rects1 = sizes.Select(s => layouter1.PutNextRectangle(s)).ToArray();
        var rects2 = sizes.Select(s => layouter2.PutNextRectangle(s)).ToArray();

        rects1.Should().BeEquivalentTo(rects2);
    }
    
    private static Point GetCenter(Rectangle rectangle)
    {
        return new Point(rectangle.Left + rectangle.Width / 2, rectangle.Top + rectangle.Height / 2);
    }
}