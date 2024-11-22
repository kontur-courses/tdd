using NUnit.Framework.Interfaces;
using TagsCloudVisualization.Layouter;
using TagsCloudVisualization.Renderer;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class CircularCloudLayouterTests
{
    private CircularCloudLayouter layouter;
    private static readonly SKPoint Center = new(500, 500);
    private static readonly float Density = 0.65f;

    [SetUp]
    public void SetUp()
    {
        layouter = new CircularCloudLayouter(Center);
    }

    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed) return;
        SaveImage();
    }

    private void SaveImage()
    {
        var filename = "tests/layouter_" + TestContext.CurrentContext.Test.ID + ".png";

        var renderer = new Renderer(new SKSize(Center.X * 2, Center.Y * 2));
        renderer.DrawRectangles(layouter.GetRectangles());
        var imageData = renderer.GetEncodedImage();
        Directory.CreateDirectory("tests");
        var path = Path.Combine(Directory.GetCurrentDirectory(), filename);
        using var stream = new FileStream(path, FileMode.Create);
        imageData.SaveTo(stream);
        Console.WriteLine($"Tag cloud visualization saved to file {path}");
    }

    private IEnumerable<SKSize> GetRandomSizes(int count)
    {
        var random = new Random();
        for (var i = 0; i < count; i++)
        {
            var size = new SKSize(random.Next(10, 100), random.Next(10, 100));
            yield return size;
        }
    }

    [Test]
    public void PutNextRectangle_ShouldThrowArgumentException_WhenSizeNotPositive()
    {
        Action action = () => layouter.PutNextRectangle(new SKSize(0, 100));
        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void PutNextRectangle_ShouldReturnRectangle()
    {
        var rectangle = layouter.PutNextRectangle(new SKSize(100, 100));

        rectangle.Should().BeEquivalentTo(new SKRect(Center.X - 50, Center.Y - 50, Center.X + 50, Center.Y + 50));
    }

    [Test]
    public void PutNextRectangle_ShouldNotIntersectRectangles()
    {
        var rectangles = new List<SKRect>();
        for (var i = 0; i < 10; i++) rectangles.Add(layouter.PutNextRectangle(new SKSize(10, 10)));

        for (var i = 0; i < rectangles.Count - 1; i++)
        for (var j = i + 1; j < rectangles.Count; j++)
            rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
    }

    [Test]
    [Repeat(3)]
    public void PutNextRectangle_ShouldGenerateDenseLayout()
    {
        var sizes = GetRandomSizes(150);
        var rectangles = sizes.Select(size => layouter.PutNextRectangle(size)).ToArray();
        var totalRectArea = rectangles.Sum(rect => rect.Width * rect.Height);
        var boundingCircleRadius = rectangles.Max(DistanceToCenter);
        var boundingCircleArea = Math.PI * boundingCircleRadius * boundingCircleRadius;
        var density = totalRectArea / boundingCircleArea;
        density.Should().BeGreaterOrEqualTo(Density);
    }

    [Test]
    [Repeat(3)]
    public void PutNextRectangle_ShouldPlaceRectanglesInCircle()
    {
        var sizes = GetRandomSizes(150);
        var rectangles = sizes.Select(size => layouter.PutNextRectangle(size)).ToArray();
        
        
        var presumedAverageSide = rectangles.Average(size => (size.Width + size.Height) / 2);
        var totalAreaOfRectangles = rectangles.Sum(rect => rect.Width * rect.Height);
        var circleRadius = Math.Sqrt(totalAreaOfRectangles / Density / Math.PI);
        var expectedMaxDistanceFromCenter = circleRadius + presumedAverageSide / 2;
        var maxDistanceFromCenter = (double)rectangles.Max(DistanceToCenter);

        maxDistanceFromCenter.Should().BeLessOrEqualTo(expectedMaxDistanceFromCenter);
    }

    private static float DistanceToCenter(SKRect rect)
    {
        var rectCenter = new SKPoint(rect.MidX, rect.MidY);
        return SKPoint.Distance(Center, rectCenter);
    }
}