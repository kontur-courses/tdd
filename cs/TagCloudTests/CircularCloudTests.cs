using FluentAssertions;
using NUnit.Framework;
using System;
using System.Drawing;
using TagsCloudVisualization.TagCloud;

namespace TagCloudTests
{
    [TestFixture]
    public class CircularCloudTests
    {
        private static Point _center;
        private CircularCloudLayouter layouter;

        [SetUp]
        public void SetUP()
        {
            _center = new(1920 / 2, 1080 / 2);
            layouter = new CircularCloudLayouter(_center);
        }

        [Test]
        public void CircularCloudLayouter_HaveNoOneRectangle_whenLayouterCreate()
        {
            layouter.rectangles.Count.Should().Be(0);
        }

        [Test]
        public void CircularCloudLayouter_CheckRectangleSize()
        {
            var size = new Size(20, 70);
            var expectedSize = new Size(20, 70);

            var nextRectangle = layouter.PutNextRectangle(size);

            nextRectangle.Size.Should().Be(expectedSize);
        }

        [Test]
        public void CircularCloudLayouter_HaveOneRectangle_whenPutSingleRectangle()
        {
            layouter.PutNextRectangle(new Size(20, 10));
            layouter.rectangles.Count.Should().Be(1);
        }

        [TestCase(1920 / 2, 1080 / 2, TestName = "{m}_when(X,Y)IsPositive")]
        [TestCase(0, 0, TestName = "{m}_(X,Y)IsZero")]
        [TestCase(-1920 / 2, -1080 / 2, TestName = "{m}_when(X,Y)IsNegative")]
        public void CircularCloudLayouter_Constructor_ShouldNotThrow(int x, int y)
        {
            var point = new Point(x, y);
            Action act = () => new CircularCloudLayouter(point);
            act.Should().NotThrow();
        }

        [TestCase(0, 0, TestName = "{m}_whenSizeIsZero")]
        [TestCase(-1920 / 2, -1080 / 2, TestName = "{m}_when(X,Y)IsNegative")]
        public void CircularCloudLayouter_PutNextRectangle_ShouldThrow(int width, int height)
        {
            var size = new Size(width, height);
            Action act = () => layouter.PutNextRectangle(size);
            act.Should().Throw<ArgumentException>();
        }

        [TestCase(null, TestName = "{m}_whenSizeIsNull")]
        public void CircularCloudLayouter_PutNextRectangle_ShouldThrow(Size size)
        {
            Action act = () => layouter.PutNextRectangle(size);
            act.Should().Throw<ArgumentException>();
        }
    }
}