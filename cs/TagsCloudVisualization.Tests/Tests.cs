using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.CloudLayouters;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Tests.Extensions;

namespace TagsCloudVisualization.Tests
{
    public class Tests
    {
        private const double Precision = 0.7072; // sqrt(2)/2.
        private static readonly Point origin = Point.Empty;
        private static readonly Random random = new Random();

        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void CircularCloudLayouterConstructor_GetCenterPoint() =>
            Assert.DoesNotThrow(() => new CircularCloudLayouter(origin));

        [Test]
        public void PutNextRectangle_OnZeroSize_ThrowArgumentException() =>
            Assert.Throws<ArgumentException>(
                () => new CircularCloudLayouter(origin).PutNextRectangle(new Size(0, 0)));

        [TestCase(12, 8, TestName = "WhenEvenWidthAndHeight")]
        [TestCase(100, 5555, TestName = "WhenEvenWidthAndOddHeight")]
        [TestCase(1, 1, TestName = "WhenOddWidthAndHeight")]
        public void PutNextRectangle_OnFirstSize_ReturnsRectangleWithCenterInTheOrigin(int width, int height)
        {
            var firstRectangle = new CircularCloudLayouter(origin).PutNextRectangle(new Size(width, height));

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
            var circularCloudLayouter = new CircularCloudLayouter(origin);
            var firstRectangle = circularCloudLayouter.PutNextRectangle(new Size(10, 5));
            var secondRectangle = circularCloudLayouter.PutNextRectangle(new Size(7, 3));

            firstRectangle.IntersectsWith(secondRectangle).Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_OnALotOfCalls_ReturnsNotIntersectedRectangles()
        {
            var circularCloudLayouter = new CircularCloudLayouter(origin);
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
            var firstRectangle = new CircularCloudLayouter(origin).PutNextRectangle(specifiedSize);

            firstRectangle.Size.Should().Be(specifiedSize);
        }

        [Test]
        public void PutNextRectangle_OnALotOfCalls_ReturnsRectanglesWithSpecifiedSizes()
        {
            var circularCloudLayouter = new CircularCloudLayouter(origin);

            var inputSizes = Enumerable.Range(0, 500).Select(i => new Size(random.Next(1, 500), random.Next(1, 500)))
                                       .ToArray();
            var rectangles = inputSizes.Select(size => circularCloudLayouter.PutNextRectangle(size));

            rectangles.Select(rectangle => rectangle.Size).Should().Equal(inputSizes);
        }

        [Test]
        public void SequenceShuffle_OnInputSequence_ReturnsSequenceWithSameItems()
        {
            var sequence = random.GetRandomSequence();

            sequence.SequenceShuffle(random).Should().BeEquivalentTo(sequence);
        }

        [Test]
        public void SequenceShuffle_OnInputSequence_ReturnsSequenceWithDifferentOrder()
        {
            var sequence = random.GetRandomSequence();

            sequence.SequenceShuffle(random).Should().NotEqual(sequence);
        }

        [Test]
        public void CreateMovedCopy_ReturnsNewRectangle()
        {
            var rectangle = Rectangle.Empty;

            rectangle.CreateMovedCopy(Size.Empty).Should().NotBeSameAs(rectangle);
        }

        [Test]
        public void CreateMovedCopy_ReturnsRectangleMovedOnSpecifiedOffset()
        {
            var rectangle = Rectangle.Empty;
            var offset = new Size(10, 10);

            rectangle.CreateMovedCopy(offset).X.Should().Be(rectangle.X + offset.Width);
            rectangle.CreateMovedCopy(offset).Y.Should().Be(rectangle.Y + offset.Height);
        }

        [Test]
        [Category("SupportTests")]
        public void GetRectangleCenter_RandomRectangle(
            [Random(0, 1000, 1)] int xLocation, [Random(0, 1000, 1)] int yLocation,
            [Random(1, 1000, 1)] int width, [Random(1, 1000, 1)] int height)
        {
            var location = new Point(xLocation, yLocation);
            var rectangle = new Rectangle(location, new Size(width, height));
            var center = rectangle.GetRectangleCenter();

            rectangle.CheckIfPointIsCenterOfRectangle(center, Precision).Should().BeTrue();
        }

        [Category("SupportTests")]
        [TestCase(0, 0, TestName = "WithOriginAsCenter")]
        [TestCase(2, -3, TestName = "WithOddCenterCoordinates")]
        public void GetRectangleWithCenterInThePoint(int xCenter, int yCenter)
        {
            var center = new Point(xCenter, yCenter);
            var size = new Size(2, 3);
            var centeredRectangle = center.GetRectangleWithCenterInThePoint(size);

            centeredRectangle.CheckIfPointIsCenterOfRectangle(center, Precision).Should().BeTrue();
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