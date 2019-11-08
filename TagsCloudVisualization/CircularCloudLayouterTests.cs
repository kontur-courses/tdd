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
    public class CircularCloudLayouterTests
    {
        [Test]
        public void ConstructorProperInput_shouldNotThrowArgumentException()
        {
            Action LayouterCreation = () => new CircularCloudLayouter(new Point(0, 0));
            LayouterCreation.Should().NotThrow<Exception>();
        }

        [Test]
        public void PutNextRectangleProperInput_shouldNotThrowArgumentException()
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            Action putNext = () => layouter.PutNextRectangle(new Size(1, 1));
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
        public void AddingTwoRectangles_RectanglesShouldNotIntersect()
        {
            var center = new Point(0, 0);
            var layouter = new CircularCloudLayouter(center);
            var rect1 = layouter.PutNextRectangle(new Size(10, 10));
            var rect2 = layouter.PutNextRectangle(new Size(10, 10));
            rect1.IntersectsWith(rect2).Should().BeFalse();
        }

        [Test]
        public void AddingTwoRectangles_Should()
        {
            var center = new Point(0, 0);
            var squareSize = new Size(10, 10);
            var layouter = new CircularCloudLayouter(center);
            var rect1 = layouter.PutNextRectangle(squareSize);
            var rect2 = layouter.PutNextRectangle(squareSize);
            rect1.Should().Be(new Rectangle(new Point(-5, -5), squareSize));
            rect2.Should().Be(new Rectangle(new Point(-5, -15), squareSize));

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
        public void AddingThreeRectangles_Should()
        {
            var center = new Point(0, 0);
            var squareSize = new Size(10, 10);
            var layouter = new CircularCloudLayouter(center);
            layouter.PutNextRectangle(squareSize);
            layouter.PutNextRectangle(squareSize);
            var rect = layouter.PutNextRectangle(squareSize);
            rect.Should().Be(new Rectangle(new Point(-5, 5), squareSize));

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
            var expectedSquares = new List<Rectangle>();
            expectedSquares.Add(new Rectangle(new Point(-5, -5), squareSize));
            expectedSquares.Add(new Rectangle(new Point(-5, -15), squareSize));
            expectedSquares.Add(new Rectangle(new Point(-5, 5), squareSize));
            expectedSquares.Add(new Rectangle(new Point(-15, -5), squareSize));
            expectedSquares.Add(new Rectangle(new Point(5, -5), squareSize));
            var actualSquares = new List<Rectangle>();
            actualSquares.Add(layouter.PutNextRectangle(squareSize));
            actualSquares.Add(layouter.PutNextRectangle(squareSize));
            actualSquares.Add(layouter.PutNextRectangle(squareSize));
            actualSquares.Add(layouter.PutNextRectangle(squareSize));
            actualSquares.Add(layouter.PutNextRectangle(squareSize));
            actualSquares.Should().Contain(expectedSquares);

        }

        [Test]
        public void CheckBorderStacking()
        {
            var center = new Point(0, 0);
            var squareSize = new Size(10, 10);
            var layouter = new CircularCloudLayouter(center);
            layouter.PutNextRectangle(squareSize);
            layouter.PutNextRectangle(squareSize);
            layouter.PutNextRectangle(squareSize);
            var rect = layouter.PutNextRectangle(new Size(10, 30));
            rect.Should().Be(new Rectangle(new Point(-15, -15), new Size(10, 30)));
        }

        [Test]
        public void AddingSquareAndBiggerSquare_SquaresShouldNotIntersect()
        {
            var center = new Point(0, 0);
            var squareSize = new Size(10, 10);
            var layouter = new CircularCloudLayouter(center);
            var rect1 = layouter.PutNextRectangle(squareSize);
            var rect2 = layouter.PutNextRectangle(new Size(20, 20));
            rect1.IntersectsWith(rect2).Should().Be(false);
        }

        [Test]
        public void AddingThreeDifferentSquares_ShouldNotIntersect()
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            var actualRect = new List<Rectangle>();
            actualRect.Add(layouter.PutNextRectangle(new Size(20, 20)));
            actualRect.Add(layouter.PutNextRectangle(new Size(10, 10)));
            actualRect.Add(layouter.PutNextRectangle(new Size(15, 15)));
            foreach (var rectangle1 in actualRect)
                foreach (var rectangle2 in actualRect)
                    if (rectangle1 != rectangle2)
                        rectangle1.IntersectsWith(rectangle2).Should().Be(false);
        }

    }
}
