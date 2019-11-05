using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using System.Drawing;
using System.Reflection;
using System.Linq;

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
        public void PutNextRectangleOnce_centerShouldBeInTheMiddleOfFirstRectangle()
        {
            var center = new Point(0, 0);
            var layouter = new CircularCloudLayouter(center);
            var rect = layouter.PutNextRectangle(new Size(10, 10));
            rect.Location.Should().Be(new Point(-5, -5));
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
        public void AddingThreeRectangles_ShouldNotIntersect()
        {
            var center = new Point(0, 0);
            var layouter = new CircularCloudLayouter(center);
            var rect1 = layouter.PutNextRectangle(new Size(10, 10));
            var rect2 = layouter.PutNextRectangle(new Size(10, 10));
            var rect3 = layouter.PutNextRectangle(new Size(10, 10));
            rect3.IntersectsWith(rect2).Should().BeFalse();
            rect3.IntersectsWith(rect1).Should().BeFalse();
        }

        [Test]
        public void AddingFourRectangles_ShouldNotIntersect()
        {
            var center = new Point(0, 0);
            var layouter = new CircularCloudLayouter(center);
            var rect1 = layouter.PutNextRectangle(new Size(10, 10));
            var rect2 = layouter.PutNextRectangle(new Size(10, 10));
            var rect3 = layouter.PutNextRectangle(new Size(10, 10));
            var rect4 = layouter.PutNextRectangle(new Size(10, 10));
            rect4.IntersectsWith(rect3).Should().BeFalse();
            rect4.IntersectsWith(rect2).Should().BeFalse();
            rect4.IntersectsWith(rect1).Should().BeFalse();
        }

        [Test]
        public void AddingFiveEqualSquares_ShouldMakeACross()
        {
            var center = new Point(0, 0);
            var squareSize = new Size(10, 10);
            var layouter = new CircularCloudLayouter(center);
            layouter.PutNextRectangle(squareSize).Should().Be(new Rectangle(new Point(-5,-5),squareSize));
            layouter.PutNextRectangle(squareSize).Should().Be(new Rectangle(new Point(-5, -15), squareSize));
            layouter.PutNextRectangle(squareSize).Should().Be(new Rectangle(new Point(-5, 5), squareSize));
            layouter.PutNextRectangle(squareSize).Should().Be(new Rectangle(new Point(-15, -5), squareSize));
            layouter.PutNextRectangle(squareSize).Should().Be(new Rectangle(new Point(5, -5), squareSize));
        }

    }
}
