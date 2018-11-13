using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    [TestFixture]
    class TagsCloudVisualization_should
    {
        private readonly string currentTestFolderPath = TestContext.CurrentContext.TestDirectory;
        private CircularCloudLayouter currentCircularCloudLayouter;

        [TestCase(0, 0, TestName = "when origin is zero point")]
        [TestCase(16.548, 8651.561, TestName = "when origin is random point")]
        public void ReturnRectangleAtOrigin(double x, double y)
        {
            var origin = new Point(x, y);
            currentCircularCloudLayouter = new CircularCloudLayouter(origin);
            var size = new Size(100, 100);

            var rectangle = currentCircularCloudLayouter.PutNextRectangle(size);

            rectangle.Center.Should().BeEquivalentTo(origin);
        }

        [Test]
        public void NoOneRectangleShouldIntersectWithOthers_WhenAddMoreThenOne()
        {
            var origin = new Point(0, 0);
            currentCircularCloudLayouter = new CircularCloudLayouter(origin);
            var size = new Size(200, 100);
            var rectangles = new List<Rectangle>();
            var result = false;

            for (var i = 0; i < 10; i++)
            {
                var rectangle = currentCircularCloudLayouter.PutNextRectangle(size);
                if (result)
                    break;
                result = rectangle.IsIntersectsWithAnyRect(rectangles);
                rectangles.Add(rectangle);
            }

            result.Should().BeFalse();
        }

        [Test]
        public void MustPositioningAsDenseCircle()
        {
            var origin = new Point(0, 0);
            currentCircularCloudLayouter = new CircularCloudLayouter(origin);
            var size = new Size(3, 3);
            var rectangles = new List<Rectangle>();

            for (var i = 0; i < 360; i++)
            {
                var rectangle = currentCircularCloudLayouter.PutNextRectangle(size);
                rectangles.Add(rectangle);
            }

            var width = rectangles.GetWidth();
            var height = rectangles.GetHeight();
            var radius = Math.Max(width, height);
            var circleSquare = Math.PI * Math.Pow(radius, 2);
            var rectsSquare = rectangles.Select(r => r.Size.Square).Sum();

            var areRectsWithinCircle = rectangles
                .Select(r => r.Center)
                .Select(rectCenter => Vector.GetLength(origin, rectCenter))
                .All(radiusVector => radiusVector <= radius);
            var isCircleDense = circleSquare - rectsSquare < circleSquare * 0.5;
            var result = areRectsWithinCircle && isCircleDense;

            result.Should().BeTrue();
        }

        [TearDown]
        public void TearDown()
        {
            var testResult = TestContext.CurrentContext.Result.Outcome;
            var testName = TestContext.CurrentContext.Test.Name;
            if (Equals(testResult, ResultState.Failure)
                || Equals(testResult, ResultState.Error))
            {
                var dateTime = DateTime.Today.ToString("dd.MM.yyyy HH.mm.ss");
                var filename = $"[{dateTime}] {testName}.png";
                var path = Path.Combine(currentTestFolderPath, filename);
                var width = (int)currentCircularCloudLayouter.GetCloud().GetWidth();
                var height = (int)currentCircularCloudLayouter.GetCloud().GetHeight();
                currentCircularCloudLayouter.GetCloud().VizualizeToFile(width, height, path);
                Console.WriteLine($"Tag cloud visualization saved to file {path}");
            }
        }
    }
}
