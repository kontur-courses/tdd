using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Models;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    class CircularCloudLayouterTests
    {
        private Point center;
        private CircularCloudLayouter layouter;

        [SetUp]
        public void SetUp()
        {
            center = new Point(300, 300);
            layouter = new CircularCloudLayouter(center);
        }

        [Test]
        [TestCase(0, 0, TestName = "CenterInStartOfCoordinates")]
        [TestCase(2, 2, TestName = "CenterInFirstQuarter")]
        [TestCase(-2, -2, TestName = "CenterInThirdQuarter")]
        [TestCase(2, -2, TestName = "CenterCoordinatesHaveDifferentSigns")]
        public void CircularCloudLayouterConstructor_DoesNotThrow(int x, int y)
        {
            Assert.DoesNotThrow(
                () => new CircularCloudLayouter(new Point(x, y)));
        }

        [Test]
        [TestCase(0, TestName = "ZeroWhenNoPlaced")]
        [TestCase(5, TestName = "FiveWhenFivePlaced")]
        [TestCase(50, TestName = "FiftyWhenFiftyPlaced")]
        public void PutNextRectangle_RectanglesAmountAsSameAsPlaced(
            int rectsCount)
        {
            PutRectangles(rectsCount);
            layouter.Rectangles.Count.Should().Be(rectsCount);
        }

        [Test]
        public void PutNextRectangle_ShouldPlaceFirstRectangleToCenter()
        {
            var rect = layouter.PutNextRectangle(new Size(10, 10));
            rect.Location.Should().Be(center);
        }

        [Test]
        [TestCase(2, TestName = "TwoRectangles")]
        [TestCase(50, TestName = "FiftyRectangles")]
        [TestCase(100, TestName = "OneHundredRectangles")]
        public void PutNextRectangle_ShouldPlaceRectangleWithoutIntersection(
            int rectsCount)
        {
            PutRectangles(rectsCount, 11, 22);
            HaveAnyIntersections(layouter.Rectangles).Should().BeFalse();
        }

        [Test]
        [TestCase(4, 5, 5, TestName = "FourRectangles")]
        [TestCase(50, 5, 5, TestName = "FiftyRectangles")]
        [TestCase(100, 10, 10, TestName = "OneHundredRectangles")]
        public void PutNextRectangle_ShouldKeepCloudShapeCloseToCircle(
            int rectsCount, int width, int height)
        {
            var layoutArea = rectsCount * width * height;
            PutRectangles(rectsCount, width, height);

            var expectedRadius = Math.Sqrt(layoutArea / Math.PI);
            var deltaXFromCenter = (double)layouter.Rectangles
                .Select(rectangle => rectangle.Right).Max() - center.X;
            var deltaYFromCenter = (double)layouter.Rectangles
                .Select(rectangle => rectangle.Bottom).Max() - center.Y;

            deltaXFromCenter.Should().BeLessThan(expectedRadius * 1.25);
            deltaYFromCenter.Should().BeLessThan(expectedRadius * 1.25);
        }

        [Test]
        public void GetLayoutSize_ShouldThrowArgumentException_NoRectangles()
        {
            Assert.Throws<ArgumentException>(() => layouter.GetLayoutSize());
        }

        private bool HaveAnyIntersections(List<Rectangle> rects)
        {
            for (int i = 0; i < rects.Count; i++)
                for (int j = i + 1; j < rects.Count; j++)
                    if (rects[i].IntersectsWith(rects[j]))
                        return true;
            return false;
        }

        private void PutRectangles(int amountToPlace,
            int width = 10, int height = 10)
        {
            for (int i = 0; i < amountToPlace; i++)
                layouter.PutNextRectangle(new Size(width, height));
        }
    }
}
