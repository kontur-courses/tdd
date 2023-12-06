[TestFixture]
public class CircularCloudLayouter_Should
{
    private const int Width = 1920;
    private const int Height = 1080;
    private CircularCloudLayouter sut;


    [SetUp]
    public void Setup()
    {
        sut = new CircularCloudLayouter(new Point(Width / 2, Height / 2));
    }

    [Test]
    public void HaveEmptyRectanglesList_WhenCreated()
    {
        sut.Rectangles.Count.Should().Be(0);
    }

    [TestCase(1, TestName = "One rectangle")]
    [TestCase(10, TestName = "10 rectangles")]
    public void HaveAllRectangles_WhichWerePut(int rectanglesCount)
    {
        for (var i = 0; i < rectanglesCount; i++)
        {
            sut.PutNextRectangle(new Size(2, 3));
        }

        sut.Rectangles.Count.Should().Be(rectanglesCount);
    }

    [TestCase(0, 5, TestName = "Width is zero")]
    [TestCase(5, 0, TestName = "Height is zero")]
    [TestCase(-5, 5, TestName = "Width is negative")]
    [TestCase(5, -5, TestName = "Height is negative")]
    public void ThrowException_OnInvalidSizeArguments(int width, int height)
    {
        Action action = () => { sut.PutNextRectangle(new Size(0, 0)); };

        action.Should().Throw<ArgumentException>();
    }
}