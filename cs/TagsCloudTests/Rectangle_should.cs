using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
    [TestFixture]
    class Rectangle_should
    {
        [Test]
        public void ReturnFalse_WhenRectanglesAreNotIntersect()
        {
            var size = new Size(10, 10);
            var rect1 = new Rectangle(new Point(0, 0), size);
            var rect2 = new Rectangle(new Point(20, 0), size);

            var result = rect1.Intersects(rect2);

            result.Should().BeFalse();
        }

        [Test]
        public void ReturnTrue_WhenRectanglesAreIntersect()
        {
            var size = new Size(10, 10);
            var rect1 = new Rectangle(new Point(0, 0), size);
            var rect2 = new Rectangle(new Point(5, 0), size);

            var result = rect1.Intersects(rect2);

            result.Should().BeTrue();
        }
    }
}
