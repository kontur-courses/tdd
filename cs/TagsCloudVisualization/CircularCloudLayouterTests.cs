using System;
using System.Drawing;
using System.IO;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private Point Center { get; set; }
        private CircularCloudLayouter CloudLayouter { get; set; }

        [SetUp]
        public void SetUp()
        {
            Center = new Point(400, 400);
            CloudLayouter = new CircularCloudLayouter(Center);
        }

        [TearDown]
        public void TearDown()
        {
            var testStatus = TestContext.CurrentContext.Result.Outcome.Status;

            if (testStatus == TestStatus.Failed)
            {
                var path = CloudLayouter.GenerateNewFilePath();
                CloudLayouter.SaveDrawing(path);
                Console.WriteLine($"Tag cloud visualization saved to file {path}");
            }
        }

        [TestCase(0, 9, TestName = "Width is zero")]
        [TestCase(-1, 8, TestName = "Width is negative")]
        [TestCase(10, 0, TestName = "Height is zero")]
        [TestCase(3, -2, TestName = "Height is negative")]
        public void PutNextRectangle_ThrowException_When(int width, int height)
        {
            var size = new Size(width, height);
            Action putRectangle = () => CloudLayouter.PutNextRectangle(size);

            putRectangle.Should().Throw<ArgumentException>();
        }

        [Test]
        public void PutNextRectangle_ReturnExpectedSize_WhenSizeIsPositive()
        {
            var expectedSize = new Size(13, 11);
            var rectangle = CloudLayouter.PutNextRectangle(expectedSize);

            rectangle.Size.Should().Be(expectedSize);
        }

        [Test]
        public void PutNextRectangle_ReturnDifferentPositions_WhenCalledTwoTimes()
        {
            var firstRectangle = CloudLayouter.PutNextRectangle(new Size(5, 5));
            var secondRectangle = CloudLayouter.PutNextRectangle(new Size(10, 10));

            secondRectangle.Location.Should().NotBe(firstRectangle.Location);
        }

        [Test]
        public void PutNextRectangle_ReturnNotCollidedRectangles_WhenCalledTwoTimes()
        {
            var firstRectangle = CloudLayouter.PutNextRectangle(new Size(100, 100));
            var secondRectangle = CloudLayouter.PutNextRectangle(new Size(100, 100));

            secondRectangle.IntersectsWith(firstRectangle).Should().Be(false);
        }

        [Test]
        public void PutNextRectangle_ThrowException_WhenRectanglesDoesntFitOnImage()
        {
            Center = new Point(0, 0);
            var size = new Size(400, 200);

            Action putRectangle = () =>
            {
                for (int i = 0; i < 100; i++)
                {
                    CloudLayouter.PutNextRectangle(size);
                }
            };

            putRectangle.Should().Throw<ArgumentException>();
        }

        [TestCase(@"C:\image.gif", TestName = "Wrong image format")]
        [TestCase(@"S:Nonexistent\Dir\image.jpg", TestName = "Directory dont exist")]
        public void SaveDrawing_ThrowException_When(string path)
        {
            Action getPath = () => CloudLayouter.SaveDrawing(path);

            getPath.Should().Throw<ArgumentException>();
        }

        [Test]
        public void GenerateNewFilePath_ReturnExistingDirectory_WhenCalled()
        {
            var path = CloudLayouter.GenerateNewFilePath().Split('\\');
            path[^1] = "";
            var directoryPath = String.Join('\\', path);

            Directory.Exists(directoryPath).Should().Be(true);
        }
    }
}