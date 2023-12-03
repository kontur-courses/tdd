namespace TagCloudTests
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private const int Width = 1920;
        private const int Height = 1080;
        private CircularCloudLayouter layouter;


        [SetUp]
        public void Setup()
        {
            layouter = new CircularCloudLayouter(new Point(Width / 2, Height / 2));
        }

        [Test]
        public void CircularCloudLayoter_RectanglesListEmpty_WhenCreated()
        {
            layouter.Rectangles.Count.Should().Be(0);
        }

        [TestCase(1, TestName = "One rectangle")]
        [TestCase(10, TestName = "10 rectangles")]
        public void CircularCloudLayouter_HasAllRectanglesThatWasPlaced(int rectanglesCount)
        {
            for (var i = 0; i < rectanglesCount; i++)
            {
                layouter.PutNextRectangle(new Size(2, 3));
            }

            layouter.Rectangles.Count.Should().Be(rectanglesCount);
        }

        [TestCase(0, 5, TestName = "Width is zero")]
        [TestCase(5, 0, TestName = "Height is zero")]
        [TestCase(-5, 5, TestName = "Width is negative")]
        [TestCase(5, -5, TestName = "Height is negative")]
        public void CircularCloudLayouter_ThrowsExceptionOnInvalidSizeArguments(int width, int height)
        {
            Action action = () => { layouter.PutNextRectangle(new Size(0, 0)); };
            
            action.Should().Throw<ArgumentException>();
        }
    }
}