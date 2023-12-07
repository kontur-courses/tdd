using NUnit.Framework;
using System.Drawing;
using TagsCloudVisualization;
using FluentAssertions;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization.Utils;

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

        [TearDown]
        public void SaveLayoutImage_WhenFailed()
        {
            var testResult = TestContext.CurrentContext.Result.Outcome;
            var testName = TestContext.CurrentContext.Test.FullName;

            if (Equals(testResult, ResultState.Failure)
                || Equals(testResult == ResultState.Error))
            {
                var cloud = circularLayouter.GetCloud();
                var bitmap = CloudDrawer.Draw(cloud, 1000, 1000);
                var path = @$"{Environment.CurrentDirectory}..\..\..\..\FailedTestsLayouts\{testName}.jpg";
                bitmap.Save(path);
                Console.WriteLine($"Tag cloud visualization saved to file {path}");
            }
        }


        private static IEnumerable<TestCaseData> PutNextRectangleArgumentException => new[]
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

        [Test]
        public void PutNextRectangle_ShouldPutRectangleWithCenterInTheCloudCenter()
        {
            var center = circularLayouter.GetCloud().Center;

            var firstRectangle = circularLayouter.PutNextRectangle(new Size(10, 10));
            var firstRectnagleCenter = Utils.GetRectangleCenter(firstRectangle);

            firstRectnagleCenter.Should().Be(center);
        }

        private static IEnumerable<TestCaseData> PutRectanglesArgumnetException = new[]
        {
            new TestCaseData(new List<Size>()).SetName("WhenGivenEmptyList"),
        };

        [TestCaseSource(nameof(PutRectanglesArgumnetException))]
        public void PutRectangles_ThrowsArgumnetNullException(List<Size> sizes) =>
            Assert.Throws<ArgumentException>(() => circularLayouter.PutRectangles(sizes));


        [Test]
        public void GetLayout_ReturnsAsMuchRectanglesAsWasPut()
        {
            circularLayouter.PutNextRectangle(new Size(10, 10));
            circularLayouter.PutNextRectangle(new Size(5, 5));
            var rectanglesAmount = circularLayouter.GetCloud().Rectangles.Count;

            rectanglesAmount.Should().Be(2);
        }

        [Test]
        public void Layout_ShouldContainRectanglesWhichDoNotIntersectWithEachOther()
        {
            var rectanglesAmount = 20;

            var rectanglesLayout = GetRectanglesLayout(rectanglesAmount);

            for (var i = 0; i < rectanglesAmount; i++)
            {
                for (var j = 0; j < rectanglesAmount; j++)
                {
                    if (i == j) continue;
                    var doesRectanglesIntersect = rectanglesLayout[i].IntersectsWith(rectanglesLayout[j]);
                    doesRectanglesIntersect.Should().BeFalse();
                }
            }
        }

        [Test]
        public void Layout_ShouldContainTightPlacedRectangles()
        {
            var rectanglesLayout = GetRectanglesLayout(100);
            var rectanglesSpace = rectanglesLayout.Sum(rect => rect.Width * rect.Height);
            var circleRadiusOfHorizontalAxe =
                (rectanglesLayout.Max(rect => rect.X) - rectanglesLayout.Min(rect => rect.X)) / 2;
            var circleRadiusOfVerticleAxe =
                (rectanglesLayout.Max(rect => rect.Y) - rectanglesLayout.Min(rect => rect.Y)) / 2;
            var circleOccupiedSpace = Math.PI * circleRadiusOfHorizontalAxe * circleRadiusOfVerticleAxe;
            var freeSpace = circleOccupiedSpace - rectanglesSpace;
            var freeSpaceProportionInPercentage = freeSpace / circleOccupiedSpace * 100;

            freeSpaceProportionInPercentage.Should().BeLessThan(30);
        }

        private List<Rectangle> GetRectanglesLayout(int rectanglesAmount)
        {
            var sizes = Utils.GenerateSizes(rectanglesAmount);
            circularLayouter.PutRectangles(sizes);
            return circularLayouter.GetCloud().Rectangles;
        }
    }
}