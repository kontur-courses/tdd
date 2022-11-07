using FluentAssertions;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouterTests
    {
        [Test]
        public void PutNextRectangle_Should_ReturnZeroSizeRectangleInZeroPoint_WhenSizeIsZeroAndCenterIsZero()
        {
            var center = Point.Empty;
            var size = Size.Empty;
            var layouter = CircularCloudLayouterFactory.Get(center);
            var expectedRectangle = new Rectangle(center, size);

            var actualRectangle = layouter.PutNextRectangle(size);

            actualRectangle.Should().Be(expectedRectangle);
        }

        [Test]
        public void PutNextRectanlge_Should_ReturnNonZeroRectangleInZeroPoint_WhenSizeIsNotZeroAndCenterIsZero()
        {
            var center = Point.Empty;
            var size = new Size(100, 100);
            var layouter = CircularCloudLayouterFactory.Get(center);
            var expectedRectangle = new Rectangle(center, size);

            var actualRectangle = layouter.PutNextRectangle(size);

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