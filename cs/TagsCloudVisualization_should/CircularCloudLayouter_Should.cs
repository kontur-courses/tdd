using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    public class CircularCloudLayouter_Should
    {
        private Point center;
        private CircularCloudLayouter layouter;

        [SetUp]
        public void SetUp()
        {
            center = new Point(10, 15);
            layouter = new CircularCloudLayouter(center);
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var visualizer = new BitmapVisualizer(layouter.Rectangles.ToArray());
                visualizer.DrawRectangles(Color.Black, Color.Red);
                var directoryToSave = new DirectoryInfo(@"../../../TestFails");
                visualizer.Save($"{TestContext.CurrentContext.Test.Name}.Failed.png", directoryToSave);
                var fullPath = Path.Combine(directoryToSave.FullName,
                    $"{TestContext.CurrentContext.Test.Name}.Failed.{layouter.Rectangles.Count}rectangles.png");
                Console.WriteLine($"Tag cloud visualization saved to file: {fullPath}");
                ;
            }
        }


        [TestCase(0, 3, TestName = "width of rectangle is not expected to be zero")]
        [TestCase(3, 0, TestName = "height of rectangle is not expected to be zero")]
        [TestCase(-1, 3, TestName = "width of rectangle is not expected to be negative")]
        [TestCase(3, -1, TestName = "height of rectangle is not expected to be zero")]
        public void Throw_WhenSizeIsIncorrect(int width, int height)
        {
            var size = new Size(width, height);
            Action act = () => layouter.PutNextRectangle(size);
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void PutFirstRectangleInCenter()
        {
            var size = new Size(5, 5);
            layouter.PutNextRectangle(new Size(5, 5)).Should().Be(new Rectangle(center, size));
        }

        [TestCase(1)]
        [TestCase(100)]
        public void CreateExpectedNumberOfRectangles(int rectanglesCount)
        {
            var size = new Size(5, 5);
            for (var i = 1; i <= rectanglesCount; i++)
                layouter.PutNextRectangle(size);


            layouter.Rectangles.Count.Should().Be(rectanglesCount);
        }

        [Test]
        public void PutNewRectangleNotIntersectedWithOthers()
        {
            var rectangles = new List<Rectangle>();
            var size = new Size(5, 5);
            var rectanglesCount = 100;
            for (var i = 1; i <= rectanglesCount; i++)
            {
                var rect = layouter.PutNextRectangle(size);
                rect.IntersectsWith(rectangles).Should().BeFalse();
                rectangles.Add(rect);
            }
        }

        [Test]
        public void PutRandomSizeRectanglesNotIntersectedWithOthers()
        {
            var rectangles = new List<Rectangle>();
            var rnd = new Random();
            var rectanglesCount = 100;
            for (var i = 1; i <= rectanglesCount; i++)
            {
                var size = new Size(rnd.Next(1, 10), rnd.Next(1, 10));
                var rect = layouter.PutNextRectangle(size);
                rect.IntersectsWith(rectangles).Should().BeFalse();
                rectangles.Add(rect);
            }
        }

        [Test]
        [Timeout(5000)]
        public void CreateRectanglesNotTooLong()
        {
            var rectanglesCount = 10000;
            var size = new Size(5, 5);
            for (var i = 1; i <= rectanglesCount; i++) layouter.PutNextRectangle(size);
        }

        [Test]
        [Repeat(5)]
        public void CreateRectanglesWithEnoughDensity()
        {
            var rnd = new Random();
            var rectanglesCount = rnd.Next(100, 200);
            for (var i = 0; i < rectanglesCount; i++)
            {
                var size = new Size(rnd.Next(50, 200), rnd.Next(50, 200));
                layouter.PutNextRectangle(size);
            }

            var rectanglesArea = layouter.Rectangles.GetSummaryArea();
            var circleArea = GetDensityCheckingCircleArea(layouter.Rectangles.ToArray());
            var densityCoeff = rectanglesArea / circleArea;
            densityCoeff.Should().BeGreaterOrEqualTo(0.7);
            TestContext.WriteLine($"Коэффициент плотности: {densityCoeff}");
        }

        private double GetDensityCheckingCircleArea(Rectangle[] rectangles)
        {
            var container = rectangles.GetRectanglesContainer();
            var radius = Math.Max(container.Width, container.Height) / 2;
            return Math.PI * radius * radius;
        }
    }
}