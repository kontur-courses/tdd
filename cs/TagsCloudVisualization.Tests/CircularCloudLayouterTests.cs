using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization.Tests
{
    public class CircularCloudLayouterTests
    {
        private Point _center;
        private CircularCloudLayouter _layouter;

        [SetUp]
        public void Setup()
        {
            _center = new Point(0, 0);
            _layouter = new CircularCloudLayouter(_center);
        }

        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        public void PutNextRectangle_ShouldThrowException_WhenInvalidSize(int width, int height)
        {
            var size = new Size(width, height);
            Assert.Throws<ArgumentException>(() => _layouter.PutNextRectangle(size));
        }

        [Test]
        public void PutNextRectangle_ShouldBeCorrectSize()
        {
            var size = new Size(100, 100);

            var rectangle = _layouter.PutNextRectangle(size);

            rectangle.Size.Should().Be(size);
        }

        [Test]
        public void PutNextRectangle_ShouldReturnNotIntersectedRectangles_WhenCalledTwice()
        {
            var size = new Size(100, 100);

            var firstRectangle = _layouter.PutNextRectangle(size);
            var secondRectangle = _layouter.PutNextRectangle(size);

            firstRectangle.IntersectsWith(secondRectangle).Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_ShouldReturnDifferentSizeRectangles_WhenCalledWithDifferentSizes()
        {
            var firstSize = new Size(100, 100);
            var secondSize = new Size(50, 100);

            var firstRectangle = _layouter.PutNextRectangle(firstSize);
            var secondRectangle = _layouter.PutNextRectangle(secondSize);

            firstRectangle.Size.Should().NotBe(secondRectangle.Size);
        }

        [TestCase(1, 10, 5)]
        public void PutNextRectangle_ShouldReturnRectangles_InCircle(
            int sideLength,
            int rectanglesCount,
            double boundingRadius)
        {
            var size = new Size(sideLength, sideLength);

            var rectangles = GenerateRectangles(rectanglesCount, size);

            rectangles.Should().OnlyContain(rect => AreAllBoundsInCircle(rect, boundingRadius));
        }

        [TestCase(50, 0.7)]
        [TestCase(100, 0.75)]
        [TestCase(1000, 0.85, Ignore = "Take too much time")]
        public void PutNextRectangle_ShouldReturnRectangles_WithBigDensity_WhenSameSize(
            int rectanglesCount,
            double expectedRatio)
        {
            Size SizeFactory()
            {
                return new Size(10, 10);
            }

            var rectanglesToCircleRatio = CalculateDensityForRectangles(rectanglesCount, SizeFactory);

            rectanglesToCircleRatio.Should().BeGreaterOrEqualTo(expectedRatio);
        }

        [TestCase(50, 0.5)]
        [TestCase(100, 0.5)]
        [TestCase(1000, 0.5, Ignore = "Take too much time")]
        public void PutNextRectangle_ShouldReturnRectangles_WithBigDensity_WhenRandomSize(
            int rectanglesCount,
            double expectedRatio)
        {
            var rnd = new Random();

            Size SizeFactory()
            {
                return new Size(rnd.Next(1, 100), rnd.Next(1, 100));
            }

            var rectanglesToCircleRatio = CalculateDensityForRectangles(rectanglesCount, SizeFactory);

            rectanglesToCircleRatio.Should().BeGreaterOrEqualTo(expectedRatio);
        }

        private double CalculateDensityForRectangles(int count, Func<Size> sizeFactory)
        {
            var rectangles = GenerateRectangles(count, sizeFactory());
            var circleRadius = GetBoundingCircleRadius(rectangles);
            var rectanglesSquare = rectangles.Sum(rect => rect.CalculateSquare());
            var circleSquare = SquareCalculator.CalculateCircleSquare(circleRadius);
            return rectanglesSquare / circleSquare;
        }

        private Rectangle[] GenerateRectangles(int count, Size size)
        {
            return Enumerable.Range(0, count)
                .Select(_ => _layouter.PutNextRectangle(size))
                .ToArray();
        }

        private double GetBoundingCircleRadius(IEnumerable<Rectangle> rectangles)
        {
            var finder = new DistantPointFinder(_center);
            var rectanglesPoints = rectangles.SelectMany(rect => rect.GetBounds());
            return finder.GetDistantPoint(rectanglesPoints).DistanceTo(_center);
        }

        private bool AreAllBoundsInCircle(Rectangle rectangle, double radius)
        {
            return rectangle.GetBounds().All(bound => bound.DistanceTo(_center) <= radius);
        }
    }
}