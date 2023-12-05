using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using TagsCloudVisualization;
using FluentAssertions;

namespace TagsCloudVizualizationTest
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter layouter;

        [SetUp]
        public void Initial() =>
            layouter = new CircularCloudLayouter(new Point(400, 400));

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

        [TestOf(nameof(CircularCloudLayouter.HaveIntersection))]
        [TestCaseSource(nameof(InvalidArguments))]
        public void WhenPassInvalidArguments_ShouldThrowArgumentException(int width, int height) =>
            Assert.Throws<ArgumentException>(() => layouter.PutNextRectangle(new Size(width, height)));

        [Test]
        public void WhenPutNewRectangle_ShouldBeAddedToList()
        {
            layouter.PutNextRectangle(new Size(4, 2));
            
            layouter.Rectangles.Count.Should().Be(1);
        }

        private static readonly List<Rectangle> Rectangles = new List<Rectangle>
        {
            new Rectangle(new Point(400, 400), new Size(4, 2))
        };

        public static TestCaseData[] RectanglesPosition =
        {
            new TestCaseData(new Rectangle(new Point(400, 402), new Size(4, 2)))
                .Returns(false).SetName("NotIntersectedRectangles2"),
            new TestCaseData(new Rectangle(new Point(400, 402), new Size(6, 2)))
                .Returns(false).SetName("NotIntersectedRectanglesOfDifferentSizes"),

            new TestCaseData(new Rectangle(new Point(400, 400), new Size(4, 2)))
                .Returns(true).SetName("IntersectedRectanglesInOnePoint"),
            new TestCaseData(new Rectangle(new Point(402, 400), new Size(4, 2)))
                .Returns(true).SetName("IntersectedRectangles")
        };

        [TestOf(nameof(CircularCloudLayouter.HaveIntersection))]
        [TestCaseSource(nameof(RectanglesPosition))]
        public bool WhenPassSeveralRectangles_ShouldReturnCorrectIntersectionResult(Rectangle newRectangle)
        {
            layouter.Rectangles = Rectangles;

            return layouter.HaveIntersection(newRectangle);
        }

        [Test]
        public void WhenPutNewRectangles_TheyShouldBeTightlyPositioned()
        {
            for (var i = 0; i < 15; i++)
                layouter.PutNextRectangle(new Size(4, 2));

            var rectanglesSquare = 0;
            var radius = 0.0;

            foreach (var rectangle in layouter.Rectangles)
            {
                var center = layouter.Center;

                rectanglesSquare += rectangle.Size.Height * rectangle.Size.Width;

                var biggestDistance= Math.Abs(Math.Sqrt(Math.Pow(rectangle.X - center.X, 2)
                                                         + Math.Pow(rectangle.Y - center.Y, 2)));
                if (biggestDistance > radius)
                    radius = biggestDistance;
            }

            var circleSquare = Math.PI * radius * radius;

            var percentOfRatio = (rectanglesSquare / circleSquare) * 100;

            percentOfRatio.Should().BeGreaterThan(80);
        }
    }
}
