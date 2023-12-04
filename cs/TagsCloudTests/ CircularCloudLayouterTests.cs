using System.Drawing;
using System.Drawing.Imaging;
using FluentAssertions;
using NUnit.Framework.Interfaces;
using tagsCloud;

namespace TagsCloudTests;

[TestFixture]
public class CircularCloudLayouterTests
{
    private Point _start;
    private CircularCloudLayouter circularCloudLayouter;

    [SetUp]
    public void Setup()
    {
        _start = new Point(10, 10);
        circularCloudLayouter = new CircularCloudLayouter(_start);
    }

    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome == ResultState.Failure)
        {
            var visualizer = new RectanglesVisualizer(circularCloudLayouter.Rectangles);
            var workingDirectory = Environment.CurrentDirectory;
            var projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            var path = projectDirectory + @"\Images\";
            var imageName = TestContext.CurrentContext.Test.Name;
            var image = visualizer.DrawTagCloud();
            image.Save($"{path}{imageName}.png", ImageFormat.Png);
            Console.WriteLine($"Tag cloud visualization saved to file {path}{imageName}");
        }
    }
    

    private readonly Func<List<Rectangle>, bool> isRectanglesIntersect = rectangles =>
        rectangles.Any(rectangle => rectangles.Any(nextRectangle =>
            nextRectangle.IntersectsWith(rectangle) && !rectangle.Equals(nextRectangle)));


    [Test]
    public void CircularCloudLayouter_GetLocationAfterInitialization_ShouldBeEmpty()
    {
        var location = circularCloudLayouter.GetRectanglesLocation();
        location.Should().BeEmpty();
    }

    [TestCase(-1, 10, TestName = "width is negative")]
    [TestCase(1, -10, TestName = "height is negative")]
    [TestCase(1, 0, TestName = "Zero height, correct width")]
    [TestCase(0, 10, TestName = "Zero width, correct height")]
    public void CircularCloudLayouter_PutRectangleWithNegativeParams_ShouldBeThrowException(int width, int height)
    {
        var size = new Size(width, height);
        Action action = () => circularCloudLayouter?.PutNextRectangle(size);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Sides of the rectangle should not be non-positive");
    }

    [Test]
    public void CircularCloudLayouter_PutOneRectangle_IsNotEmpty()
    {
        var rect = circularCloudLayouter.PutNextRectangle(new Size(10, 10));
        var location = circularCloudLayouter.GetRectanglesLocation();
        location.Should().NotBeEmpty();
    }

    [Test]
    public void CircularCloudLayouter_CreateSecondLayouterAfterAddingRectangle_IsNotEmpty()
    {
        var rect = circularCloudLayouter.PutNextRectangle(new Size(10, 10));
        var circularCloudLayouter2 = new CircularCloudLayouter(new Point(12, 12));
        circularCloudLayouter.Rectangles.Should().NotBeEmpty();
    }
    
    [TestCase(1000, TestName = "check intersection of 1000 rectangles")]
    public void CircularCloudLayouter_PutNextRectangle_RectanglesShouldNotIntersect(int count)
    {
        for (var i = 0; i < count; i++)
        {
            var size = Utils.GetRandomSize();
            var rect = circularCloudLayouter.PutNextRectangle(size);
        }

        isRectanglesIntersect(circularCloudLayouter.Rectangles).Should().BeFalse();
    }
}