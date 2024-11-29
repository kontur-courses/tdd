using FluentAssertions;
using NUnit.Framework.Interfaces;
using System.Drawing;
using TagsCloud;
using TagsCloudVisualization;

namespace TagsCloud_Tests;

[TestFixture]
public class CircularCloudLayouterTests
{
    private const string FailedTestVisualizationsSavePath = @".\failed_tests";

    private ICircularCloudLayouter _layouter;
    private RectanglesVisualizer _visualization;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        if (!Directory.Exists(FailedTestVisualizationsSavePath))
        {
            Directory.CreateDirectory(FailedTestVisualizationsSavePath);
        }
    }

    [SetUp]
    public void Setup()
    {
        _layouter = new SpiralCircularCloudLayouter(Point.Empty);
        _visualization = new RectanglesVisualizer();
    }

    [TestCase(0, 1)]
    [TestCase(-1, 1)]
    [TestCase(1, 0)]
    [TestCase(1, -1)]
    [Description("Бросает исключение, если длина или ширина меньше или равны 0")]
    public void PutNextRectangle_ThrowsException_WhenSizeIsNotPositive(int width, int height)
    {
        var size = new Size(width, height);

        var putRectangle = () => PutNextRectangle(size);

        putRectangle.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestCase(1, 1, 0, 0)]
    [TestCase(4, 6, -2, -3)]
    [TestCase(5, 7, -2, -3)]
    [Description("Центр первого прямоугольника должен совпадать с центром облака")]
    public void PutNextRectangle_SetsFirstRectangleInCenter(
        int width,
        int height,
        int expectedX,
        int expectedY)
    {
        var size = new Size(width, height);
        var expectedLocation = new Point(expectedX, expectedY);

        var rectangle = PutNextRectangle(size);

        rectangle.Location.Should().Be(expectedLocation);
        rectangle.Size.Should().Be(size);
    }

    [Test]
    [Description("Прямоугольники не имеют пересечений")]
    public void PutNextRectangle_ReturnsNonIntersectingWithEachOtherRectangles()
    {
        var count = 1_000;
        var random = new Random();
        var rectangles = new Rectangle[count];

        for (var i = 0; i < count; i++)
        {
            var size = new Size(random.Next(2, 100), random.Next(2, 100));
            var rectangle = PutNextRectangle(size);
            rectangles[i] = rectangle;
        }

        var haveIntersection = rectangles
            .Where((rectangle, i) => rectangles.Take(i).Any(rectangle.IntersectsWith))
            .Any();

        haveIntersection.Should().BeFalse();
    }

    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome == ResultState.Failure)
        {
            var testName = TestContext.CurrentContext.Test.Name;
            var fileName = $"{testName}.png";
            var path = Path.Combine(FailedTestVisualizationsSavePath, fileName);

            try
            {
                _visualization.GetImage().Save(path);
                TestContext.Out.WriteLine($"Tag cloud visualization saved to file '{fileName}'.");
            }
            catch (Exception ex)
            {
                TestContext.Out.WriteLine($"Unable to save tag cloud visualization for failed test '{testName}'.");
                TestContext.Out.WriteLine(ex.Message);
            }
        }
    }

    private Rectangle PutNextRectangle(Size rectangleSize)
    {
        var rectangle = _layouter.PutNextRectangle(rectangleSize);
        _visualization.AddRectangle(rectangle);
        return rectangle;
    }
}
