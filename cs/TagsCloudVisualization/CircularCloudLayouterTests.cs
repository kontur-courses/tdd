using FluentAssertions;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouterTests
    {
        [Test]
        public void PutNextRectangle_Should_ThrowArgumentException_WhenSizeIsZero()
        {
            var center = Point.Empty;
            var size = Size.Empty;
            var layouter = CircularCloudLayouterFactory.Get(center);
            
            var action = () => layouter.PutNextRectangle(size);

            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void PutNextRectanlge_Should_ReturnNonZeroRectangleInZeroPoint_WhenSizeIsNotZeroAndCenterIsZero()
        {
            var size = new Size(100, 100);
            var layouter = CircularCloudLayouterFactory.Get(Point.Empty);
            var expectedRectangle = new Rectangle(new Point(-50, -50), size);

            var actualRectangle = layouter.PutNextRectangle(size);

            actualRectangle.Should().Be(expectedRectangle);
        }

        [Test]
        public void PutNextRectangle_Should_AddSecondRectangleOnTheFirstTop()
        {
            var firstRectangleSize = new Size(200, 100);
            var secondRectangleSize = new Size(100, 100);
            var layouter = CircularCloudLayouterFactory.Get(Point.Empty);
            var expectedRectangle = new Rectangle(new Point(-secondRectangleSize.Width / 2, -(firstRectangleSize.Height / 2 + secondRectangleSize.Height)), secondRectangleSize);

            layouter.PutNextRectangle(firstRectangleSize);
            var actualRectangle = layouter.PutNextRectangle(secondRectangleSize);

            actualRectangle.Should().Be(expectedRectangle);
        }

        [Test]
        public void PutNextRectangle_Should_AddThirdRectangleOnTheFirstBottom_WhenTheSecondIsOnTheFirstTop()
        {
            var firstRectangleSize = new Size(200, 100);
            var rectangleSize = new Size(100, 100);
            var layouter = CircularCloudLayouterFactory.Get(Point.Empty);
            var expectedRectangle = new Rectangle(new Point(rectangleSize.Width / 2 - firstRectangleSize.Width / 2, firstRectangleSize.Height / 2), rectangleSize);

            var _ = layouter.PutNextRectangle(firstRectangleSize);
            var second = layouter.PutNextRectangle(rectangleSize);
            var actualRectangle = layouter.PutNextRectangle(rectangleSize);

            actualRectangle.Should().Be(expectedRectangle);
        }
    }

    internal static class CircularCloudLayouterFactory
    {
        public static CircularCloudLayouter Get(Point center)
        {
            return new CircularCloudLayouter(center);
        }
    }
}