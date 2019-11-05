using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Tests
    {
        private CircularCloudLayouter layouter;
        private readonly Point center = new Point(250, 250);

        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(
                center,
                new ArchimedeanSpiral(1));
        }

        [TestCase(5, TestName = "WhenPut5Rectangles")]
        [TestCase(25, TestName = "WhenPut25Rectangles")]
        [TestCase(50, TestName = "WhenPut50Rectangles")]
        public void PutNextRectangle_ReturnsDisjointRectangles(int count)
        {
            var rectangles = new List<Rectangle>();
            for (var i = 1; i < count; i++)
                rectangles.Add(layouter.PutNextRectangle(new Size(i, i)));
            IsDisjointRectangles(rectangles).Should().BeTrue();
        }

        [Test]
        public void PutNextRectangle_ShouldPutFirstRectangleInCenter()
        {
            var size = new Size(50, 70);
            var rectangle = layouter.PutNextRectangle(size);
            var expectedLocation = new Point(center.X - size.Width / 2, center.Y - size.Height / 2);
            rectangle.X.Should().BeInRange(
                expectedLocation.X - 1,
                expectedLocation.X + 1);
            rectangle.Y.Should().BeInRange(
                expectedLocation.Y - 1,
                expectedLocation.Y + 1);
        }

        [TestCase(10, TestName = "WhenPut10Rectangles")]
        [TestCase(30, TestName = "WhenPut30Rectangles")]
        [TestCase(50, TestName = "WhenPut50Rectangles")]
        public void PutNextRectangle_ShouldPutRectanglesTightly(int rectanglesCount)
        {
            var size = new Size(10, 10);
            for (var i = 0; i < rectanglesCount - 1; i++)
            {
                layouter.PutNextRectangle(size);
            }

            var rect = layouter.PutNextRectangle(size);
            Math.Abs(rect.X - center.X).Should()
                .BeLessThan(rectanglesCount * size.Width / (rectanglesCount / 4));
            Math.Abs(rect.Y - center.Y).Should()
                .BeLessThan(rectanglesCount * size.Height / (rectanglesCount / 4));
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome != ResultState.Failure)
                return;
            var failedTestsPath = TestContext.CurrentContext.TestDirectory + @"\FailedTests";
            if (!Directory.Exists(failedTestsPath))
                Directory.CreateDirectory(failedTestsPath);
            var cloudDrawer = new CircularCloudDrawer(new Size(500, 500), layouter);
            cloudDrawer.DrawLayouterRectangles();
            var filename = failedTestsPath + $"\\{TestContext.CurrentContext.Test.FullName}.png";
            cloudDrawer.Save(filename);
            TestContext.WriteLine($"Tag cloud visualisation saved to file: '{filename}'");
        }

        private bool IsDisjointRectangles(List<Rectangle> rectangles)
        {
            for (var i = 0; i < rectangles.Count; i++)
            {
                for (var j = i + 1; j < rectangles.Count; j++)
                {
                    if (rectangles[i].IntersectsWith(rectangles[j]))
                        return false;
                }
            }

            return true;
        }
    }
}