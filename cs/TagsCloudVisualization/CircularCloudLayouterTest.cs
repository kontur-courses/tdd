namespace TagsCloudVisualization;

public class CircularCloudLayouterTest
{
    private CircularCloudLayouter ccl;

    [SetUp]
    public void SetUpTests()
    {
        ccl = new CircularCloudLayouter(new Point(500,500));
    }
    [TestCase(0, 0)]
    [TestCase(1, -1)]
    [TestCase(-1, 1)]
    public void UncorrectInitialTest_Initial_Exeption(int x, int y)
    {
        Assert.Throws<ArgumentException>(() => new CircularCloudLayouter(new Point(x, y)));
    }
    [Test]
    public void RectangleSizeBiggerThenRadius_PutNextRectangle_Exeption()
    {
        Assert.Throws<ArgumentException>(() => ccl.PutNextRectangle(new Size(1000, 1000)));

    }
    [Test]
    public void CorrectCenterRectangle_PutNextRectangle()
    {
        var rect = ccl.PutNextRectangle(new Size(100, 100));
        rect.X.Should().Be(450);
        rect.Y.Should().Be(450);
    }
    [Test]
    public void TwoCorrectRectangle_PutNextRectangle()
    {
        var rect = ccl.PutNextRectangle(new Size(100, 100));
        rect.X.Should().Be(450);
        rect.Y.Should().Be(450);
        rect = ccl.PutNextRectangle(new Size(100, 100));
        rect.X.Should().Be(550);
        rect.Y.Should().Be(442);
    }
}