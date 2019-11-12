using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization.CloudLayouters;
using TagsCloudVisualization.Tests.Extensions;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private const double Precision = 0.7072; // sqrt(2)/2.
        private const string FailedTestsDirectoryName = "FailedVisualizationTests";
        private static readonly Point origin = Point.Empty;

        private CircularCloudLayouter circularCloudLayouter;
        private VisualizationContext visualizationContext;

        [SetUp]
        public void SetUp()
        {
            circularCloudLayouter = new CircularCloudLayouter(origin);
            visualizationContext = new VisualizationContext();
        }

        [TearDown]
        public void TearDown()
        {
            if (!(TestContext.CurrentContext.Result.Outcome.Status is TestStatus.Failed) ||
                visualizationContext.IsEmpty)
                return;

            var failedTestFilename = $"{GetCurrentTestName()}_{DateTime.Now:dd.MM.yyyy-HH.mm.ss}.png";

            Directory.CreateDirectory(FailedTestsDirectoryName);
            var wrongVisualisationImageFilepath = Path.Combine(TestContext.CurrentContext.TestDirectory,
                                                               FailedTestsDirectoryName,
                                                               failedTestFilename);

            using var image = visualizationContext.GetTestVisualization();
            image.Save(wrongVisualisationImageFilepath);

            TestContext.WriteLine($@"Tag cloud visualization saved to file:{Environment.NewLine
                                      }{wrongVisualisationImageFilepath}");

            string GetCurrentTestName() => TestContext.CurrentContext.Test is var test &&
                                           test.MethodName == test.Name
                                               ? test.Name
                                               : test.MethodName + test.Name;
        }

        [Test]
        public void AlwaysFailedTest()
        {
            var randomizer = TestContext.CurrentContext.Random;

            var sut = new CircularCloudLayouter(new Point(500, 500));
            var rectangles = Enumerable.Range(0, 50)
                                       .Select(i => sut.PutNextRectangle(
                                                   new Size(randomizer.Next(50, 100), randomizer.Next(50, 100))))
                                       .Append(new Rectangle(530, 550, 100, 45))
                                       .ToArray();

            visualizationContext.AllRectangles.UnionWith(rectangles);
            visualizationContext.WrongRectangles.UnionWith(new[]
            {
                GetAnyPairOfIntersectingRectangles(rectangles).Value.Item1,
                GetAnyPairOfIntersectingRectangles(rectangles).Value.Item2
            });

            Assert.Fail("Always fail");
        }

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
            var randomizer = TestContext.CurrentContext.Random;

            var rectangles = Enumerable.Range(0, 500)
                                       .Select(i => circularCloudLayouter.PutNextRectangle(
                                                   new Size(randomizer.Next(1, 500), randomizer.Next(1, 500))))
                                       .ToArray();

            GetAnyPairOfIntersectingRectangles(rectangles).Should().BeNull();
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
            var randomizer = TestContext.CurrentContext.Random;

            var inputSizes = Enumerable.Range(0, 500)
                                       .Select(i => new Size(randomizer.Next(1, 500), randomizer.Next(1, 500)))
                                       .ToArray();
            var rectangles = inputSizes.Select(size => circularCloudLayouter.PutNextRectangle(size));

            rectangles.Select(rectangle => rectangle.Size).Should().Equal(inputSizes);
        }

        private static (Rectangle, Rectangle)? GetAnyPairOfIntersectingRectangles(Rectangle[] rectangles)
        {
            for (int i = 0; i < rectangles.Length; i++)
                for (int j = i + 1; j < rectangles.Length; j++)
                    if (rectangles[i].IntersectsWith(rectangles[j]))
                        return (rectangles[i], rectangles[j]);
            return null;
        }
    }
}