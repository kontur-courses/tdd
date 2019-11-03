using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
    [TestFixture]
    class TagsCloudVisualization_should
    {
        static CircularCloudLayouter cloudLayouter;

        [SetUp]
        public void SetUp() => cloudLayouter = new CircularCloudLayouter(new Point(0, 0), new RectangularSpiral());

        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        [TestCase(-1, -1)]
        public void PutNextRectangle_ThrowArgumentException_WhenSizeParamsIsNegative(int width, int height)
        {
            Action act = () => cloudLayouter.PutNextRectangle(new Size(width, height));
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void PutNextRectangle_ThrowArgumentException_WhenSizeIsEmpty()
        {
            Action act = () => cloudLayouter.PutNextRectangle(Size.Empty);
            act.Should().Throw<ArgumentException>();
        }

        [TestCase(5,5)]
        [TestCase(7, 2)]
        [TestCase(100, 123)]
        public void PutNextRectangle_ShouldReturnRectangle_WithGivenSize(int width, int height)
        {
            var rectangle = cloudLayouter.PutNextRectangle(new Size(width, height));

            rectangle.Size.Should().Be(new Size(width, height));
        }

        [TestCase(0, 0)]
        [TestCase(2, 3)]
        [TestCase(-4, 1)]
        public void PutNextRectangle_ShouldBePutFirstRectangle_InCenter(int x, int y)
        {
            var center = new Point(x, y);
            var cloudLayouter = new CircularCloudLayouter(center, new RectangularSpiral());

            var firstRectangle = cloudLayouter.PutNextRectangle(new Size(1, 1));

            firstRectangle.Location.Should().Be(center);
        }

        [Test]
        public void PutNextRectangle_FirstAndSecondRectangles_Should_NotIntersect()
        {
            var firstRectangle = cloudLayouter.PutNextRectangle(new Size(3, 4));
            var secondRectangle = cloudLayouter.PutNextRectangle(new Size(10, 6));

            firstRectangle.IntersectsWith(secondRectangle).Should().BeFalse();
        }

        [TestCase(10)]
        [TestCase(25)]
        [TestCase(50)]
        [TestCase(100)]
        public void PutNextRectangle_AllRectanglesOnOneCloud_Should_NotIntersect(int countRectangles)
        {
            var random = new Random();
            var rectangles = new List<Rectangle>();

            for (int i = 0; i < countRectangles; i++)
            {
                var size = new Size(random.Next(10, 20), random.Next(10, 20));
                rectangles.Add(cloudLayouter.PutNextRectangle(size));
            }

            for (int i = 0; i < rectangles.Count; i++)
                for (int j = i + 1; j < rectangles.Count; j++)
                    rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
        }
    }
}
