namespace TagsCloudVisualization;

public class CircularCloudLayouterTest
{
    private CircularCloudLayouter ccl;
    private List<Rectangle> rectangles;

    [SetUp]
    public void SetUpTests()
    {
        ccl = new CircularCloudLayouter();
        rectangles = new List<Rectangle>();
        rectangles.Add(new Rectangle(400,450,200,100));
    }
    [TestCase(0, 0)]
    [TestCase(1, -1)]
    [TestCase(-1, 1)]
    public void UncorrectInitialTest_Initial_Exception(int x, int y)
    {
        Rectangle rect = new Rectangle(x, y, 100, 100);
        rectangles.Clear();
        rectangles.Add(rect);
        Assert.Throws<ArgumentException>(
            () => ccl.PutNextRectangle(rectangles, new Size(100,100)));
    }
    [Test]
    public void CorrectSecondRectangle_PutNextRectangle()
    {
        var rect = ccl.PutNextRectangle(rectangles, new Size(100, 100));
        rect.X.Should().Be(450);
        rect.Y.Should().Be(450);
    }

    [Test]
    public void PrintOccupiedSquare()
    {
        for (int i = 0; i < 10; i++)
            rectangles.Add(ccl.PutNextRectangle(rectangles, new Size(15 * i, 10 * i)));
        int radius = Math.Min(rectangles[0].GetCenter().X, rectangles[0].GetCenter().Y);
        double circleSquare = radius * radius * Math.PI;
        double rectanglesSquare = 0;
        foreach (var rect in rectangles)
            rectanglesSquare += rect.GetSquare();
        Console.Write("Occupied area: {0}%", (rectanglesSquare / circleSquare) * 100);
    }
    [Test]
    public void CheckIntersectWithCircle()
    {
        for (int i = 0; i < 10; i++)
            rectangles.Add(ccl.PutNextRectangle(rectangles, new Size(15 * i, 10 * i)));
        var center = rectangles[0].GetCenter();
        int radius = Math.Min(center.X, center.Y);
        foreach (var rect in rectangles)
        {
            var horizontalCoordinate = new int[] { rect.X, rect.Right };
            var verticalCoordinate = new int[] { rect.Y, rect.Bottom };
            for (var i = 0; i < 4; i++)
            {
                var x = horizontalCoordinate[i % 2] - center.X;
                var y = verticalCoordinate[i / 2] - center.Y;
                (Math.Sqrt(x * x + y * y) >= radius).Should().BeFalse();
            }
        }
    }
    [Test]
    public void Add40RandomRectangleAndSaveAsPic_SaveAsPic() // Тест ничего не проверяет
    {
        Random rand = new Random();
        for (int i = 0; i < 20; i++)
            rectangles.Add(ccl.PutNextRectangle(rectangles,
                new Size(rand.Next(10, 100), rand.Next(10, 100))));
        PictureSaver.SaveRectanglesAsPicture(rectangles, "D:");
    }
}