using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.CloudLayouts;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CircularCloudLayout_should
    {
        [Test]
        public void PutNextRectangle_FirstRectangle_ShouldBeInCenter()
        {
            var center = new Point();
            var size = new Size(100, 100);
            var expected = new Rectangle(center, size);
            var layout = new CircularCloudLayout(center);

            var result = layout.PutNextRectangle(size);

            result.Should().Be(expected);
        }

        [Test]
        public void PutNextRectangle_SecondRectangle_ShouldNotIntersect_WithFirst()
        {
            var size = new Size(100, 100);
            var center = new Point();
            var layout = new CircularCloudLayout(center);

            var firstRectangle = layout.PutNextRectangle(size);
            var secondRectangle = layout.PutNextRectangle(size);

            firstRectangle.Should().NotBe(secondRectangle);
        }

        [Test]
        public void PutNextRectangle_ShouldBe()
        {
            var size = new Size(100, 100);
            var center = new Point();
            var layout = new CircularCloudLayout(center);

            for (var i = 0; i < 10; i++)
            {
                var rect = layout.PutNextRectangle(size);
            }
        }
    }
}