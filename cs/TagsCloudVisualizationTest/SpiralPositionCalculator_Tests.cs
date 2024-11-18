using NUnit.Framework;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class CircularPositionCalculatorTests
    {
        private CircularPositionCalculator calculator;
        private Point center;

        [SetUp]
        public void SetUp()
        {
            center = new Point(0, 0);
            calculator = new CircularPositionCalculator(center);
        }

        [Test]
        public void CalculateNextPosition_ShouldReturnRectangleAtCenter_WhenNoRectanglesExist()
        {
            var size = new Size(10, 10);
            var rectangles = new List<Rectangle>();

            var result = calculator.CalculateNextPosition(size).First();

            result.Location.Should().Be(center);
        }

        [Test]
        public void CalculateNextPosition_ShouldIncreaseOffset_WhenAngleCompletesFullCircle()
        {
            var size = new Size(10, 10);
            var source =calculator.CalculateNextPosition(size)
                .Skip(100)
                .First();
            GetDistance(source.Location, center).Should().NotBe(0);
        }

        [Test]
        public void ValidateRectanglePosition_ShouldReturnTrue_WhenNoIntersections()
        {
            var size = new Size(10, 10);
            var rectangles = new List<Rectangle>();
            var rectangle = new Rectangle(new Point(0, 0), size);

            var result = calculator.IsRectanglePositionValid(rectangles, rectangle);

            result.Should().BeTrue();
        }

        [Test]
        public void ValidateRectanglePosition_ShouldReturnFalse_WhenIntersectsWithExistingRectangle()
        {
            var size = new Size(10, 10);
            var rectangles = new List<Rectangle>
            {
                new Rectangle(new Point(0, 0), size)
            };
            var rectangle = new Rectangle(new Point(0, 0), size);

            var result = calculator.IsRectanglePositionValid(rectangles, rectangle);

            result.Should().BeFalse();
        }

        private static double GetDistance(Point point, Point other)
        {
            var dx = point.X - other.X;
            var dy = point.Y - other.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}