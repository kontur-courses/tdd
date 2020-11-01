using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using ProjectCircularCloudLayouter;

namespace CircularCloudLayouterShould
{
    public class Tests
    {
        private CircularCloudLayouter _layouter;
        private List<Rectangle> _rectangles;
        private Random _random = new Random();

        [SetUp]
        public void CreateLayouter()
        {
            _layouter = new CircularCloudLayouter(new Point(0, 0));
        }

        [SetUp]
        public void CreateRectangles()
        {
            _rectangles = new List<Rectangle>
            {
                new Rectangle(new Point(0, 0), new Size(5, 5)),
                new Rectangle(new Point(7, -5), new Size(1, 3)),
                new Rectangle(new Point(-7, -5), new Size(5, 10))
            };
        }

        [TearDown]
        public void DrawCloudAfterFailedTest()
        {
            if (!TestContext.CurrentContext.Result.Outcome.Status.Equals(TestStatus.Failed) ||
                _layouter.GetRectangles.Count == 0) return;
            var imageName = $"{TestContext.CurrentContext.Test.Name}_failed.bmp";
            _layouter.MakeImageTagsCircularCloud(imageName, ImageFormat.Bmp);
        }

        [TestCase(0, 1, TestName = "When zero width")]
        [TestCase(1, 0, TestName = "When zero height")]
        [TestCase(-1, 1, TestName = "When negative width")]
        [TestCase(1, -1, TestName = "When negative height")]
        public void PutNextRectangle_ThrowArgumentException(int width, int height)
        {
            var size = new Size(width, height);

            var act = new Action(() => _layouter.PutNextRectangle(size));

            act.Should().Throw<ArgumentException>();
        }

        [TestCase(1, 1, TestName = "When simple positive size")]
        public void PutNextRectangle_DoNotThrowArgumentException(int width, int height)
        {
            var size = new Size(width, height);

            var act = new Action(() => _layouter.PutNextRectangle(size));

            act.Should().NotThrow<ArgumentException>();
        }

        [TestCase(10, 5, TestName = "When rectangleWidth > height")]
        [TestCase(3, 7, TestName = "When rectangleWidth < height")]
        [TestCase(23, 23, TestName = "When rectangleWidth = height")]
        public void PutNextRectangle_LocationIsEquivalentToSpiralCenterPosition(int widthRectangle, int heightRectangle)
        {
            var size = new Size(widthRectangle, heightRectangle);

            _layouter.PutNextRectangle(size);

            _layouter.GetCurrentRectangle.Location.Should().Be(_layouter.Spiral.Center);
        }

        [TestCase(10, TestName = "10 rectangles when put 10 rectangles")]
        [TestCase(100, TestName = "100 rectangles when put 100 rectangles")]
        [TestCase(10, TestName = "300 rectangles when put 300 rectangles")]
        [TestCase(0, TestName = "Zero when don't put rectangles")]
        public void PutNextRectangle_ManyRectangles(int countRectangles)
        {
            for (var i = 0; i < countRectangles; i++)
                _layouter.PutNextRectangle(new Size(_random.Next(50, 70), _random.Next(20, 40)));

            _layouter.GetRectangles.Count.Should().Be(countRectangles);
        }

        [TestCase(-3, -6, 2, 3,
            TestName = "When rectangle to check within rectangle")]
        [TestCase(1, 2, 5, 7,
            TestName = "When simple intersection")]
        public void IsAnyIntersectWithRectangles_ExistIntersections(int coordinateX, int coordinateY, int width, int height)
        {
            var rectangle = new Rectangle(new Point(coordinateX, coordinateY), new Size(width, height));

            var isIntersect = CircularCloudLayouter.IsAnyIntersectWithRectangles(rectangle, _rectangles);

            isIntersect.Should().BeTrue();
        }

        [TestCase(5, 5, 3, 7,
            TestName = "When exist common points")]
        [TestCase(5, 5, 3, 7,
            TestName = "When not exist common points")]
        public void IsAnyIntersectWithRectangles_NotIntersections(int coordinateX, int coordinateY, int width, int height)
        {
            var rectangle = new Rectangle(new Point(coordinateX, coordinateY), new Size(width, height));

            var isIntersect = CircularCloudLayouter.IsAnyIntersectWithRectangles(rectangle, _rectangles);

            isIntersect.Should().BeFalse();
        }

        [Test, Timeout(1000)]
        public void PutNextRectangle_TimeOut_WhenPut1000ThousandRectangles()
        {
            for (var i = 0; i < 1000; i++)
                _layouter.PutNextRectangle(new Size(_random.Next(50, 70), _random.Next(20, 40)));
        }

        [TestCase(1, 5, 1, 5, TestName = "Zero distance when points are equivalent")]
        [TestCase(-7, 7, -7, 7, TestName = "Zero distance when points are equivalent")]
        public void GetDistanceBetweenPoint_ZeroDistance_WhenEquivalentPoints(int firstX, int firstY,
            int secondX, int secondY)
        {
            var firstPoint = new Point(firstX, firstY);
            var secondPoint = new Point(secondX, secondY);

            var distanceBetweenPoints = CircularCloudLayouter.GetCeilingDistanceBetweenPoints(firstPoint, secondPoint);

            distanceBetweenPoints.Should().Be(0);
        }

        [TestCase(5, -1, 1, 2, 5,
            TestName = "Positive distance when points are different")]
        [TestCase(5, -1, 5, 2, 3,
            TestName = "Positive distance when first coordinates are equal")]
        [TestCase(3, -1, 5, -1, 2,
            TestName = "Positive distance when second coordinates are equal")]
        [TestCase(5, -1, 2, -3, 4,
            TestName = "Positive ceiling distance when ceiling is needed")]
        public void GetDistanceBetweenPoint_ZeroDistance_WhenEquivalentPoints(int firstX, int firstY,
            int secondX, int secondY, int expectedDistance)
        {
            var firstPoint = new Point(firstX, firstY);
            var secondPoint = new Point(secondX, secondY);

            var distanceBetweenPoints = CircularCloudLayouter.GetCeilingDistanceBetweenPoints(firstPoint, secondPoint);

            distanceBetweenPoints.Should().Be(expectedDistance);
        }

        [Test]
        public void PutNextRectangle_IncreasedCloudRadius_WhenPutFirstRectangle()
        {
            var size = _rectangles[0].Size;

            _layouter.PutNextRectangle(size);

            _layouter.CloudRadius.Should().Be(8);
        }
    }
}