using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using TagsCloudVisualization.PointsGenerators;
using TagsCloudVisualization.TagCloud;

namespace TagsCloudVisualizationTests.TagCloudTests
{
    [TestFixture]
    public class CircularCloudLayouter
    {
        [SetUp]
        public void SetUp()
        {
            var centerPoint = new Point(250, 250);
            pointGenerator = new ArchimedesSpiral(centerPoint);
            sut = new TagsCloudVisualization.TagCloud.CircularCloudLayouter(pointGenerator);
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.FailCount == 0)
                return;
            
            var pathToDirectory = @"..\..\..\FailedTests\";
            var fileName = TestContext.CurrentContext.Test.Name;
            TagCloudVisualizer.PrintTagCloud(sut.GetAddedRectangles().ToList(),
                sut.Center, 100, 100, 
                pathToDirectory, fileName);
            
            Console.WriteLine($"Tag cloud visualization saved to file {pathToDirectory}{fileName}");
        }
        
        private ICloudLayouter sut;
        private IPointGenerator pointGenerator;

        [Test]
        public void InitializeCircularCloudLayouter_Throw_NullPointGenerator()
        {
            Assert.Throws<ArgumentException>(
                () => new TagsCloudVisualization.TagCloud.CircularCloudLayouter(null));
        }

        [Test]
        public void GetAddedRectangle_RectCentralPointInCenter_WhenPutOneRectangle()
        {
            var size = new Size(200, 200);
            sut.PutNextRectangle(size);

            var firstAddedRect = sut.GetAddedRectangles().First();

            firstAddedRect.Location.Should().Be(new Point(sut.Center.X - firstAddedRect.Width / 2,
                sut.Center.Y - firstAddedRect.Height / 2));
        }

        [TestCase(0, TestName = "Put 0 rectangles")]
        [TestCase(5, TestName = "Put 5 rectangles")]
        public void PutNextRectangle_CorrectRectanglesCount_ReturnPutRectangles(int expectedCount)
        {
            for (int i = 0; i < expectedCount; i++)
                sut.PutNextRectangle(new Size(200, 200));

            sut.GetAddedRectangles().Count().Should().Be(expectedCount);
        }

        [Test]
        public void GetAddedRectangle_ReturnAddedRectangle_WhenPutOneRectangle()
        {
            var size = new Size(200, 200);
            var expectedRects = new List<Rectangle>
            {
                new Rectangle(
                    new Point(sut.Center.X - size.Width / 2,
                        sut.Center.Y - size.Height / 2), size)
            };
            sut.PutNextRectangle(size);

            var addedRects = sut.GetAddedRectangles();

            addedRects.Should().Equal(expectedRects);
        }

        [TestCase(100, -100, TestName = "Height is negative")]
        [TestCase(-100, 100, TestName = "Width is negative")]
        [TestCase(100, 0, TestName = "Height equal 0")]
        [TestCase(0, 100, TestName = "Width equal 0")]
        public void PutNextRectangle_Throws_IncorrectArguments(int width, int height)
        {
            Assert.Throws<ArgumentException>(() => sut.PutNextRectangle(new Size(width, height)));
        }

        [Test]
        public void PutNextRectangle_ReturnsCorrectSecondRectPosition_WhenPutTwoRectangles()
        {
            sut.PutNextRectangle(new Size(50, 50));
            sut.PutNextRectangle(new Size(50, 50));
            var expectedPosition = new Point(275, 219);

            var secondAddedRectangle = sut.GetAddedRectangles().Skip(1).First();

            secondAddedRectangle.Location.Should().Be(expectedPosition);
        }

        [Test]
        public void PutNextRectangle_PointsGenerationStartsOver_WhenPutOneRectangle()
        {
            sut.PutNextRectangle(new Size(300, 300));

            pointGenerator.GetNextPoint().Should().Be(sut.Center);
        }
        
        [TestCase(2, TestName = "Added 2 rectangles")]
        [TestCase(10, TestName = "Added 10 rectangles")]
        [TestCase(100, TestName = "Added 100 rectangles")]
        public void GetAddedRectangles_RectanglesShouldNotIntersect_WhenPutManyRectangles(int addedRectanglesCount)
        {
            for (int i = 0; i < addedRectanglesCount; i++)
                sut.PutNextRectangle(new Size(50, 50));

            var addedRectangles = sut.GetAddedRectangles().ToList();

            foreach (var addedRectangle in addedRectangles)
                addedRectangles.All(
                    rectangle => rectangle.IntersectsWith(addedRectangle)).Should().Be(false);
        }
    }
}