using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace CloudTag
{
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter layouter;
        private readonly Point layouterCenter = new Point(0, 0);

        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(layouterCenter);
        }

        [Test]
        public void Constructor_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => new CircularCloudLayouter(new Point(0, 0)));
        }

        [Test]
        public void PutNextRectangle_ReturnsRectangle_FirstAdding()
        {
            var size = new Size(10, 10);
            var actualRectangle = layouter.PutNextRectangle(size);
            var expectedRectangle = new Rectangle(Point.Empty, size);
            expectedRectangle.SetCenter(layouterCenter);
            actualRectangle.Should().Be(expectedRectangle);
        }

        [TestCase(2, TestName = "2 rect")]
        [TestCase(10, TestName = "10 rect")]
        [TestCase(80, TestName = "80 rect")]
        [TestCase(200, TestName = "200 rect")]
        public void PutNextRectangle_ShouldNotIntersect_RectSameSize(int rectCount)
        {
            var size = new Size(10, 10);
            for (var i = 0; i < rectCount; i++)
                layouter.PutNextRectangle(size);

            var rectsInLayout = layouter.GetRectangles().ToArray();
            for (var i = 0; i < rectCount; i++)
            {
                rectsInLayout[i].Should().Match<Rectangle>(
                    rect1 => !rectsInLayout
                        .Skip(i + 1)
                        .Any(rect1.IntersectsWith)
                );
            }
        }

        [Test]
        public void PutNextRectangle_ReturnEmptyRect_SizeIsZero()
        {
            var actualRect = layouter.PutNextRectangle(new Size(0, 0));
            actualRect.Should().Be(Rectangle.Empty);
        }

        [TestCase(1, TestName = "1 rect")]
        [TestCase(20, TestName = "20 rect")]
        public void GetRectangles_ReturnEmptyCollection_AddedRectsWithZeroSize(int rectCount)
        {
            var size = new Size(0,0);
            for (var i = 0; i < rectCount; i++)
                layouter.PutNextRectangle(size);

            layouter.GetRectangles().Should().BeEmpty();
        }

        [Test]
        public void PutNextRectangle_ReturnsEmptyRect_OneOfSidesIs0()
        {
            var actualRect1 = layouter.PutNextRectangle(new Size(0, 10));
            var actualRect2 = layouter.PutNextRectangle(new Size(10, 0));

            actualRect1.Should().Be(Rectangle.Empty);
            actualRect2.Should().Be(Rectangle.Empty);
        }
        
        [Timeout(1000)]
        [Test]
        public void PutNextRectangle_PerformanceTest_5000RectSameSize()
        {
            var size = new Size(10, 10);
            for (var i = 0; i < 5000; i++)
                layouter.PutNextRectangle(size);
        }
        
        
    }
}