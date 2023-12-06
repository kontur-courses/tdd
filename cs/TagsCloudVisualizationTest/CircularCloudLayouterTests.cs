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
            layouter = new CircularCloudLayouter(new Point(400, 400));
        }

        [Test]
        public static void CircularCloudLayouterCtor_WhenPassValidArguments_DoesNotThrowException() =>
            Assert.DoesNotThrow(() => new CircularCloudLayouter(new Point(200, 200)));

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
        public void WhenPutNewRectangles_TheyShouldBeTightlyPositioned()
        {
            var currentRectangles = new List<Rectangle>();

            for (var i = 0; i < 15; i++)
                currentRectangles.Add(layouter.PutNextRectangle(new Size(4, 2)));

            var rectanglesSquare = 0;
            var radius = 0.0;

            foreach (var rectangle in currentRectangles)
            {
                var center = new Point(400, 400);

                rectanglesSquare += rectangle.Size.Height * rectangle.Size.Width;

                var biggestDistance = Math.Abs(Math.Sqrt(Math.Pow(rectangle.X - center.X, 2)
                                                         + Math.Pow(rectangle.Y - center.Y, 2)));
                if (biggestDistance > radius)
                    radius = biggestDistance;
            }

            var circleSquare = Math.PI * radius * radius;

            var percentOfRatio = rectanglesSquare / circleSquare * 100;

            percentOfRatio.Should().BeGreaterThan(90);
        }
    }
}