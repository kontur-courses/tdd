using System.Configuration;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace CloudTag
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter layouter;

        [Test]
        public void Constructor_DoesntThrow()
        {
            Assert.DoesNotThrow(() => new CircularCloudLayouter(new Point(0, 0)));
        }

        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(new Point(0, 0));
        }

        [Test]
        public void PutNextRectangle_ReturnsRectangle_FirstAdding()
        {
            var size = new Size(10, 10);
            var rectangle = layouter.PutNextRectangle(size);
            rectangle.Location.Should().Be(new Point(-5, -5));
            rectangle.Size.Should().Be(size);
        }

        [Test]
        public void PutNextRectangle_ShouldntIntersect_TwoRectSameSize()
        {
            var size = new Size(10, 10);
            var rect1 = layouter.PutNextRectangle(size);
            var rect2 = layouter.PutNextRectangle(size);
            
            rect1.Should().Match<Rectangle>(rect => !rect.IntersectsWith(rect2));
        }
    }
}