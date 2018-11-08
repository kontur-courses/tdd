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

            layouter = new CircularCloudLayouter(new Point(0, 0));

            rectangles = new List<Rectangle>();
        }

        [TearDown]
        public void TearDown()
        {
            var testResult = TestContext.CurrentContext.Result.Outcome;

            if (Equals(testResult, ResultState.Failure))
            {
                CircularCloudVisualizer.DrawTags(rectangles, subTestFolder, "screenshot", 2048, 1080);

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

        [TestCase(1, TestName = "one rectangle")]
        [TestCase(2, TestName = "two rectangles")]
        [TestCase(10, TestName = "ten rectangles")]
        [TestCase(50, TestName = "fifty rectangles")]
        [TestCase(100, TestName = "one hundred rectangles")]
        [TestCase(1000, TestName = "one thousand rectangles")]
        public void PutNextRectangle_SeveralRectangles_NotIntersect(int rectanglesCount)
        {
            var rectanglesSizes = GenerateRectanglesSizes(rectanglesCount);

            foreach (var rs in rectanglesSizes)
                rectangles.Add(layouter.PutNextRectangle(rs));

            rectangles.Any(rect => rectangles.Where(r => !r.Equals(rect))
                                             .Any(r => r.IntersectsWith(rect)))
                      .Should().BeFalse();
        }

        public IEnumerable<Size> GenerateRectanglesSizes(int count)
        {
            var random = new Random();
            var rectangles = new List<Size>();

            for (var i = 0; i < count; i++)
                rectangles.Add(new Size(random.Next(5, 50), random.Next(5, 50)));

            return rectangles;
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
    }
}
