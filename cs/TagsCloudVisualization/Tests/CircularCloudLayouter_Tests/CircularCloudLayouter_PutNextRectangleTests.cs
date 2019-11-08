using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization.Tests.CircularCloudLayouter_Tests
{
    class CircularCloudLayouter_PutNextRectangleTests
    {
        private CircularCloudLayouter sut;
        private List<Rectangle> rectangles;

        [SetUp]
        public void SetUp()
        {
            sut = new CircularCloudLayouter(new Point(0, 0));
            rectangles = new List<Rectangle>();
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                var path = TestsResultsPath.GetPathToFailedTestResult(
                    nameof(CircularCloudLayouter_PutNextRectangleTests),
                    TestContext.CurrentContext.Test.Name,
                    DateTime.Now);
                var message = "Failed to save result image";
                if (RectanglesVisualizator.TryDrawRectanglesAndSaveAsPng(rectangles.ToArray(), path, out var savedPath))
                    message = $"Tag cloud visualization saved to file {savedPath}";
                TestContext.Out.WriteLine(message);
            }
        }

        [TestCase(-1, 10)]
        [TestCase(10, -4)]
        [TestCase(-5, -7)]
        public void WhenAnyRectSizeIsNegative_ShouldThrow(int rectWidth, int rectHeight)
        {
            Action act = () => sut.PutNextRectangle(new Size(rectWidth, rectHeight));

            act.Should().Throw<ArgumentException>();
        }

        [TestCase(5, 10)]
        [TestCase(0, 0)]
        public void ReturnedRectangleSize_ShouldBeEqualsSizeFromArgument(int rectWidth, int rectHeight)
        {
            var size = new Size(rectWidth, rectHeight);

            var rect = sut.PutNextRectangle(size);

            rect.Size.Should().Be(size);
        }

        [TestCase(5, 10)]
        [TestCase(6, 103)]
        [TestCase(8, 6)]
        [TestCase(7, 5)]
        public void CenterOfFirstRectangle_ShouldBeLayouterCentralPoint(int rectWidth, int rectHeight)
        {
            var center = new Point(101, 500);
            var sut = new CircularCloudLayouter(center);

            var rect = sut.PutNextRectangle(new Size(rectWidth, rectHeight));
            var rectCenter = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);

            rectCenter.Should().Be(center);
        }

        [TestCase(100)]
        [TestCase(20)]
        [TestCase(5)]
        public void MultipleRectangles_ShouldNotIntersectEachOther(int rectanglesCount)
        {
            for (var i = 1; i <= rectanglesCount; i++)
                rectangles.Add(sut.PutNextRectangle(new Size((i % 6 + 1) * 7, (i % 7 + 1) * 3)));

            foreach (var rect in rectangles)
                rectangles
                    .Where(r => !r.Equals(rect))
                    .Any(r => r.IntersectsWith(rect))
                    .Should().BeFalse();
        }

        [TestCase(500)]
        [TestCase(50)]
        [TestCase(10)]
        public void MultipleRectangles_ShouldBeLocatedOnCircle(int rectanglesCount)
        {
           for (var i = 1; i <= rectanglesCount; i++)
                rectangles.Add(sut.PutNextRectangle(new Size((i % 5 + 1) * 10, (i % 8 + 1) * 8)));

            var totalRectanglesArea = rectangles.Sum(r => r.Width * r.Height);
            var minRadius = (int)Math.Sqrt((totalRectanglesArea / Math.PI));
            var radius = minRadius + minRadius / 2 + 50;

            rectangles.All(r => IsRectangleInsideCircleWithZeroCentralPoint(r, radius))
                .Should()
                .BeTrue();
        }

        private bool IsRectangleInsideCircleWithZeroCentralPoint(Rectangle rect, int circlesRadius)
        {
            var radiusSquared = Math.Pow(circlesRadius, 2);

            bool IsPointInsideCircle(Point p) => Math.Pow(p.X, 2) + Math.Pow(p.Y, 2) <= radiusSquared;

            return IsPointInsideCircle(new Point(rect.Left, rect.Bottom)) &&
                IsPointInsideCircle(new Point(rect.Right, rect.Bottom)) &&
                IsPointInsideCircle(new Point(rect.Right, rect.Top)) &&
                IsPointInsideCircle(new Point(rect.Left, rect.Top));
        }
    }
}