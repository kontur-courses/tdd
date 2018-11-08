using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualisationTests
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        [TestCase(0, 0, TestName = "Zero rectangle size")]
        [TestCase(100, -100, TestName = "Negative rectangle height")]
        [TestCase(-100, 100, TestName = "Negative rectangle width")]
        [TestCase(-100, -100, TestName = "Negative rectangle size")]
        public void PutNextRectangle_Throws_ArgumentException(int width, int height)
        {
            var tagCloud = new CircularCloudLayouter(new Point(0, 0));

            Action putRectangleAct = () => tagCloud.PutNextRectangle(new Size(width, height));

            putRectangleAct.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Ctor_HasCorrectCenter()
        {
            var center = new Point(123, 456);

            var tagCloud = new CircularCloudLayouter(center);

            tagCloud.Center.Should().Be(center);
        }

        [Test]
        public void PutNextRectangle_CenterOfFirstRectangleIsCenterOfLayout()
        {
            var center = new Point(200, 200);
            var rectSize = new Size(350, 420);
            var expectedRectLocation = new Point(
                center.X - rectSize.Width / 2,
                center.Y - rectSize.Height / 2);
            var tagCloud = new CircularCloudLayouter(center);

            var resultRect = tagCloud.PutNextRectangle(rectSize);

            resultRect.Location.Should().Be(expectedRectLocation);
        }

        [Test]
        public void PutNextRectangle_ReturnsRectangleWithCorrectSize()
        {
            var rectSize = new Size(200, 155);
            var tagCloud = new CircularCloudLayouter(new Point(0, 0));

            var resultRect = tagCloud.PutNextRectangle(rectSize);

            resultRect.Size.Should().Be(rectSize);
        }

        [TestCase(2)]
        [TestCase(10)]
        [TestCase(100)]
        public void HasCorrectCount_WhenMultipleRectanglesAdded(int count)
        {
            var size = new Size(10, 10);
            var tagCloud = new CircularCloudLayouter(new Point(0, 0));

            for (var i = 0; i < count; i++)
                tagCloud.PutNextRectangle(size);

            tagCloud.Rectangles.Count.Should().Be(count);
        }

        [Test]
        public void PutNextRectangle_RectanglesDoNotIntersect()
        {
            const int count = 100;
            var size = new Size(10, 10);
            var tagCloud = new CircularCloudLayouter(new Point(0, 0));

            for (var i = 0; i < count; i++)
                tagCloud.PutNextRectangle(size);

            tagCloud.Rectangles
                .Any(rect => tagCloud.Rectangles
                    .Where(r => !r.Equals(rect))
                    .Any(r => r.IntersectsWith(rect)))
                .Should().BeFalse();
        }

        [Test, Timeout(2000)]
        public void PutNextRectangle_WorksFast_When500RectanglesArePutIn()
        {
            const int count = 500;
            var size = new Size(10, 10);
            var tagCloud = new CircularCloudLayouter(new Point(0, 0));
            for (var i = 0; i < count; i++)
                tagCloud.PutNextRectangle(size);
        }
    }
}