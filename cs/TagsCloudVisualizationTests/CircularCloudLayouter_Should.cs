using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter _sut;
        private List<Rectangle> _rectangles;

        [SetUp]
        public void SetUp()
        {
            _sut = new CircularCloudLayouter(new Point(0, 0));
            _rectangles = new List<Rectangle>();
        }

        [TearDown]
        public void TearDown()
        {
            if (!TestContext.CurrentContext.Result.Outcome.Status.Equals(TestStatus.Failed) ||
                _rectangles.Count == 0) return;
            var test = TestContext.CurrentContext.Test;
            var testName = test.FullName.Split('.');
            var fullTestName =
                $"{testName[0]}.{testName[1]}.{test.MethodName}.{(testName.Length == 3 ? testName[2] : "")}";
            var imageName = $"{DateTime.Now:yyyy.MM.dd HH.mm.ss}_{fullTestName}_failed.bmp";
            CircularCloudVisualisation.MakeImageTagsCircularCloud(_rectangles, _sut.CloudRadius, imageName,
                ImageFormat.Bmp);
        }

        [TestCase(0, 1, TestName = "When zero width")]
        [TestCase(1, 0, TestName = "When zero height")]
        [TestCase(-1, 1, TestName = "When negative width")]
        [TestCase(1, -1, TestName = "When negative height")]
        public void PutNextRectangle_ThrowArgumentException(int width, int height)
        {
            var size = new Size(width, height);

            var act = new Action(() => _rectangles.Add(_sut.PutNextRectangle(size)));

            act.Should().Throw<ArgumentException>();
        }

        [TestCase(1, 1, TestName = "When simple positive size")]
        public void PutNextRectangle_DoNotThrowArgumentException(int width, int height)
        {
            var size = new Size(width, height);

            var act = new Action(() => _rectangles.Add(_sut.PutNextRectangle(size)));

            act.Should().NotThrow<ArgumentException>();
        }

        [TestCase(10, 5, TestName = "When rectangleWidth > height")]
        [TestCase(3, 7, TestName = "When rectangleWidth < height")]
        [TestCase(23, 23, TestName = "When rectangleWidth = height")]
        public void PutNextRectangle_LocationIsEquivalentToSpiralCenterPosition(int widthRectangle, int heightRectangle)
        {
            var size = new Size(widthRectangle, heightRectangle);

            _rectangles.Add(_sut.PutNextRectangle(size));

            _sut.GetCurrentRectangle.Location.Should().Be(new Point(0, 0));
        }

        [TestCase(10, TestName = "10 rectangles when put 10 rectangles")]
        [TestCase(100, TestName = "100 rectangles when put 100 rectangles")]
        [TestCase(10, TestName = "300 rectangles when put 300 rectangles")]
        [TestCase(0, TestName = "Zero when don't put rectangles")]
        public void PutNextRectangle_CountRectangles(int countRectangles)
        {
            _rectangles.AddRange(_sut.MakeLayouter(countRectangles, 50,
                70, 20, 40));

            _sut.RectanglesCount.Should().Be(countRectangles);
        }


        [Test, Timeout(1000)]
        public void PutNextRectangle_MakeItFastUntilTimeEnd_WhenPutOneThousandRectangles()
        {
            _rectangles.AddRange(_sut.MakeLayouter(1000, 50,
                70, 20, 40));
        }

        [Test]
        public void PutNextRectangle_IncreasedCloudRadius_WhenPutFirstRectangle()
        {
            var size = new Size(5, 5);

            _rectangles.Add(_sut.PutNextRectangle(size));

            _sut.CloudRadius.Should().Be(8);
        }
    }
}