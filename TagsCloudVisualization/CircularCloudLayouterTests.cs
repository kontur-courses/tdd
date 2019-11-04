using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using System.Drawing;

namespace TagsCloudVisualization
{
    [TestFixture]
    class CircularCloudLayouterTests
    {
        [Test]
        public void Constructor_shouldNotThrowArgumentExceptionOnProperInput()
        {
            Action LayouterCreation = () => new CircularCloudLayouter(new Point(0, 0));
            LayouterCreation.Should().NotThrow<Exception>();
        }
        [Test]
        public void PutNextRectangle_shouldNotThrowArgumentExceptionOnProperInput()
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            Action putNext = ()=>layouter.PutNextRectangle(new Size(1, 1));
            putNext.Should().NotThrow<Exception>();
        }

        [Test]
        public void PutNextRectangleOnce_centerShouldBeInFirstRectangle()
        {
            var center = new Point(0, 0);
            var layouter = new CircularCloudLayouter(center);
            var rect = layouter.PutNextRectangle(new Size(10, 10));
            rect.Contains(center).Should().BeTrue();
        }
        [Test]
        public void AddingTwoRectangles_ShouldNotIntersect()
        {
            var center = new Point(0, 0);
            var layouter = new CircularCloudLayouter(center);
            var rect1 = layouter.PutNextRectangle(new Size(10, 10));
            var rect2 = layouter.PutNextRectangle(new Size(10, 10));
            rect1.IntersectsWith(rect2).Should().BeFalse();
        }
        [Test]
        public void PuttingFourEqualRectangles_ShouldMakeRectangleLayout()
        {
            var expected = new Rectangle(new Point(-10, -10), new Size(20, 20));
            var center = new Point(0, 0);
            var layouter = new CircularCloudLayouter(center);
            var rectangles = new List<Rectangle>();
            for (int i=0; i<4;i++)
                rectangles.Add(layouter.PutNextRectangle(new Size(10, 10)));
            var actual = new Rectangle();
            for (int i = 0; i < 4; i++)
                actual = Rectangle.Union(actual, rectangles[i]);
            actual.Should().Be(expected);
        }
    }
}
