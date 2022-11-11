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
        rectangles.Add(new Rectangle(450,450,150,100));
    }
    [TestCase(0, 0)]
    [TestCase(1, -1)]
    [TestCase(-1, 1)]
    public void UncorrectInitialTest_Initial_Exception(int x, int y)
    {
        Rectangle rect = new Rectangle(x, y, 100, 100);
        rectangles.Clear();
        rectangles.Add(rect);
        Assert.Throws<ArgumentException>(() => ccl.PutNextRectangle(rectangles, new Size(100,100)));
    }
    [Test]
    public void CorrectSecondRectangle_PutNextRectangle()
    {
        var rect = ccl.PutNextRectangle(rectangles, new Size(100, 100));
        rect.X.Should().Be(450);
        rect.Y.Should().Be(450);
    }
    [Test]
    [Ignore("Для проверки")]
    public void Add40RandomRectangleAndSaveAsPic_SaveAsPic() // Тест ничего не проверяет
    {
        Random rand = new Random();
        for (int i = 0; i < 40; i++)
            ccl.PutNextRectangle(rectangles, new Size(rand.Next(10, 100), rand.Next(10, 100)));
        PictureSaver.SaveRectanglesAsPicture(rectangles, "D:");
    }
}