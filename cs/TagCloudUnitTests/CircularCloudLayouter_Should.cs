using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagCloud;
using System.Linq;

namespace TagCloudUnitTests
{
    [TestFixture]
    internal class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter _layouter;

        [SetUp]
        public void Setup()
        {
            _layouter = new CircularCloudLayouter(new Point(0, 0));
        }

        [Test]
        public void HaveNoOneRectangles_WhenCreated()
        {
            _layouter.Rectangles.Count.Should().Be(0);
        }

        [Test]
        public void HaveOneRectangles_WhenOneRectangleAdded()
        {
            _layouter.PutNextRectangle(new Size(10, 10));

            _layouter.Rectangles.Count.Should().Be(1);
        }

        [Test]
        public void PutInTheCenter_WhenFirstRectangleAdded()
        {
            _layouter.PutNextRectangle(new Size(100, 100));

            _layouter.Rectangles.First().GetCenter().Should().BeEquivalentTo(_layouter.CloudCenter);
        }

        [TestCase(-1, 1, TestName = "negative width")]
        [TestCase(1, -1, TestName = "negative height")]
        [TestCase(-1, -1, TestName = "negative width and height")]
        [TestCase(0, 1, TestName = "zero width")]
        [TestCase(1, 0, TestName = "zero height")]
        [TestCase(0, 0, TestName = "zero width and height")]
        public void ThrowArgumentException_WhenRectangleSizeHasInvalidWidthOrHeight(int width, int height)
        {
            Action action = () => _layouter.PutNextRectangle(new Size(width, height));

            action.Should().Throw<ArgumentException>().WithMessage("width and height of rectangle must be more than zero");
        }

        [Test]
        public void HaveTwoAlignedHorizontallySquares_WhenTwoSquaresAdded()
        {
            _layouter.PutNextRectangle(new Size(100, 100));
            _layouter.PutNextRectangle(new Size(100, 100));

            var firstRectangle = _layouter.Rectangles[0];
            var secondRectangle = _layouter.Rectangles[1];

            Math.Abs(firstRectangle.Y).Should().Be(Math.Abs(secondRectangle.Y));
        }

        [Test]
        public void HaveTwoAlignedVerticallySquares_WhenTwoSquaresAdded()
        {
            _layouter.PutNextRectangle(new Size(150, 150));
            _layouter.PutNextRectangle(new Size(150, 150));

            var firstRectangle = _layouter.Rectangles[0];
            var secondRectangle = _layouter.Rectangles[1];

            Math.Abs(firstRectangle.X).Should().Be(Math.Abs(secondRectangle.X));
        }

        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        public void HaveNonIntersectingRectangles_WhenSeveralRectanglesAdded(int rectanglesCount)
        {
            var sizes = RectangleSizeGenerator.GetConstantSizesList(rectanglesCount, new Size(50, 25));

            IsAnyIntersection().Should().Be(false);
        }

        private bool IsAnyIntersection()
        {
            foreach (var rectangle in _layouter.Rectangles)
                if (_layouter.Rectangles.Any(r => r.IntersectsWith(rectangle)))
                    return true;

            return false;
        }
    }
}