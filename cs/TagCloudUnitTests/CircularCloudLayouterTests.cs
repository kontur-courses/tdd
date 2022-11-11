using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagCloud;

namespace TagCloudUnitTests
{
    [TestFixture]
    internal class CircularCloudLayouterTests
    {
        private readonly Size cloudImageSize = new Size(750, 750);

        private CircularCloudLayouter layouter;

        private IReadOnlyList<Rectangle> layout;

        private DirectoryInfo testsLogDirectory;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            testsLogDirectory = GetTestsLogDirectory();
        }

        [SetUp]
        public void Setup()
        {
            layouter = new CircularCloudLayouter(new Point(cloudImageSize.Width / 2, cloudImageSize.Height / 2));

            layout = new List<Rectangle>();
        }

        [TearDown]
        public void TearDown()
        {
            if (IsTestInCategory("Layout") && IsTestResultStateFailureOrError())
            {
                GenerateAndSaveCloudImage();
            }
        }

        [Test, Category("Layout")]
        public void PutNextRectangle_PutsRectangleInTheCenter_WhenFirstRectangleAdded()
        {
            layout = GetLayout(new[] { new Size(100, 50) });

            layout.First().GetCenter().Should().BeEquivalentTo(layouter.CloudCenter);
        }

        [TestCase(-1, 1, TestName = "negative width")]
        [TestCase(1, -1, TestName = "negative height")]
        [TestCase(-1, -1, TestName = "negative width and height")]
        [TestCase(0, 1, TestName = "zero width")]
        [TestCase(1, 0, TestName = "zero height")]
        [TestCase(0, 0, TestName = "zero width and height")]
        public void PutNextRectangle_ThrowsArgumentException_WhenSizeIsInvalid(int width, int height)
        {
            Action action = () => layouter.PutNextRectangle(new Size(width, height));

            action.Should().Throw<ArgumentException>().WithMessage("width and height of rectangle must be more than zero");
        }

        [Test, Category("Layout")]
        public void PutNextRectangle_PutsTwoAlignedHorizontallyRectangles_WhenTwoAdded()
        {
            var sizes = new[]
            {
                new Size(100, 150),
                new Size(100, 150)
            };

            layout = GetLayout(sizes);

            var firstRectangle = layout[0];
            var secondRectangle = layout[1];

            Math.Abs(firstRectangle.Y).Should().Be(Math.Abs(secondRectangle.Y));
        }

        [Test, Category("Layout")]
        public void PutNextRectangle_PutsTwoAlignedVerticallyRectangles_WhenTwoAdded()
        {
            var sizes = new[]
            {
                new Size(150, 100),
                new Size(150, 100)
            };

            layout = GetLayout(sizes);

            var firstRectangle = layout[0];
            var secondRectangle = layout[1];

            Math.Abs(firstRectangle.X).Should().Be(Math.Abs(secondRectangle.X));
        }
        
        [TestCase(2, Category = "Layout")]
        [TestCase(10, Category = "Layout")]
        [TestCase(100, Category = "Layout")]
        [TestCase(1000, Category = "Layout")]
        public void PutNextRectangle_PutsNonIntersectingRectangles_WhenSeveralRectanglesAdded(int rectanglesCount)
        {
            var sizes = RectangleSizeGenerator.GetConstantSizes(rectanglesCount, new Size(100, 50));

            layout = GetLayout(sizes);

            IsAnyIntersection(layout).Should().Be(false);
        }

        private bool IsAnyIntersection(IReadOnlyList<Rectangle> cloudLayout)
        {
            foreach (var rectangle in cloudLayout)
                if (cloudLayout.Where(r => !r.Equals(rectangle)).Any(r => r.IntersectsWith(rectangle)))
                    return true;

            return false;
        }

        private IReadOnlyList<Rectangle> GetLayout(IReadOnlyList<Size> rectanglesSizes)
        {
            var rectangles = new List<Rectangle>();

            foreach (var size in rectanglesSizes)
                rectangles.Add(layouter.PutNextRectangle(size));

            return rectangles.AsReadOnly();
        }

        public bool IsTestInCategory(string categoryName)
        {
            return TestContext.CurrentContext.Test.Properties["Category"].Contains(categoryName);
        }

        public bool IsTestResultStateFailureOrError()
        {
            var resultState = TestContext.CurrentContext.Result.Outcome;

            return resultState == ResultState.Failure || resultState == ResultState.Error;
        }

        private void GenerateAndSaveCloudImage()
        {
            var imageCreator = new CloudImageGenerator(layouter, cloudImageSize, Color.Black);

            var image = imageCreator.Generate(layout);

            var subTestDirectoryInfo = GetSubTestDirectory();

            image.Save(Path.Combine(subTestDirectoryInfo.FullName, "CloudImage.png"));
        }

        private DirectoryInfo GetSubTestDirectory()
        {
            var currentTestName = TestContext.CurrentContext.Test.Name;

            return testsLogDirectory.CreateSubdirectory(currentTestName);
        }

        private static DirectoryInfo GetSolutionDirectory()
        {
            var currentDirectory = new DirectoryInfo(Environment.CurrentDirectory);

            while (currentDirectory != null && !currentDirectory.GetFiles("*.sln").Any())
            {
                currentDirectory = currentDirectory.Parent;
            }

            return currentDirectory;
        }

        private static DirectoryInfo GetTestsLogDirectory()
        {
            DirectoryInfo solutionDirectory = GetSolutionDirectory();

            if (solutionDirectory.GetDirectories("TestsLog").Any())
                return solutionDirectory.GetDirectories("TestsLog").First();

            return solutionDirectory.CreateSubdirectory("TestsLog");
        }
    }
}