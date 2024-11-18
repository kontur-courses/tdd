namespace TagsCloudVisualizationTest;

public class CircularCloudLayouter_Tests
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter layouter;
        private IPositionCalculator calculator;
        private Point center;

        [SetUp]
        public void SetUp()
        {
            center = new Point(0, 0);
            calculator = new CircularPositionCalculator(center);
            layouter = new CircularCloudLayouter(center, calculator);
        }

        [Test]
        public void PutNextRectangle_ThrowsArgumentException_WhenWidthIsNonPositive()
        {
            var size = new Size(0, 10);

            Action act = () => layouter.PutNextRectangle(size);

            act.Should().Throw<ArgumentException>().WithMessage("Size width must be positive number");
        }

        [Test]
        public void PutNextRectangle_ThrowsArgumentException_WhenHeightIsNonPositive()
        {
            var size = new Size(10, 0);

            Action act = () => layouter.PutNextRectangle(size);

            act.Should().Throw<ArgumentException>().WithMessage("Size height must be positive number");
        }

        [Test]
        public void PutNextRectangle_AddsRectangleToList_WhenValidSize()
        {
            var size = new Size(10, 10);

            var result = layouter.PutNextRectangle(size);

            layouter.Rectangles.Should().Contain(result);
        }

        [Test]
        public void PutNextRectangle_ReturnsNonIntersectingRectangle_WhenRectanglesExist()
        {
            var size = new Size(10, 10);
            layouter.PutNextRectangle(size);

            var result = layouter.PutNextRectangle(size);

            result.IntersectsWith(layouter.Rectangles[0]).Should().BeFalse();
        }
    }
}