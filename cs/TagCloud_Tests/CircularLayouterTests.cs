using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagCloud.Layouting;
using TagCloud_TestDataGenerator;

namespace TagCloudVisualization_Tests
{
    public class CircularLayouterTests
    {
        [Test]
        public void Constructor_ShouldNotThrow_WhenCreated()
        {
            Action act = () => new CircularCloudLayouter(Point.Empty);
            act.Should().NotThrow();
        }

        [Test]
        public void Constructor_ShouldAllowNegativeXY_ForCloudCenter()
        {
            var center = new Point(-1, -1);

            var layouter = new CircularCloudLayouter(center);

            layouter.Center.Should().BeEquivalentTo(center);
        }

        [Test]
        public void Constructor_ShouldSetCenter_WhenCreate()
        {
            var centerPoint = new Point(10, 10);

            var layouter = new CircularCloudLayouter(centerPoint);

            layouter.Center.Should().BeEquivalentTo(centerPoint);
        }

        [Test]
        public void PutNextRectangle_ShouldReturnRectangle_AfterFirstPut()
        {
            var layouter = new CircularCloudLayouter(Point.Empty);

            var expectedRect = new Rectangle(Point.Empty, new Size(10, 10));
            var actualRect = layouter.PutNextRectangle(new Size(10, 10));

            actualRect.Size.Should().BeEquivalentTo(expectedRect.Size);
        }

        [Test]
        public void PutNextRectangle_ShouldThrow_WhenIncorrectSize(
            [ValueSource(nameof(IncorrectRectangleSizes))]
            Size rectSize)
        {
            var layouter = new CircularCloudLayouter(Point.Empty);

            Action act = () => layouter.PutNextRectangle(rectSize);

            act.Should().Throw<ArgumentException>("Incorrect size:", rectSize);
        }

        [Test]
        public void PutNextRectangle_ShouldAlignRectangleMiddlePointToCenter()
        {
            var layouter = new CircularCloudLayouter(Point.Empty);
            var rectSize = new Size(10, 10);

            var rect = layouter.PutNextRectangle(rectSize);
            rect.Location
                .Should()
                .BeEquivalentTo(new Point(-5, -5));
        }

        [Test]
        public void PutNextRectangle_ShouldPutSecondRectangleAboveFirst()
        {
            var layouter = new CircularCloudLayouter(Point.Empty);
            var firstRectSize = new Size(10, 10);
            var secondRectSize = new Size(15, 10);

            var firstRect = layouter.PutNextRectangle(firstRectSize);
            var secondRect = layouter.PutNextRectangle(secondRectSize);

            secondRect.Bottom.Should().BeLessThan(firstRect.Top);
        }

        [Test]
        public void PutNextRectangle_ShouldNotIntersect_TwoRectangles()
        {
            var layouter = new CircularCloudLayouter(Point.Empty);

            var firstRect = layouter.PutNextRectangle(new Size(10, 10));
            var secondRect = layouter.PutNextRectangle(new Size(5, 15));

            firstRect.IntersectsWith(secondRect).Should().BeFalse();
        }

        [TestCase(2)]
        [TestCase(4)]
        [TestCase(8)]
        [TestCase(16)]
        [TestCase(32)]
        [TestCase(64)]
        public void PutNextRectangle_ShouldNotIntersect_ForEachRectangle(int n)
        {
            var layouter = new CircularCloudLayouter(Point.Empty);

            var rectangles = new List<Rectangle>(n);
            foreach (var rectSize in DataGenerator.GetNextNSizes(n))
                rectangles.Add(layouter.PutNextRectangle(rectSize));

            IsIntersectExist(rectangles).Should().BeFalse();
        }

        private static bool IsIntersectExist(List<Rectangle> rectangles)
        {
            for (var i = 0; i < rectangles.Count; i++)
            {
                var rect = rectangles[i];
                for (var j = 1; j < rectangles.Count; j++)
                    if (rect.IntersectsWith(rectangles[j]))
                        return true;
            }

            return false;
        }

        private static IEnumerable<Size> IncorrectRectangleSizes()
        {
            yield return new Size(-1, -1);
            yield return new Size(-1, 0);
            yield return new Size(0, -1);
            yield return new Size(0, 0);
        }
    }
}