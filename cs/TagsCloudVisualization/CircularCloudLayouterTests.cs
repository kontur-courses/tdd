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
        private CircularCloudVisualizator visualizator;
        private List<Rectangle> rectangles;

        [SetUp]
        public void SetUp()
        {
            center = new Point(400, 400);
            layouter = new CircularCloudLayouter(center);
            visualizator = new CircularCloudVisualizator(new Size(800, 800));
            rectangles = new List<Rectangle>();
        }

        [TestCase(0, 0, TestName = "Zero coordinates")]
        [TestCase(-10, -10, TestName = "Negative coordinates")]
        [TestCase(100500, 100500, TestName = "Big positive coordinates")]
        public void Constructor_ShouldNotThrowArgumentException_OnCorrectInput(int x, int y)
        {
            var centralPoint = new Point(x, y);
            Action act = () => new CircularCloudLayouter(centralPoint);
            act.Should().NotThrow();
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
        public void PutNextRectangle_ShouldNotThrowException_OnCorrectInput(int x, int y)
        {
            var size = new Size(x, y);
            Action act = () => layouter.PutNextRectangle(size);
            act.Should().NotThrow();
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

        [TearDown]
        public void TearDown()
        {
            var testResult = TestContext.CurrentContext.Result.Outcome;
            var path = "../mistake.jpg";

            if (Equals(testResult, ResultState.Failure) ||
                Equals(testResult == ResultState.Error))
            {
                visualizator.DrawRectangles(rectangles);
                visualizator.SaveCanvas(path);
                Console.WriteLine($"Tag cloud visualization saved to file {path}", path);
            }
        }
    }
}