using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter_should
    {

        private static Point center;
        private static CircularCloudLayouter layouter;

        [SetUp]
        public void SetUp()
        {
            center = new Point(10, 10);
            layouter = new CircularCloudLayouter(center);
        }

        [Test]
        public void Constructor_WhenInitialized_CenterIsPointInInitialized()
        {
            layouter.Center.Should().Be(center);
        }

        [TestCase(-10, -10)]
        [TestCase(-10, 10)]
        [TestCase(10, -10)]
        public void Constructor_WhenInitializedPointWithNegativeCoords_ThrowArgumentException(int x, int y)
        {
            var center = new Point(x, y);
            Action action = () =>
            {
                // ReSharper disable once ObjectCreationAsStatement
                new CircularCloudLayouter(center);
            };
            action.ShouldThrow<ArgumentException>();
        }

        [TestCase(0, 0)]
        [TestCase(-5, -5)]
        [TestCase(-5, 0)]
        [TestCase(-5, 5)]
        public void PutNextRectangle_WhenSizeIsNegative_ThrowArgumentException(int width, int height)
        {
            Action action = () =>
            {
                // ReSharper disable once ObjectCreationAsStatement
                layouter.PutNextRectangle(new Size(width, height));
            };
            action.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void PutNextRectangle_WhenPutOneRectangle_RectanglesContainThisRectangle()
        {
            var size = new Size(4,4);
            var rectangle = layouter.PutNextRectangle(size);
            rectangle.GetCenter().Should().Be(center);
            rectangle.Size.Should().Be(size);
        }

        [Test]
        public void DoSomething_WhenSomething()
        {
            var size = new Size(5, 5);

            var rectangle1 = layouter.PutNextRectangle(size);

            rectangle1.GetCenter().Should().Be(center);
            rectangle1.Size.Should().Be(size);

            var rectangle2 = layouter.PutNextRectangle(size);

            rectangle2.IntersectsWith(rectangle1).Should().BeFalse();
        }
    }
}
