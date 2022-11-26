using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization;

[TestFixture]
public class CircularCloudLayouterTest
{
    private CircularCloudLayouter ccl;
    private List<Rectangle> rectangles;
    private Point center;

    [SetUp]
    public void SetUpTests()
    {
        ccl = new CircularCloudLayouter();
        rectangles = new List<Rectangle>();
        center = new(500, 500);
    }

    [TestCase(0, 0)]
    [TestCase(1, -1)]
    [TestCase(-1, 1)]
    public void UncorrectInitialTest_Initial_Exception(int x, int y)
    {
        Assert.Throws<ArgumentException>(
            () => ccl.GetNextRectangle(new Point(x, y), rectangles, new Size(100, 100)));
    }

    [Test]
    public void CorrectSecondRectangle_PutNextRectangle()
    {
        var rect = ccl.GetNextRectangle(center, rectangles, new Size(100, 100));
        rect.X.Should().Be(450);
        rect.Y.Should().Be(450);
    }

    [Test]
    public void PrintOccupiedSquare()
    {
        for (int i = 1; i <= 10; i++)
            rectangles.Add(ccl.GetNextRectangle(center, rectangles, new Size(15 * i, 10 * i)));
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
        for (int i = 1; i <= 10; i++)
            rectangles.Add(ccl.GetNextRectangle(this.center, rectangles, new Size(15 * i, 10 * i)));
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
    public void Add20RandomRectangleAndSaveAsPic_SaveAsPic() // Тест ничего не проверяет
    {
        Random rand = new Random();
        for (int i = 0; i < 20; i++)
            rectangles.Add(ccl.GetNextRectangle(center, rectangles,
                new Size(rand.Next(10, 100), rand.Next(10, 100))));
        PictureSaver.SaveRectanglesAsPicture(rectangles, "D:");
    }

    [Test]
    public void AssertRectangleSizesDoNotChange()
    {
        List<Size> rectSizes = new();
        for (int i = 1; i <= 10; i++)
            rectSizes.Add(new Size(i * 10, i * 10));
        var rectangles = ccl.GenerateCloud(new(500, 500), rectSizes);
        for(int i = 0; i < 10; i++)
        {
            rectSizes[i].Width.Should().Be(rectangles[i].Width);
            rectSizes[i].Height.Should().Be(rectangles[i].Height);
        }
    }
    [Test]
    public void AssertRectangleListSizeDoNotChange()
    {
        List<Size> rectSizes = new();
        for (int i = 10; i < 100; i += 10)
            rectSizes.Add(new Size(i, i));
        var rectangles = ccl.GenerateCloud(new(500, 500), rectSizes);
        rectangles.Count.Should().Be(rectSizes.Count);
    }
    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
        {
            PictureSaver.SaveRectanglesAsPicture(rectangles, "D:", "testFallen.jpg");
            Console.WriteLine("Упавший тест сохранен: D:testFallen.jpg");
        }
    }
}