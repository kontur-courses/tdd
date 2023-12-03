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

        [Test]
        public void CircularCloudLayouter_HasOneRectangle_AfterOneRectanglePut()
        {
            layouter.PutNextRectangle(new Size(2, 3));

            layouter.Rectangles.Count.Should().Be(1);
        }

        [Test]
        public void CircularCloudLayouter_ThrowsException_OnZeroDimension()
        {
            Action action = () => { layouter.PutNextRectangle(new Size(0, 0)); };

            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void CircularCloudLayouter_HasTenRectangles_AfterTenRectanglesPlaced()
        {
            for (var i = 0; i < 10; i++)
            {
                layouter.PutNextRectangle(new Size(2, 3));
            }

            layouter.Rectangles.Count.Should().Be(10);
        }
    }
}