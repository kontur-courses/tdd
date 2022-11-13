using System;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        const int Width = 800;
        const int Height = 600;
        const string FailureDirectoryName = "Failure";

        private Point center;
        private CircularCloudLayouter cloud;

        private Size minSize = new Size(30, 30);
        private Size maxSize = new Size(50, 50);

        [SetUp]
        public void SetUp()
        {
            center = new Point(Width / 2, Height / 2);
            var spiral = new ArchimedeanSpiral(center);
            cloud = new CircularCloudLayouter(spiral);
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome == ResultState.Failure)
            {
                var projectPath = Utilities.GetProjectPath();
                Directory.CreateDirectory(Path.Combine(projectPath, FailureDirectoryName));

                var imageCreator = new ImageCreator(Width, Height);
                imageCreator.Graphics.DrawRectangles(Pens.Black, cloud.Rectangles.ToArray());

                var filename = $"{TestContext.CurrentContext.Test.MethodName}.png";
                var fullPath = Path.Combine(projectPath, FailureDirectoryName, filename);

                imageCreator.Save(fullPath);
                Console.WriteLine($"Tag cloud visualization saved to file<{fullPath}>");
            }
        }

        [Test]
        public void CircularCloudLayouter_DoesNotThrowException()
        {
            var center = new Point(0, 0);
            var spiral = new ArchimedeanSpiral(center);

            Action action = () => new CircularCloudLayouter(spiral);

            action.Should().NotThrow();
        }

        [TestCase(-1, -1, TestName = "Negatvie size")]
        [TestCase(-1, 0, TestName = "Negatvie width")]
        [TestCase(0, -1, TestName = "Negatvie height")]
        public void PutNextRectangle_ThrowException_On(int width, int height)
        {
            Action action = () => cloud.PutNextRectangle(new Size(width, height));
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void PutNextRectangle_ReturnsRectangleWithGivenSize()
        {
            var rectangleSize = new Size(minSize.Width, maxSize.Height);
            var rectangle = cloud.PutNextRectangle(rectangleSize);
            rectangle.Size.Should().Be(rectangleSize);
        }

        [Test]
        public void PutNextRectangle_ShouldAddRectangleWithGivenSize()
        {
            var rectangleSize = new Size(minSize.Width, maxSize.Height);
            cloud.PutNextRectangle(rectangleSize);
            cloud.Rectangles.Last().Size.Should().Be(rectangleSize);
        }

        [TestCase(1, 0, 1, TestName = "One rectangle to empty cloud")]
        [TestCase(1, 1, 2, TestName = "One rectangle to non-empty cloud ")]
        [TestCase(2, 0, 2, TestName = "Some rectangles to empty cloud")]
        [TestCase(2, 2, 4, TestName = "Some rectangles to non-empty cloud ")]
        public void PutNextRectangle_ShouldAddRectangle(
            int count, int alreadyExist, int expectedExist)
        {
            Utilities.GenerateRectangleSize(alreadyExist, minSize, maxSize)
                .Select(rectSize => cloud.PutNextRectangle(rectSize))
                .ToArray();
            cloud.Rectangles.Should().HaveCount(alreadyExist);

            Utilities.GenerateRectangleSize(count, minSize, maxSize)
                .Select(rectSize => cloud.PutNextRectangle(rectSize))
                .ToArray();
            cloud.Rectangles.Should().HaveCount(expectedExist);
        }

        [TestCase(0, TestName = "Empty cloud")]
        [TestCase(1, TestName = "Cloud exist one rectangles")]
        [TestCase(100, TestName = "Cloud exist many rectangles")]
        public void PutNextRectangle_ShouldNotIntersectWithOtherRectangles(int alreadyExist)
        {
            Utilities.GenerateRectangleSize(alreadyExist, minSize, maxSize)
                .Select(rectSize => cloud.PutNextRectangle(rectSize))
                .ToArray();

            var rectangle = cloud.PutNextRectangle(new Size(minSize.Width, maxSize.Height));
            cloud.Rectangles.Take(alreadyExist)
                .Any(rect => rect.IntersectsWith(rectangle))
                .Should().BeFalse();
        }

        [TestCase(0, TestName = "Zero rectangles", ExpectedResult = false)]
        [TestCase(2, TestName = "Two rectangles", ExpectedResult = false)]
        [TestCase(100, TestName = "Many rectangles", ExpectedResult = false)]
        public bool CircularCloudLayouter_WithSomeAmountRectangles_ShouldNotIntersectWithEachOther(
            int alreadyExist)
        {
            Utilities.GenerateRectangleSize(alreadyExist, minSize, maxSize)
                .Select(rectSize => cloud.PutNextRectangle(rectSize))
                .ToArray();

            for (var i = 0; i < cloud.Rectangles.Count; i++)
            for (var j = i + 1; j < cloud.Rectangles.Count; j++)
                if (cloud.Rectangles[i].IntersectsWith(cloud.Rectangles[j]))
                    return true;

            return false;
        }
    }
}