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
        public void AddingTwoRectangles_Should()
        {
            var center = new Point(0, 0);
            var squareSize = new Size(10, 10);
            var layouter = new CircularCloudLayouter(center);
            layouter.PutNextRectangle(squareSize).Should().Be(new Rectangle(new Point(-5, -5), squareSize));
            layouter.PutNextRectangle(squareSize).Should().Be(new Rectangle(new Point(-5, -15), squareSize));

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
            layouter.PutNextRectangle(squareSize).Should().Be(new Rectangle(new Point(-5, 5), squareSize));

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
        // единственный случай добавления не в угол это когда координата центра раскладки совпадает с координатой центра прямоугольника
        [Test]//падает потому, что не умеет добавлять посередине отрезка а только в углы
        public void AddingFiveEqualSquares_ShouldMakeACross()//надо переделать на parametrized
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
            layouter.PutNextRectangle(new Size(10, 30)).Should().Be(new Rectangle(new Point(-15, -15), new Size(10, 30)));
        }

    }
}
