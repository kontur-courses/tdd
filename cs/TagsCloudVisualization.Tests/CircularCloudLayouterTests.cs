using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.CloudLayouters;
using TagsCloudVisualization.Tests.Extensions;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private const double Precision = 0.7072; // sqrt(2)/2.
        private static readonly Point origin = Point.Empty;
        private static readonly Random random = new Random();

        private CircularCloudLayouter circularCloudLayouter;

        [SetUp]
        public void SetUp() => circularCloudLayouter = new CircularCloudLayouter(origin);

        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void CircularCloudLayouterConstructor_GetCenterPoint() =>
            Assert.DoesNotThrow(() => new CircularCloudLayouter(new Point(10, 20)));

        [Test]
        public void PutNextRectangle_OnZeroSize_ThrowArgumentException() =>
            Assert.Throws<ArgumentException>(() => circularCloudLayouter.PutNextRectangle(new Size(0, 0)));

        [TestCase(12, 8, TestName = "WhenEvenWidthAndHeight")]
        [TestCase(100, 5555, TestName = "WhenEvenWidthAndOddHeight")]
        [TestCase(1, 1, TestName = "WhenOddWidthAndHeight")]
        public void PutNextRectangle_OnFirstSize_ReturnsRectangleWithCenterInTheOrigin(int width, int height)
        {
            var firstRectangle = circularCloudLayouter.PutNextRectangle(new Size(width, height));

            firstRectangle.CheckIfPointIsCenterOfRectangle(origin, Precision);
        }

        [TestCase(0, 0, TestName = "WhenOriginAsCenter")]
        [TestCase(11, 57, TestName = "WhenCenterWithDifferentCoordinates")]
        [TestCase(250, 250, TestName = "WhenCenterWithSameCoordinates")]
        public void PutNextRectangle_OnFirstSize_ReturnsRectangleWithCenterInSpecifiedPoint(int xCenter, int yCenter)
        {
            var center = new Point(xCenter, yCenter);
            var firstRectangle = new CircularCloudLayouter(center).PutNextRectangle(new Size(100, 50));

            firstRectangle.CheckIfPointIsCenterOfRectangle(center, Precision);
        }

        [Test]
        public void PutNextRectangle_OnSecondSize_ReturnsNotIntersectedRectangle()
        {
            var firstRectangle = circularCloudLayouter.PutNextRectangle(new Size(10, 5));
            var secondRectangle = circularCloudLayouter.PutNextRectangle(new Size(7, 3));

            firstRectangle.IntersectsWith(secondRectangle).Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_OnALotOfCalls_ReturnsNotIntersectedRectangles()
        {
            var rectangles = Enumerable.Range(0, 500)
                                       .Select(i => circularCloudLayouter.PutNextRectangle(
                                                   new Size(random.Next(1, 500), random.Next(1, 500))))
                                       .ToArray();

            CheckIfAnyIntersects(rectangles).Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_OnFirstSize_ReturnsRectangleWithSpecifiedSize([Random(1, 1000, 1)] int width,
                                                                                   [Random(1, 1000, 1)] int height)
        {
            var specifiedSize = new Size(width, height);
            var firstRectangle = circularCloudLayouter.PutNextRectangle(specifiedSize);

            firstRectangle.Size.Should().Be(specifiedSize);
        }

        [Test]
        public void PutNextRectangle_OnALotOfCalls_ReturnsRectanglesWithSpecifiedSizes()
        {
            var inputSizes = Enumerable.Range(0, 500).Select(i => new Size(random.Next(1, 500), random.Next(1, 500)))
                                       .ToArray();
            var rectangles = inputSizes.Select(size => circularCloudLayouter.PutNextRectangle(size));

            rectangles.Select(rectangle => rectangle.Size).Should().Equal(inputSizes);
        }

        private static bool CheckIfAnyIntersects(Rectangle[] rectangles)
        {
            for (int i = 0; i < rectangles.Length; i++)
                for (int j = i + 1; j < rectangles.Length; j++)
                    if (rectangles[i].IntersectsWith(rectangles[j]))
                        return true;
            return false;
        }
    }
}