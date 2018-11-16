using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;


namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter _layout;

        [SetUp]
        public void SetUp()
        {
            _layout = null;
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.FailCount > 0 && _layout != null)
            {
                var testDir = TestContext.CurrentContext.TestDirectory;
                var methodName = TestContext.CurrentContext.Test.Name;
                var filePath = System.IO.Path.Combine(testDir, "..", "..", "Tests", "BadCases", "Case" + methodName + ".jpg");

                var visualizer = new Visualizer(_layout);
                visualizer.DrawTagsCloud(filePath);
                Console.WriteLine("Tag cloud visualization saved to file: {0}", filePath);
            }

            _layout = null;
        }

        [TestCase(TestName = "ShouldFall_AndCreateFileReport")]
        public void PutNextRectangle_IncorrectPlacement()
        {
            _layout = GetIncorrectRectanglePlacement();
            var result = CircularCloudLayouter.IsRectanglesIntersect(_layout.Rectangles[0], _layout.Rectangles[1]);

            result.Should().BeFalse();
        }

        public static CircularCloudLayouter GetIncorrectRectanglePlacement()
        {
            var layout = new CircularCloudLayouter(new Point(0, 0));
            var rect1 = new Rectangle(new Point(0, 10), new Size(20, 10));
            var rect2 = new Rectangle(new Point(-10, 10), new Size(20, 10));

            layout.Rectangles.AddRange(new List<Rectangle>() {rect1, rect2});

            return layout;
        }

        [TestCase(ExpectedResult = false)]
        public bool PutNextRectangle_CorrectInputShouldNotIntersect()
        {
            _layout = new CircularCloudLayouter(new Point(0, 0));
            _layout.PutNextRectangle(new Size(69, 39));
            _layout.PutNextRectangle(new Size(68, 44));
            _layout.PutNextRectangle(new Size(85, 53));
            _layout.PutNextRectangle(new Size(110, 46));

            return CircularCloudLayouter.IsRectanglesIntersect(_layout.Rectangles);
        }

        [TestCase(ExpectedResult = false)]
        public bool PutNextRectangle_RandomAmountOfRectanglesShouldNotIntersect()
        {
            _layout = new CircularCloudLayouter(new Point(0, 0));

            var rnd = new Random();
            for (var i = 0; i < rnd.Next(1, 30); i++)
            {
                var x = rnd.Next(60, 120);
                var y = rnd.Next(30, 60);
                if ((x + y) % 2 != 0)
                    x++;

                _layout.PutNextRectangle(new Size(x, y));
            }

            return CircularCloudLayouter.IsRectanglesIntersect(_layout.Rectangles);
        }

        [TestCase(10, 4)]
        public void PutNextRectangle(int width, int height)
        {
            _layout = new CircularCloudLayouter(new Point(0, 0));
            var rectangle = _layout.PutNextRectangle(new Size(width, height));

            rectangle.Should().BeEquivalentTo(new Rectangle(0, height, width, height));
        }

        [TestCase(-2, -3, "both center coordinates should be non-negative",
            TestName = "FallOn_NegativeCoordinates")]
        public void ConstructorIncorrectInput(int centerX, int centerY, string msg)
        {
            Action act = () => new CircularCloudLayouter(new Point(centerX, centerY));

            act.Should().Throw<ArgumentException>()
                .WithMessage(msg);
        }
    }
}