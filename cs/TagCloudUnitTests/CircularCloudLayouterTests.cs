using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagCloud;

namespace TagCloudUnitTests
{
    [TestFixture]
    internal class CircularCloudLayouterTests
    {
        private CircularCloudLayouter layouter;

        [SetUp]
        public void Setup()
        {
            layouter = new CircularCloudLayouter(new Point(0, 0));
        }

        [Test]
        public void PutNextRectangle_PutsRectangleInTheCenter_WhenFirstRectangleAdded()
        {
            var rectangle = layouter.PutNextRectangle(new Size(100, 50));

            rectangle.GetCenter().Should().BeEquivalentTo(layouter.CloudCenter);
        }

        [TestCase(-1, 1, TestName = "negative width")]
        [TestCase(1, -1, TestName = "negative height")]
        [TestCase(-1, -1, TestName = "negative width and height")]
        [TestCase(0, 1, TestName = "zero width")]
        [TestCase(1, 0, TestName = "zero height")]
        [TestCase(0, 0, TestName = "zero width and height")]
        public void PutNextRectangle_ThrowsArgumentException_WhenSizeIsInvalid(int width, int height)
        {
            Action action = () => layouter.PutNextRectangle(new Size(width, height));

            action.Should().Throw<ArgumentException>().WithMessage("width and height of rectangle must be more than zero");
        }

        [Test]
        public void PutNextRectangle_PutsTwoAlignedHorizontallyRectangles_WhenTwoAdded()
        {
            var sizes = new[]
            {
                new Size(100, 50),
                new Size(100, 50)
            };

            var layout = GetLayout(sizes);

            var firstRectangle = layout[0];
            var secondRectangle = layout[1];

            Math.Abs(firstRectangle.Y).Should().Be(Math.Abs(secondRectangle.Y));
        }

        [Test]
        public void PutNextRectangle_PutsTwoAlignedVerticallyRectangles_WhenTwoAdded()
        {
            var sizes = new[]
            {
                new Size(150, 100),
                new Size(150, 100)
            };

            var layout = GetLayout(sizes);

            var firstRectangle = layout[0];
            var secondRectangle = layout[1];

            Math.Abs(firstRectangle.X).Should().Be(Math.Abs(secondRectangle.X));
        }

        [TestCase(2)]
        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        public void PutNextRectangle_PutsNonIntersectingRectangles_WhenSeveralRectanglesAdded(int rectanglesCount)
        {
            var sizes = RectangleSizeGenerator.GetConstantSizes(rectanglesCount, new Size(100, 100));

            var layout = GetLayout(sizes);

            IsAnyIntersection(layout).Should().Be(false);
        }

        private bool IsAnyIntersection(Rectangle[] layout)
        {
            foreach (var rectangle in layout)
                if (layout.Where(r => !r.Equals(rectangle)).Any(r => r.IntersectsWith(rectangle)))
                    return true;

            return false;
        }

        private Rectangle[] GetLayout(IReadOnlyList<Size> rectanglesSizes)
        {
            var rectangles = new List<Rectangle>();

            foreach (var size in rectanglesSizes)
                rectangles.Add(layouter.PutNextRectangle(size));

            return rectangles.ToArray();
        }
    }
}