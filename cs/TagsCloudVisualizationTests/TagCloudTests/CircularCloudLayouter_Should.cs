using System;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;
using TagsCloudVisualization.PointsGenerators;
using TagsCloudVisualization.TagCloud;

namespace TagsCloudVisualizationTests.TagCloudTests
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        [SetUp]
        public void SetUp()
        {
            var canvasSize = new Size(500, 500);
            var centerPoint = new Point(canvasSize.Width / 2, canvasSize.Height / 2);
            pointGenerator = new ArchimedesSpiral(centerPoint, canvasSize);
            cloudLayouter = new CircularCloudLayouter(pointGenerator);
        }

        private ICloudLayouter cloudLayouter;
        private IPointGenerator pointGenerator;

        [Test]
        public void InitializeCircularCloudLayouter_Throw_NullPointGenerator()
        {
            Assert.Throws<ArgumentException>(() => new CircularCloudLayouter(null));
        }

        [TestCase(0)]
        [TestCase(3)]
        public void AddedRectanglesCount_ReturnRutRectangles(int expectedCount)
        {
            for (int i = 0; i < expectedCount; i++)
                cloudLayouter.PutNextRectangle(new Size(200, 200));

            cloudLayouter.AddedRectanglesCount.Should().Be(expectedCount);
        }

        [Test]
        public void GetAddedRectangle_CentralRectPointInCenter_WhenPutOneRectangle()
        {
            var size = new Size(200, 200);
            cloudLayouter.PutNextRectangle(size);

            var firstAddedRect = cloudLayouter.GetAddedRectangle(0);

            firstAddedRect.Location.Should().Be(new Point(cloudLayouter.Center.X - firstAddedRect.Width / 2,
                cloudLayouter.Center.Y - firstAddedRect.Height / 2));
        }

        [Test]
        public void GetAddedRectangle_ReturnAddedRectangle_WhenPutOneRectangle()
        {
            var size = new Size(200, 200);
            cloudLayouter.PutNextRectangle(size);

            var firstAddedRect = cloudLayouter.GetAddedRectangle(0);

            firstAddedRect.Should().Be(new Rectangle(
                new Point(cloudLayouter.Center.X - firstAddedRect.Width / 2,
                    cloudLayouter.Center.Y - firstAddedRect.Height / 2), size));
        }

        [TestCase(-1, TestName = "Index is negative")]
        [TestCase(20, TestName = "Index more than put rectangles")]
        public void GetAddedRectangle_Throws_IncorrectIndex(int index)
        {
            cloudLayouter.PutNextRectangle(new Size(200, 200));

            Assert.Throws<ArgumentException>(() => cloudLayouter.GetAddedRectangle(index));
        }

        [TestCase(100, -100, TestName = "Height is negative")]
        [TestCase(-100, 100, TestName = "Width is negative")]
        [TestCase(100, 0, TestName = "Height equal 0")]
        [TestCase(0, 100, TestName = "Width equal 0")]
        [TestCase(500, 100, TestName = "Width is equal canvas width")]
        [TestCase(100, 500, TestName = "Height is equal canvas height")]
        [TestCase(1000, 100, TestName = "Width is more than canvas width")]
        [TestCase(1000, 100, TestName = "Height is more than canvas height")]
        public void PutNextRectangle_Throws_IncorrectArguments(int width, int height)
        {
            Assert.Throws<ArgumentException>(() => cloudLayouter.PutNextRectangle(new Size(width, height)));
        }

        [Test]
        public void PutNextRectangle_ReturnsCorrectSecondRectPosition_WhenPutTwoRectangles()
        {
            cloudLayouter.PutNextRectangle(new Size(50, 50));
            cloudLayouter.PutNextRectangle(new Size(50, 50));
            var expectedPosition = new Point(275, 219);

            var secondAddedRectangle = cloudLayouter.GetAddedRectangle(1);

            secondAddedRectangle.Location.Should().Be(expectedPosition);
        }

        [Test]
        public void PutNextRectangle_ReturnsCorrectSecondRectSize_WhenPutRectanglesNotFitInCanvas()
        {
            cloudLayouter.PutNextRectangle(new Size(300, 300));
            cloudLayouter.PutNextRectangle(new Size(300, 300));
            var expectedSize = new Size(150, 150);

            var secondAddedRectangle = cloudLayouter.GetAddedRectangle(1);

            secondAddedRectangle.Size.Should().Be(expectedSize);
        }

        [Test]
        public void PutNextRectangle_PointsGenerationStartsOver_WhenPutOneRectangle()
        {
            cloudLayouter.PutNextRectangle(new Size(300, 300));

            pointGenerator.GetNextPoint().Should().Be(cloudLayouter.Center);
        }
    }
}