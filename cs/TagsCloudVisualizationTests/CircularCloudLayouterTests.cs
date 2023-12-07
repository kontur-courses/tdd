using System.Drawing;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class CircularCloudLayouterTests
{
    private ICloudLayouter layouter = null!;
    private Random random = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        random = new Random();
    }

    [SetUp]
    public void SetUp()
    {
        layouter = new CircularCloudLayouter(new Point(500, 500), new CircularCloudBuilder(1, 0.1d));
    }

    [TestCase(-200, -300)]
    [TestCase(-200, 300)]
    [TestCase(200, -300)]
    public void PutNextRectangle_ThrowsArgumentException_WhenRectanlgeSizeContainsNegativeParameters(int width,
        int height)
    {
        Assert.Throws<ArgumentException>(() => layouter.PutNextRectangle(new Size(width, height)));
    }

    [Test]
    public void GetCloudBorders_ThrowsInvalidOperationException_WhenListOfRectanglesIsEmpty()
    {
        Assert.Throws<InvalidOperationException>(() => new List<Rectangle>().GetBorders());
    }

    [Test]
    public void PutNextRecangle_PlacesRectangleWithoutIntersection_OnMultipleRectangles(
        [Values] bool randomRectangleSize)
    {
        var rectangles = new List<Rectangle>();
        var rectSize = new Size(20, 10);

        for (var i = 1; i <= 50; i++)
        {
            if (randomRectangleSize)
                rectSize = new Size(random.Next(201), random.Next(201));

            var newRectangle = layouter.PutNextRectangle(rectSize);

            foreach (var rectangle in rectangles)
                Assert.That(rectangle.IntersectsWith(newRectangle), Is.False);

            rectangles.Add(newRectangle);
        }
    }

    [Test]
    public void PutNextRectangle_HasCircularShape_OnDifferentInputData(
        [Values(50, 100)] int numberOfRectangles,
        [Values] bool randomRectangleSize)
    {
        PlaceRectangles(numberOfRectangles, randomRectangleSize);

        var borders = layouter.PlacedRectangles.GetBorders();

        var heightToWidthRatio =
            (double) Math.Min(borders.Width, borders.Height) / Math.Max(borders.Width, borders.Height);

        Assert.That(heightToWidthRatio, Is.GreaterThan(0.8).Within(0.05));
    }

    [Test]
    public void PutNextRectangle_IsDenseEnough_OnDifferentInputData(
        [Values(50, 100)] int numberOfRectangles,
        [Values] bool randomRectangleSize)
    {
        PlaceRectangles(numberOfRectangles, randomRectangleSize);

        var borders = layouter.PlacedRectangles.GetBorders();

        var radius = Math.Max(borders.Width, borders.Height) / 2;
        var circleSquare = Math.PI * radius * radius;
        var rectanglesAccumulatedSquare = layouter.PlacedRectangles.Sum(r => r.Width * r.Height);

        var rectanglesToCircleSquareRatio = rectanglesAccumulatedSquare / circleSquare;

        Assert.That(rectanglesToCircleSquareRatio, Is.GreaterThan(0.7).Within(0.05));
    }

    [Timeout(5000)]
    [Test]
    public void PutNextRectangle_HasSufficientPerformance_OnLargeAmountOfRectangles(
        [Values] bool randomRectangleSize)
    {
        PlaceRectangles(200, randomRectangleSize);
    }

    [TearDown]
    public void GenerateImage_OnTestFail()
    {
        if (TestContext.CurrentContext.Result.Outcome == ResultState.Success)
            return;

        var drawer = new TagsCloudDrawer(layouter, new Pen(Color.Red, 1), 5);
        var bitmap = drawer.DrawTagCloud();
        TagsCloudDrawer.SaveImage(bitmap, @"..\..\..\FailedTests", $"{TestContext.CurrentContext.Test.Name}.jpeg");
    }

    private void PlaceRectangles(int numberOfRectangles, bool randomRectangleSize)
    {
        var rectSize = new Size(20, 10);

        for (var i = 1; i <= numberOfRectangles; i++)
        {
            if (randomRectangleSize)
                rectSize = new Size(random.Next(201), random.Next(201));
            layouter.PutNextRectangle(rectSize);
        }
    }
}