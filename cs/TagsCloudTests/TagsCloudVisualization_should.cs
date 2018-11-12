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
        public void MustPositioningAsCircle()
        {
            var origin = new Point(0, 0);
            currentCircularCloudLayouter = new CircularCloudLayouter(origin);
            var size = new Size(1, 1);
            var rectangles = new List<Rectangle>();
            const int radius = 11;

            for (var i = 0; i < 10; i++)
            {
                var rectangle = currentCircularCloudLayouter.PutNextRectangle(size);
                rectangles.Add(rectangle);
            }

            var result = rectangles
                .Select(r => r.Center)
                .Select(p => Math.Pow(p.X - origin.X, 2) + Math.Pow(p.Y - origin.Y, 2))
                .All(h => h <= radius);
            result.Should().BeTrue();
        }

        [Test]
        public void DenseCloudFormed()
        {
            var origin = new Point(0, 0);
            currentCircularCloudLayouter = new CircularCloudLayouter(origin);
            var size = new Size(1, 1);
            var rectangles = new List<Rectangle>();
            const int radius = 11;

            for (var i = 0; i < 10; i++)
            {
                var rectangle = currentCircularCloudLayouter.PutNextRectangle(size);
                rectangles.Add(rectangle);
            }

            var result = rectangles
                .Select(r => r.Center)
                .Select(p => Math.Pow(p.X - origin.X, 2) + Math.Pow(p.Y - origin.Y, 2))
                .Select(Math.Abs)
                .All(h => h <= radius);
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
                var width = currentCircularCloudLayouter.Width;
                var height = currentCircularCloudLayouter.Height;
                currentCircularCloudLayouter.GetCloud().VizualizeToFile(width, height, path);
                Console.WriteLine($"Tag cloud visualization saved to file {path}");
            }
        }
    }
}
