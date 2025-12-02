using FluentAssertions;
using NUnit.Framework.Interfaces;
using SixLabors.ImageSharp;
using TagCloud;
using TagCloud.CoordinatesProviders;
using TagCloud.Visualizers;

namespace TagCloudTests;

public class Tests
{
    private CircularCloudLayouter layouter;

    [SetUp]
    public void SetUp()
    {
        var center = new Point(0, 0);
        var provider = new ArchimedesSpiral(center, 1, 1);
        layouter = new CircularCloudLayouter(center, provider);
    }
    
    [TearDown]
    public void TearDown()
    {
        var testStatus = TestContext.CurrentContext.Result.Outcome.Status;
        
        if (testStatus != TestStatus.Failed)
            return;
        
        var testName = TestContext.CurrentContext.Test.Name;
        var savePath = $"{AppDomain.CurrentDomain.BaseDirectory}/{testName}failed.png";

        var visualizer = new TagCloudVisualizer(Color.Black, Color.Red);
        visualizer.DrawRectangles(layouter.Rectangles, new Size(1920, 1080), savePath);
        
        TestContext.Out.WriteLine($"Saved visualization to {savePath})");
    }

    [TestCaseSource(nameof(GetInvalidSizes))]
    public void PutNextRectangle_ShouldThrow_WhenInvalidRectangleSizePresent(Size rectangleSize)
    {
        var action = () => layouter.PutNextRectangle(rectangleSize);
        
        action.Should().Throw<ArgumentException>();
    }
    
    [TestCaseSource(nameof(GetValidSizes))]
    public void PutNextRectangle_RectanglesShouldHaveCorrectSizes_WhenValidSizesPresent(Size rectangleSize)
    {
        var rectangle = layouter.PutNextRectangle(rectangleSize);
        
        rectangle.Width.Should().Be(rectangleSize.Width);
        rectangle.Height.Should().Be(rectangleSize.Height);
    }
    
    [Test]
    public void PutNextRectangle_ShouldNotIntersectWithFirstRectangle_WhenTwoRectanglesAreAlreadyPutted()
    {
        var rectangle1 = layouter.PutNextRectangle(new Size(10, 10));
        var rectangle2 = layouter.PutNextRectangle(new Size(10, 10));
        
        rectangle1.IntersectsWith(rectangle2).Should().BeFalse();
    }

    [Test]
    public void PutNextRectangle_RectanglesShouldNotIntersect_WhenMultipleRectanglesGenerated()
    {
        var rectangles = new List<Rectangle>();
        var random = new Random();

        for (var i = 0; i < 100; i++)
            rectangles.Add(layouter.PutNextRectangle(new Size(random.Next(10, 100), random.Next(10, 100))));
        
        foreach (var firstRectangle in rectangles)
            foreach (var secondRectangle in rectangles.Where(r => firstRectangle != r))
                firstRectangle.IntersectsWith(secondRectangle).Should().BeFalse();
    }

    private static IEnumerable<TestCaseData> GetInvalidSizes()
    {
        yield return new TestCaseData(new Size(-100, 100))
            .SetName("PutNextRectangle_ShouldThrow_WhenWidthIsNegative");
        yield return new TestCaseData(new Size(100, -100))
            .SetName("PutNextRectangle_ShouldThrow_WhenHeightIsNegative");
        yield return new TestCaseData(new Size(-100, -100))
            .SetName("PutNextRectangle_ShouldThrow_WhenBothHeightAndWidthAreNegative");
        yield return new TestCaseData(new Size(0, 100))
            .SetName("PutNextRectangle_ShouldThrow_WhenWidthIsZero");
        yield return new TestCaseData(new Size(100, 0))
            .SetName("PutNextRectangle_ShouldThrow_WhenHeightIsZero");
        yield return new TestCaseData(new Size(-100, 0))
            .SetName("PutNextRectangle_ShouldThrow_WhenWidthIsNegativeAndHeightIsZero");
        yield return new TestCaseData(new Size(0, -100))
            .SetName("PutNextRectangle_ShouldThrow_WhenHeightIsNegativeAndWidthIsZero");
        yield return new TestCaseData(new Size(0, 0))
            .SetName("PutNextRectangle_ShouldThrow_WhenBothWidthAndHeightAreZero");
    }

    private static IEnumerable<TestCaseData> GetValidSizes()
    {
        yield return new TestCaseData(new Size(100, 100));
        yield return new TestCaseData(new Size(1, 1));
        yield return new TestCaseData(new Size(1, 2));
        yield return new TestCaseData(new Size(2, 1));
    }
}