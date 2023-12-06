namespace TagsCloudVisualizationTests;

public static class CircularCloudLayouterTests
{
    [SetUp]
    public static void SetUp()
    {
        _layouter = new CircularCloudLayouter(new Point(0, 0));
    }
    
    private static CircularCloudLayouter _layouter;
    
    [TestCase(0, 0, TestName = "CircularCloudLayouter_PutNextRectangle_ThrowsArgumentExceptionOnZeroDimensions")]
    [TestCase(-1, -1, TestName = "CircularCloudLayouter_PutNextRectangle_ThrowsArgumentExceptionOnNegativeDimensions")]
    public static void CircularCloudLayouter_PutNextRectangle_ThrowsArgumentExceptionOn(int width, int height)
    {
        new Action(() => _layouter.PutNextRectangle(new Size(width, height)))
            .Should()
            .ThrowExactly<ArgumentException>()
            .Where(e => e.Message.Equals("Width and height of rectangle must be positive"));
    }

    [Test]
    public static void CircularCloudLayouter_PutNextRectangle_RectanglesShouldNotIntersect()
    {
        var rectangles = AddRectangles().ToArray();
        
        rectangles
            .Where(r1 => rectangles.Where(r2 => r2 != r1).Any(r1.IntersectsWith))
            .Should()
            .BeEmpty();
    }

    [Test, Timeout(400)]
    public static void CircularCloudLayouter_PutNextRectangle_MustBeEfficient()
    {
        AddRectangles().Count();
    }

    private static IEnumerable<Rectangle> AddRectangles(int count = 100)
    {
        var rnd = new Random(228_666);
        
        for (var i = 0; i < count; i++) 
            yield return _layouter.PutNextRectangle(GetNextRandomSize(rnd));
    }

    private static Size GetNextRandomSize(Random rnd)
    {
        return new Size(rnd.Next(1, 10), rnd.Next(1, 10));
    }
}