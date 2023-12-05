using System;
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
            var positions = new List<Point>() //Arrange
            {
                new Point(400, 400),
                new Point(400, 402),
                new Point(399, 398),
                new Point(403, 398),
                new Point(404, 400),
                new Point(401, 404),
                new Point(396, 401),
                new Point(398, 396)
            };

            for (var i = 0; i < 8; i++)
                layouter.PutNextRectangle(new Size(4, 2)); //Act

            for (var i = 0; i < positions.Count; i++)
                layouter.Rectangles[i].Location.Should().Be(positions[i]); //Assert
        }
    }
}
