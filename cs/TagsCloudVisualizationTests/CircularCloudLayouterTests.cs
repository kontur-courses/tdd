using System.Drawing;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests;

public class CircularCloudLayouterTests
{
    private CircularCloudLayouter layouter = null!;
    private Random random = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        random = new Random();
    }

    [SetUp]
    public void SetUp()
    {
        layouter = new CircularCloudLayouter(new Point(500, 500));
    }

    [TestCase(-200, -300)]
    [TestCase(-200, 300)]
    [TestCase(200, -300)]
    public void PutNextRectangle_ThrowsArgumentException_WhenRectanlgeSizeContainsNegativeParameters(int width,
        int height)
    {
        Assert.Throws<ArgumentException>(() => layouter.PutNextRectangle(new Size(width, height)));
    }

    [TestCase(true)]
    [TestCase(false)]
    public void PutNextRecangle_PlacesRectangleWithoutIntersection_OnMultipleRectangles(bool randomRectangleSize)
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

    [TestCase(10, true)]
    [TestCase(10, false)]
    [TestCase(50, true)]
    [TestCase(50, false)]
    [TestCase(100, true)]
    [TestCase(100, false)]
    public void PutNextRectangle_HasCircularShape_OnDifferentInputData(int numberOfRectangles, bool randomRectangleSize)
    {
        PlaceRectangles(numberOfRectangles, randomRectangleSize);

        var borders = layouter.GetCloudBorders();

        var heightToWidthRatio = (double) Math.Min(borders.Width, borders.Height) / Math.Max(borders.Width, borders.Height);
        
        Assert.That(heightToWidthRatio, Is.GreaterThan(0.8).Within(0.05));
    }
    
    
    [TestCase(10, true)]
    [TestCase(10, false)]
    [TestCase(50, true)]
    [TestCase(50, false)]
    [TestCase(100, true)]
    [TestCase(100, false)]
    public void PutNextRectangle_IsDenseEnough_OnDifferentInputData(int numberOfRectangles, bool randomRectangleSize)
    {
        PlaceRectangles(numberOfRectangles, randomRectangleSize);
        
        var borders = layouter.GetCloudBorders();

        var radius = Math.Max(borders.Width, borders.Height) / 2;
        var circleSquare = Math.PI * radius * radius;
        var rectanglesAccumulatedSquare = layouter.PlacedRectangles.Sum(r => r.Width * r.Height);

        var rectanglesToCircleSquareRatio = rectanglesAccumulatedSquare / circleSquare;
        
        Assert.That(rectanglesToCircleSquareRatio, Is.GreaterThan(0.7).Within(0.05));
    }

    [Timeout(5000)]
    [TestCase(true)]
    [TestCase(false)]
    public void PutNextRectangle_HasSufficientPerformance_OnLargeAmountOfRectangles(bool randomRectangleSize)
    {
        PlaceRectangles(200, randomRectangleSize);
    }

    [TearDown]
    public void GenerateImage_OnTestFail()
    {
        if (TestContext.CurrentContext.Result.Outcome == ResultState.Success)
            return;

        var drawer = new TagsCloudDrawer(layouter);
        var bitmap = drawer.DrawRectangles(new Pen(Color.Red, 1), 5);
        TagsCloudDrawer.SaveImage(bitmap, @"..\..\..\FailedTests",$"{TestContext.CurrentContext.Test.Name}.jpeg");
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