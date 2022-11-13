using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter layouter;
        private Point center;
        private Drawer visualizator;
        private List<Rectangle> rectangles;
        private Spiral spiral;
        private double angleOffset;
        private double radiusOffset;

        [SetUp]
        public void SetUp()
        {
            angleOffset = 1;
            radiusOffset = 1;
            center = new Point(0, 0);
            spiral = new Spiral(center, angleOffset, radiusOffset);
            layouter = new CircularCloudLayouter(center, new Spiral(center, angleOffset, radiusOffset));
            visualizator = new Drawer(new Size(800, 800));
            rectangles = new List<Rectangle>();
        }

        [Test]
        public void PutNextRectangle_ShouldReturnRectangleWithCorrectSize()
        {
            var size = new Size(1000, 10000);
            var rectangle = layouter.PutNextRectangle(size);
            rectangle.Size.Should().Be(size);
        }

        [Test]
        public void PutNextRectangle_ShouldPlaceFirstRectangleInCenter()
        {
            var size = new Size(8, 8);
            var rectangleInCenter = new Rectangle(new Point(center.X - 4, center.Y - 4), size);
            var rectangle = layouter.PutNextRectangle(size);
            rectangle.Should().BeEquivalentTo(rectangleInCenter);
        }

        [TestCase(1, -3, TestName = "Y < 0")]
        [TestCase(-3, 1, TestName = "X < 0")]
        [TestCase(-3, -3, TestName = "X < 0, Y < 0")]
        public void PutNextRectangle_ShouldThrowArgumentException_OnIncorrectInput(int x, int y)
        {
            var size = new Size(x, y);
            Action act = () => layouter.PutNextRectangle(size);
            act.Should().Throw<ArgumentException>().WithMessage("Wrong size of rectangle");
        }

        [TestCase(0, 3, TestName = "X = 0, Y > 0")]
        [TestCase(3, 0, TestName = "X > 0, Y = 0")]
        [TestCase(0, 0, TestName = "X = 0, Y = 0")]
        [TestCase(10000, 10000, TestName = "Big rectangle")]
        public void PutNextRectangle_ShouldReturnCorrectRectangle_OnCorrectInput(int x, int y)
        {
            var size = new Size(x, y);
            var coordinatesCorrectRectangle = new Point(center.X - x / 2, center.Y - y / 2);
            var rectangle = layouter.PutNextRectangle(size);
            rectangle.Should().BeEquivalentTo(new Rectangle(coordinatesCorrectRectangle, size));
        }

        [Test]
        public void PutNextRectangle_ShouldNotIntersectRectangles()
        {
            var rnd = new Random();
            for (var i = 0; i < 100; i++)
            {
                var rectangle = layouter.PutNextRectangle(new Size(rnd.Next(10, 100), rnd.Next(10, 50)));
                rectangles.Add(rectangle);
            }

            foreach (var rectangle in rectangles)
                rectangles.Any(rect => rect.IntersectsWith(rectangle) && rect != rectangle).Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_ShouldCorrectPlaceRectangles()
        {
            var expectedRectangles = new List<Rectangle>();
            var rectangleSize = new Size(10, 10);
            for (var i = 0; i < 5; i++)
            {
                var rectangle = layouter.PutNextRectangle(rectangleSize);
                rectangles.Add(rectangle);

                var expectedLocation = i == 0
                    ? new Rectangle(new Point(-5, -5), rectangleSize)
                    : spiral
                        .GetPoints()
                        .Select(point =>
                            new Rectangle(
                                RectangleCoordinatesCalculator.CalculateRectangleCoordinates(point, rectangleSize),
                                rectangleSize))
                        .First(rect =>
                            !expectedRectangles.Any(r => r.IntersectsWith(rect)));
                expectedRectangles.Add(expectedLocation);
            }

            for (var i = 0; i < 5; i++)
            {
                rectangles[i].Location.Should().BeEquivalentTo(expectedRectangles[i].Location);
            }
        }

        [TearDown]
        public void TearDown()
        {
            var testResult = TestContext.CurrentContext.Result.Outcome;
            var testMethodName = TestContext.CurrentContext.Test.MethodName;
            var testName = TestContext.CurrentContext.Test.FullName;
            var path = $"../mistake_{testName}.jpg";
            if (testResult.Status == TestStatus.Failed)
            {
                visualizator.DrawRectangles(rectangles);
                visualizator.SaveCanvas(path);
                Console.WriteLine($"Tag cloud visualization saved to file {path}", path);
            }
        }
    }
}