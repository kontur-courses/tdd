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
        private double angleOffset;
        private double radiusOffset;

        [SetUp]
        public void SetUp()
        {
            angleOffset = 1;
            radiusOffset = 1;
            center = new Point(0, 0);
            layouter = new CircularCloudLayouter(new Spiral(center, angleOffset, radiusOffset));
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

        [TestCaseSource(nameof(_putNextRectangleShouldCorrectPlaceRectanglesCases))]
        public void PutNextRectangle_ShouldCorrectPlaceRectangles(List<Rectangle> expectedRectangles)
        {
            var rectangleSize = new Size(10, 10);
            for (var i = 0; i < 5; i++)
            {
                var rectangle = layouter.PutNextRectangle(rectangleSize);
                rectangles.Add(rectangle);
            }

            for (var i = 0; i < 5; i++)
                rectangles[i].Location.Should().BeEquivalentTo(expectedRectangles[i].Location);
        }

        private static object[] _putNextRectangleShouldCorrectPlaceRectanglesCases =
        {
            new List<Rectangle>
            {
                new Rectangle(-5, -5, 10, 10), new Rectangle(-5, -16, 10, 10),
                new Rectangle(5, -11, 10, 10), new Rectangle(7, 0, 10, 10),
                new Rectangle(-3, 9, 10, 10)
            }
        };

        [TearDown]
        public void TearDown()
        {
            var testResult = TestContext.CurrentContext.Result.Outcome;
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