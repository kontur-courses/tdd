using System.Drawing;
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

    [Timeout(1000)]
    [TestCase(true)]
    [TestCase(false)]
    public void PutNextRectangle_HasSufficientPerformance_OnLargeAmountOfRectangles(bool randomRectangleSize)
    {
        var rectSize = new Size(20, 10);
        
        for (var i = 1; i <= 200; i++)
        {
            if (randomRectangleSize)
                rectSize = new Size(random.Next(201), random.Next(201));
            layouter.PutNextRectangle(rectSize);
        }
    }
}