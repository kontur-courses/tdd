using System;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using TagsCloudVisualization;
using FluentAssertions;
using NUnit.Framework.Interfaces;
using TestContext = NUnit.Framework.TestContext;
using System.IO;
using System.Reflection;

namespace TagsCloudVisualizationTest
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter layouter;
        private static readonly Point Center = new Point(500, 500);
        private RectangleF[] currentRectangles;

        [SetUp]
        public void Initial()
        {
            layouter = new CircularCloudLayouter(Center, new Spiral());
        }

        [TearDown]
        public void TearDown()
        {
            var cloudCreator = new TagCloudCreator();
          
            if (currentRectangles == null)
                return;

            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var imageName = TestContext.CurrentContext.Test.Name;
                cloudCreator.CreateCloud(currentRectangles, imageName);

                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
                Console.WriteLine($"Tag cloud visualization saved to {path}\\{imageName}");
            }
        }

        [Test]
        public static void CircularCloudLayouterCtor_WhenPassValidArguments_DoesNotThrowException() =>
            Assert.DoesNotThrow(() => new CircularCloudLayouter(Center, new Spiral()));

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
            currentRectangles = new RectangleF[]
            {
                layouter.PutNextRectangle(new Size(40, 20))
            };

            currentRectangles.Length.Should().Be(1);
        }

        public static TestCaseData[] RectanglesPosition =
        {
            new TestCaseData(new Size(40, 20), new Size(40, 20))
                .Returns(false).SetName("WhenPassIdenticalRectangles"),
            new TestCaseData(new Size(60, 20), new Size(40, 20))
                .Returns(false).SetName("WhenPassRectanglesOfDifferentSizes")
        };

        [TestOf(nameof(CircularCloudLayouter.PutNextRectangle))]
        [TestCaseSource(nameof(RectanglesPosition))]
        public bool WhenPassSeveralRectangles_ShouldReturnCorrectIntersectionResult(Size rectangleSize,
            Size newRectangleSize)
        {
            currentRectangles = new RectangleF[]
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
            currentRectangles = new RectangleF[]
            {
                layouter.PutNextRectangle(new Size(40,20))
            };

            currentRectangles.First().Location.X.Should().Be(480);
            currentRectangles.First().Location.Y.Should().Be(490);
        }
    }
}