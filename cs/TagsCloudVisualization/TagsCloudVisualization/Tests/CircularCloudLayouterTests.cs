using NUnit.Framework;
using System.Drawing;
using TagsCloudVisualization;
using FluentAssertions;

namespace Tests
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter circularLayouter;
        [SetUp]
        public void Setup()
        {
            circularLayouter = new CircularCloudLayouter(new Point(500, 500));
        }


        static IEnumerable<TestCaseData> PutNextRectangleArgumentException => new[]
        {
            new TestCaseData(new Size(0,1)).SetName("WhenGivenNotPositiveWidth"),
            new TestCaseData(new Size(1,0)).SetName("WhenGivenNotPositiveHeigth"),
            new TestCaseData(new Size(0,0)).SetName("WhenGivenNotPositiveHeigthAndWidth"),
        };

        [TestCaseSource(nameof(PutNextRectangleArgumentException))]
        public void PutNextRectangle_ShouldThrowArgumentException(Size rectangleSize) =>
            Assert.Throws<ArgumentException>(() => circularLayouter.PutNextRectangle(rectangleSize));

        [Test]
        public void PutNextRectangle_ShouldReturnRectangleOfTheSameSize()
        {
            var rectangleSizeToPut = new Size(10, 10);

            var resultRectangle = circularLayouter.PutNextRectangle(rectangleSizeToPut);

            resultRectangle.Size.Should().Be(rectangleSizeToPut);
        }

        [Test]
        public void PutNextRectangle_ShouldReturnRectangleThatDoesNotIntersectWithAlreadyPutOnes()
        {
            var firstPutRectangle = circularLayouter.PutNextRectangle(new Size(10, 10));
            var secondPutRectangle = circularLayouter.PutNextRectangle(new Size(5, 5));

            var doesRectanglesIntersect = firstPutRectangle.IntersectsWith(secondPutRectangle);

            doesRectanglesIntersect.Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_ShouldPutRectanglesAtDistanceLessThanOne()
        {
            var firstPutRectangle = circularLayouter.PutNextRectangle(new Size(10, 10));
            var secondPutRectangle = circularLayouter.PutNextRectangle(new Size(5, 5));

            var distanceBetweenRectangles = Utils.CalculateShortestDistance(firstPutRectangle, secondPutRectangle);

            distanceBetweenRectangles.Should().BeLessThan(1);
        }
    }
}