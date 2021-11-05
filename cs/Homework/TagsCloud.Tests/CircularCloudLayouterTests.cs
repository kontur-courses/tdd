using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.Visualization;
using TagsCloud.Visualization.Extensions;
using TagsCloud.Visualization.PointGenerator;

namespace TagsCloud.Tests
{
    public class CircularCloudLayouterTests
    {
        private Point center;
        private CircularCloudLayouter layouter;

        [SetUp]
        public void InitLayouter()
        {
            center = new Point(10, 10);
            layouter = new CircularCloudLayouter(center, new ArchimedesSpiralPointGenerator(center));
        }

        [TestCase(0, 1, TestName = "Only one is zero")]
        [TestCase(0, 0, TestName = "Both coordinates are zero")]
        [TestCase(-1, 3, TestName = "Negative width")]
        public void PutNextRectangle_Should_ThrowException_WhenSizeNotPositive(int width, int height)
        {
            var size = new Size(width, height);

            Assert.Throws<ArgumentException>(() => layouter.PutNextRectangle(size));
        }

        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        public void PutNextRectangle_RectanglesAmount_ShouldBeEqual_AmountOfAdded(int count)
        {
            var rectangles = PutRandomRectangles(count);

            rectangles.Should().HaveCount(count);
        }

        [Test]
        public void PutNextRectangle_ShouldNot_ChangeSize()
        {
            var size = new Size(12, 34);

            var rectangle = layouter.PutNextRectangle(size);

            rectangle.Size.Should().Be(size);
        }

        [TestCase(10, 10)]
        [TestCase(10, 1)]
        [TestCase(123, 45)]
        [TestCase(1, 200)]
        public void PutNextRectangle_FirstRectangle_Should_BePlacedInCenter(int width, int height)
        {
            var rectangle = layouter.PutNextRectangle(new Size(width, height));

            var rectangleCenter = rectangle.GetCenter();

            rectangleCenter.Should().Be(center);
        }

        [Test]
        public void PutNextRectangle_Rectangles_Should_HaveDifferentCentres()
        {
            var rectangles = PutRandomRectangles(1000);

            rectangles.Should().OnlyHaveUniqueItems(x => x.GetCenter());
        }

        [Test]
        public void Rectangles_ShouldNot_Intersect()
        {
            var rectangles = PutRandomRectangles(1000).ToList();

            foreach (var (rectangle, otherRectangles) in GetItemAndListWithoutIt(rectangles))
                rectangle.IntersectsWith(otherRectangles).Should().BeFalse();
        }

        [TestCase(200, TestName = "Big count")]
        [TestCase(5, TestName = "Little count")]
        public void ResultLayout_Should_BeCloseToRoundForm(int count)
        {
            var random = new Random();
            var width = random.Next(300);
            var height = random.Next(200);
            var size = new Size(width, height);

            var rectangles = Enumerable.Range(0, count)
                .Select(_ => layouter.PutNextRectangle(size));

            foreach (var rectangle in rectangles)
                rectangle.Location.GetDistance(center).Should()
                    .BeLessThan(Math.Sqrt(width * height * count));
        }

        [TestCase(1000, TestName = "Big count")]
        [TestCase(5, TestName = "Little count")]
        public void Rectangles_Should_BeTightlySpaced(int count)
        {
            var rectangles = PutRandomRectangles(count).ToList();

            var expected = Math.Max(
                rectangles.Max(rectangle => rectangle.Width),
                rectangles.Max(rectangle => rectangle.Height)
            );

            foreach (var (rect, otherRects) in GetItemAndListWithoutIt(rectangles))
            {
                var minDistanceToOtherRectangles = otherRects
                    .Min(x => x.GetCenter().GetDistance(rect.GetCenter()));

                minDistanceToOtherRectangles.Should().BeLessOrEqualTo(expected);
            }
        }

        private IEnumerable<Rectangle> PutRandomRectangles(int count)
        {
            const int maxWidth = 100;
            const int maxHeight = 100;
            var rnd = new Random();
            var sizes = Enumerable.Range(0, count)
                .Select(_ => new Size(rnd.Next(1, maxWidth), rnd.Next(1, maxHeight)));

            return sizes.Select(x => layouter.PutNextRectangle(x));
        }

        private IEnumerable<(Rectangle, IEnumerable<Rectangle>)> GetItemAndListWithoutIt(
            IReadOnlyCollection<Rectangle> rectangles)
        {
            return rectangles.Select(x => (x, rectangles.Where(y => y != x)));
        }
    }
}