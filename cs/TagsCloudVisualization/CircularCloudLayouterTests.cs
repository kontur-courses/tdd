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
        public void PutNextRectangle_Should_AddSecondRectangleOnTheFirstTop_WhenWidthOfFirstIsGreaterThanTheHeight()
        {
            var firstRectangleSize = new Size(200, 100);
            var secondRectangleSize = new Size(100, 100);
            var layouter = CircularCloudLayouterFactory.Get(Point.Empty);
            var expectedRectangle = new Rectangle(new Point(-50, -150), secondRectangleSize);

            layouter.PutNextRectangle(firstRectangleSize);
            var actualRectangle = layouter.PutNextRectangle(secondRectangleSize);

            actualRectangle.Should().Be(expectedRectangle);
        }

        [Test]
        public void PutNextRectangle_Should_AddSecondRectangleOnTheFirstLeft_WhenHeightOfFirstIsGreaterThanTheWidth()
        {
            var firstRectangleSize = new Size(100, 200);
            var secondRectangleSize = new Size(100, 100);
            var layouter = CircularCloudLayouterFactory.Get(Point.Empty);
            var expectedRectangle = new Rectangle(new Point(-150, -50), secondRectangleSize);
            
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
            var expectedRectangle = new Rectangle(new Point(-50, 50), rectangleSize);

            var _ = layouter.PutNextRectangle(firstRectangleSize);
            var second = layouter.PutNextRectangle(rectangleSize);
            var actualRectangle = layouter.PutNextRectangle(rectangleSize);

            actualRectangle.Should().Be(expectedRectangle);
        }

        [Test]
        public void PutNextRectangle_Should_AddRectangleInHole_WhenThereIsASuitableHole()
        {
            var brickSize = new Size(200, 200);
            var smallRectangleSize = new Size(40, 10);
            var layouter = CircularCloudLayouterFactory.Get(Point.Empty);

            var expectedRectangle1 = new Rectangle(new Point(30, 100), smallRectangleSize);
            var expectedRectangle2 = new Rectangle(new Point(-60, 100), smallRectangleSize);

            // Making two holes on the right
            var r1 = layouter.PutNextRectangle(brickSize);
            var r2 = layouter.PutNextRectangle(brickSize);
            var r3 = layouter.PutNextRectangle(brickSize);
            var r4 = layouter.PutNextRectangle(brickSize);
            var r5 = layouter.PutNextRectangle(new Size(60, 20));
            var r6 = layouter.PutNextRectangle(new Size(180, 100));
            var r7 = layouter.PutNextRectangle(brickSize);
            var r8 = layouter.PutNextRectangle(brickSize);

            // Putting rectangle in upper or lower hole
            var actual = layouter.PutNextRectangle(smallRectangleSize);

            actual.Should().Match<Rectangle>(r => r == expectedRectangle1 || r == expectedRectangle2);
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