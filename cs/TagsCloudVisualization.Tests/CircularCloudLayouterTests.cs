using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    [TestFixture]
    class CircularCloudLayouterTests
    {
        private string currentTestsFolder = TestContext.CurrentContext.TestDirectory;
        private DirectoryInfo visualizationsFolder;
        private DirectoryInfo subTestFolder;
        private CircularCloudLayouter layouter;
        private List<Rectangle> rectangles;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            visualizationsFolder = Directory.CreateDirectory(Path.Combine(currentTestsFolder, "Visualizations"));
        }

        [SetUp]
        public void SetUp()
        {
            var currentTestName = TestContext.CurrentContext.Test.Name;

            subTestFolder = Directory.CreateDirectory(Path.Combine(visualizationsFolder.FullName, currentTestName));

            var rnd = new Random();
            layouter = new CircularCloudLayouter(new Point(rnd.Next(-100, 100), rnd.Next(-100, 100)));

            rectangles = new List<Rectangle>();
        }

        [TearDown]
        public void TearDown()
        {
            var testResult = TestContext.CurrentContext.Result.Outcome;

            if (Equals(testResult, ResultState.Failure))
            {
                TagCloudVisualizer.Visualize(rectangles, subTestFolder, "screenshot", 2048, 1080);

                var path = Path.Combine(subTestFolder.FullName, "screenshot");

                TestContext.Out.WriteLine("Tag cloud visualization saved to file " + path);
            }
        }

        [Test]
        public void Constructor_InitializeCenterPoint()
        {
            var center = new Point(15, 74);
            var layouter = new CircularCloudLayouter(center);

            layouter.Center.Should().BeEquivalentTo(center);
        }

        [TestCase(0, 0, TestName = "in the center of the coordinate system")]
        [TestCase(50, 0, TestName = "on axis OX")]
        [TestCase(-50, -50, TestName = "on third part of the coordinate system")]
        public void PutNextRectangle_FirstRectangle_PutToCenter(int x, int y)
        {
            var layouter = new CircularCloudLayouter(new Point(x, y));

            var r = layouter.PutNextRectangle(new Size(100, 50));
            rectangles.Add(r);

            r.Should().BeEquivalentTo(new Rectangle(new Point(x, y), new Size(100, 50)));
        }

        [TestCase(0, 0, TestName = "with zero width and height")]
        [TestCase(0, 50, TestName = "with zero width")]
        [TestCase(50, 0, TestName = "with zero height")]
        [TestCase(-50, -50, TestName = "with negative width and height")]
        [TestCase(-50, 50, TestName = "with negative width")]
        [TestCase(50, -50, TestName = "with negative height")]
        public void PutNextRectangle_InvalidRectangleSize_ThrowException(int width, int height)
        {
            var rectangleSize = new Size(width, height);
            Action act = () => layouter.PutNextRectangle(rectangleSize);

            act.Should().Throw<ArgumentException>();
        }

        [TestCase(100, 100, TestName = "square")]
        [TestCase(500, 200, TestName = "rectangle")]
        public void PutNextRectangle_ValidRectangleSize_PutSuccessfully(int width, int height)
        {
            var rectangleSize = new Size(width, height);

            layouter.PutNextRectangle(rectangleSize).Should().BeOfType<Rectangle>();
        }

        [TestCase(1, TestName = "one rectangle")]
        [TestCase(2, TestName = "two rectangles")]
        [TestCase(10, TestName = "ten rectangles")]
        [TestCase(50, TestName = "fifty rectangles")]
        [TestCase(100, TestName = "one hundred rectangles")]
        [TestCase(1000, TestName = "one thousand rectangles")]
        public void PutNextRectangle_SeveralRectangles_NotIntersect(int rectanglesCount)
        {
            PlaceRectangles(rectanglesCount);

            rectangles.Any(rect => rectangles.Where(r => !r.Equals(rect))
                                             .Any(r => r.IntersectsWith(rect)))
                      .Should().BeFalse();
        }

        [TestCase(1, TestName = "one rectangle")]
        [TestCase(2, TestName = "two rectangles")]
        [TestCase(10, TestName = "ten rectangles")]
        [TestCase(50, TestName = "fifty rectangles")]
        [TestCase(100, TestName = "one hundred rectangles")]
        [TestCase(1000, TestName = "one thousand rectangles")]
        public void PutNextRectangle_PlacedRectangles_TightlyPlaced(int rectanglesCount)
        {
            PlaceRectangles(rectanglesCount);

            var radius = GetDistanceToNearestEdge(rectangles, layouter.Center);

            var circleArea = Math.PI * Math.Pow(radius, 2);

            var rectanglesArea = rectangles.Select(r => r.Width * r.Height)
                                           .Sum();

            const double densityCoefficient = 0.75;

            var density = rectanglesArea / circleArea;

            density
                .Should()
                .BeGreaterThan(densityCoefficient);
        }

        public double GetDistanceFromOriginTo(Point p) => Math.Sqrt(Math.Pow(p.X, 2) + Math.Pow(p.Y, 2));

        public void PlaceRectangles(int rectanglesCount)
        {
            var rectanglesSizes = TagsGenerator.GenerateRectanglesSizes(rectanglesCount);

            foreach (var rs in rectanglesSizes)
                rectangles.Add(layouter.PutNextRectangle(rs));
        }

        public static int GetDistanceToNearestEdge(IEnumerable<Rectangle> placedRectangles, Point center)
        {
            var mostDistanceFromOriginByOrdinate = 0;
            var mostDistanceFromOriginByAbscissa = 0;

            foreach (var r in placedRectangles)
            {
                var distanceByOrdinate = Math.Abs(Math.Abs(r.Location.Y) - Math.Abs(center.Y));
                var distanceByAbscissa = Math.Abs(Math.Abs(r.Location.X) - Math.Abs(center.X));

                mostDistanceFromOriginByOrdinate = Math.Max(mostDistanceFromOriginByOrdinate, distanceByOrdinate);
                mostDistanceFromOriginByAbscissa = Math.Max(mostDistanceFromOriginByAbscissa, distanceByAbscissa);
            }

            return Math.Min(mostDistanceFromOriginByOrdinate, mostDistanceFromOriginByAbscissa);
        }
    }
}