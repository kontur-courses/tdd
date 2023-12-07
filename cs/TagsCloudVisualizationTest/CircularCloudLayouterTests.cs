using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using TagsCloudVisualization;
using FluentAssertions;

namespace TagsCloudVisualizationTest
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter layouter;

        [SetUp]
        public void Initial()
        {
            layouter = new CircularCloudLayouter(new Point(400, 400), new Spiral());
        }

        [Test]
        public static void CircularCloudLayouterCtor_WhenPassValidArguments_DoesNotThrowException() =>
            Assert.DoesNotThrow(() => new CircularCloudLayouter(new Point(200, 200), new Spiral()));

        public static TestCaseData[] InvalidArguments =
        {
            new TestCaseData(-200, 200).SetName("PassNegativeWidth"),
            new TestCaseData(200, -200).SetName("PassNegativeHeight"),
            new TestCaseData(-200, -200).SetName("PassNegativeArguments"),
            new TestCaseData(0, 0).SetName("PassOnlyZero")
        };

        [TestOf(nameof(CircularCloudLayouter.PutNextRectangle))]
        [TestCaseSource(nameof(InvalidArguments))]
        public void WhenPassInvalidArguments_ShouldThrowArgumentException(int width, int height) =>
            Assert.Throws<ArgumentException>(() => layouter.PutNextRectangle(new Size(width, height)));

        [Test]
        public void WhenPutNewRectangle_ShouldBeAddedToList()
        {
            var currentRectangles = new List<Rectangle>
            {
                layouter.PutNextRectangle(new Size(4, 2))
            };

            currentRectangles.Count.Should().Be(1);
        }

        public static TestCaseData[] RectanglesPosition =
        {
            new TestCaseData(new Size(4, 2), new Size(4, 2))
                .Returns(false).SetName("WhenPassIdenticalRectangles"),
            new TestCaseData(new Size(6, 2), new Size(4, 2))
                .Returns(false).SetName("WhenPassRectanglesOfDifferentSizes")
        };

        [TestOf(nameof(CircularCloudLayouter.PutNextRectangle))]
        [TestCaseSource(nameof(RectanglesPosition))]
        public bool WhenPassSeveralRectangles_ShouldReturnCorrectIntersectionResult(Size rectangleSize,
            Size newRectangleSize)
        {
            var currentRectangles = new List<Rectangle>
            {
                layouter.PutNextRectangle(rectangleSize),
                layouter.PutNextRectangle(newRectangleSize)
            };

            return currentRectangles.First().IntersectsWith(currentRectangles.Last());
        }

        [Test]
        [TestOf(nameof(CircularCloudLayouter.PutNextRectangle))]
        public void WhenPassFirstPoint_ShouldBeInCenter()
        {
            layouter = new CircularCloudLayouter(new Point(400, 400), new Spiral());

            var currentRectangles = new List<Rectangle>()
            {
                layouter.PutNextRectangle(new Size(4,2))
            };

            currentRectangles.First().Location.Should().Be(new Point(398, 399));
        }
    }
}