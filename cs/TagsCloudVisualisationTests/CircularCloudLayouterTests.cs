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
        [SetUp]
        public void SetUp()
        {
            center = new Point(0, 0);
            tagCloud = new CircularCloudLayouter(center);
        }

        [TearDown]
        public void TearDown()
        {
            var testContext = TestContext.CurrentContext;
            var filename = $"{testContext.WorkDirectory}/{testContext.Test.Name}.png";

            if (testContext.Result.FailCount == 0) return;

            var bmp = new CircularCloudVisualizer().DrawRectangles(tagCloud.Rectangles, tagCloud.Radius);
            bmp.Save(filename);
            TestContext.WriteLine($"Tag cloud visualization saved to file {filename}");
        }

        private CircularCloudLayouter tagCloud;
        private Point center;

        [TestCase(0, 0, TestName = "Zero rectangle size")]
        [TestCase(100, -100, TestName = "Negative rectangle height")]
        [TestCase(-100, 100, TestName = "Negative rectangle width")]
        [TestCase(-100, -100, TestName = "Negative rectangle size")]
        public void PutNextRectangle_Throws_ArgumentException(int width, int height)
        {
            Action putRectangleAct = () => tagCloud.PutNextRectangle(new Size(width, height));

            putRectangleAct.Should().Throw<ArgumentException>();
        }

        [TestCase(0, 0, 0, 0, TestName = "Empty layouter")]
        [TestCase(0, 0, 1, 40, TestName = "One rectangle, zero center")]
        [TestCase(10, -60, 1, 65, TestName = "One rectangle, non-zero center")]
        [TestCase(0, 0, 2, TestName = "Two rectangles, zero center")]
        [TestCase(0, 0, 500, TestName = "Several rectangles, zero center")]
        public void HasCorrectRadius(int centerX, int centerY, int rectCount, int expectedRadiusTreshold = -1)
        {
            var countExpected = expectedRadiusTreshold < 0;

            var size = new Size(80, 10);
            center = new Point(centerX, centerY);

            tagCloud = new CircularCloudLayouter(center);
            for (var i = 0; i < rectCount; i++)
            {
                var rectangle = tagCloud.PutNextRectangle(size);
                if (countExpected)
                    expectedRadiusTreshold = Math.Max(
                        expectedRadiusTreshold,
                        Math.Max(
                            Math.Max(Math.Abs(rectangle.Left), Math.Abs(rectangle.Right)),
                            Math.Max(Math.Abs(rectangle.Top), Math.Abs(rectangle.Bottom))));
            }

            tagCloud.Radius.Should().Be(expectedRadiusTreshold);
        }

        [TestCase(2)]
        [TestCase(10)]
        [TestCase(100)]
        public void HasCorrectCount_WhenMultipleRectanglesAdded(int count)
        {
            var size = new Size(80, 10);

            for (var i = 0; i < count; i++)
                tagCloud.PutNextRectangle(size);

            tagCloud.Rectangles.Count.Should().Be(count);
        }

        [Test]
        public void HasCorrectCenter()
        {
            center = new Point(123, 456);

            tagCloud = new CircularCloudLayouter(center);

            tagCloud.Center.Should().Be(center);
        }

        [Test]
        public void Is_Dense()
        {
            var cloudArea = 0;
            var size = new Size(80, 10);

            for (var i = 0; i < 500; i++)
            {
                var rectangle = tagCloud.PutNextRectangle(size);
                cloudArea += rectangle.Width * rectangle.Height;
            }

            var cloudRadiusFromFirstRectangle =
                tagCloud.Radius - Math.Max(Math.Abs(tagCloud.Center.X), Math.Abs(tagCloud.Center.Y));

            var circleArea = Math.PI * Math.Pow(cloudRadiusFromFirstRectangle, 2);
            circleArea.Should().BeGreaterOrEqualTo(cloudArea);
        }

        [Test]
        public void PutNextRectangle_CenterOfFirstRectangleIsCenterOfLayout()
        {
            center = new Point(200, 200);
            var rectSize = new Size(350, 420);
            var expectedRectLocation = new Point(
                center.X - rectSize.Width / 2,
                center.Y - rectSize.Height / 2);
            tagCloud = new CircularCloudLayouter(center);

            var resultRect = tagCloud.PutNextRectangle(rectSize);

            resultRect.Location.Should().Be(expectedRectLocation);
        }

        [Test]
        public void PutNextRectangle_RectanglesDoNotIntersect()
        {
            const int count = 100;
            var size = new Size(80, 10);

            for (var i = 0; i < count; i++)
                tagCloud.PutNextRectangle(size);

            tagCloud.Rectangles
                .Any(rect => tagCloud.Rectangles
                    .Where(r => !r.Equals(rect))
                    .Any(r => r.IntersectsWith(rect)))
                .Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_ReturnsRectangleWithCorrectSize()
        {
            var rectSize = new Size(200, 155);

            var resultRect = tagCloud.PutNextRectangle(rectSize);

            resultRect.Size.Should().Be(rectSize);
        }

        [Test]
        [Timeout(2000)]
        public void PutNextRectangle_WorksFast_When500RectanglesArePutIn()
        {
            const int count = 500;
            var size = new Size(80, 10);
            for (var i = 0; i < count; i++)
                tagCloud.PutNextRectangle(size);
        }
    }
}