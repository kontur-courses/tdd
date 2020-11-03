using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloud.Core;

namespace TagsCloud.Tests
{
    internal class CircularCloudLayouter_Tests
    {
        private const double MinDensity = 0.5;
        public const int ImageWidth = 1920;
        public const int ImageHeight = 1080;
        private Point center;
        private CircularCloudLayouter cloud;

        [SetUp]
        public void SetUp()
        {
            center = new Point(ImageWidth / 2, ImageHeight / 2);
            cloud = new CircularCloudLayouter(center);
        }

        [TestCase(-1, 1, TestName = "WhenNotPositiveWidth")]
        [TestCase(1, -1, TestName = "WhenNotPositiveHeight")]
        public void PutNextRectangle_ThrowException(int width, int height)
        {
            Assert.Throws<ArgumentException>(() => cloud.PutNextRectangle(new Size(width, height)));
        }

        [TestCase(1, TestName = "WhenAdd1Rectangle")]
        [TestCase(10, TestName = "WhenAdd10Rectangle")]
        [TestCase(100, TestName = "WhenAdd100Rectangle")]
        public void PutNextRectangle_CorrectCountOfRectangles(int expectedCount)
        {
            for (var i = 0; i < expectedCount; ++i)
                cloud.PutNextRectangle(new Size(1, 1));

            cloud.Rectangles.Should().HaveCount(expectedCount);
        }

        [Test]
        public void PutNextRectangle_FirstRectangleOnCenter()
        {
            center = new Point(1, 2);
            cloud = new CircularCloudLayouter(center);

            var rect = cloud.PutNextRectangle(new Size(10, 10));
            var rectCenter = new Point(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);

            rectCenter.Should().Be(center);
        }

        [Timeout(1000)]
        [TestCase(1000, 10, 10, 10, 10, TestName = "WhenAdd1000SameRectangles")]
        [TestCase(1000, 10, 50, 10, 20, TestName = "WhenAdd1000RandomRectangles")]
        public void PutNextRectangle_RectanglesDoesNotIntersect(int count, int minWidth, int maxWidth, int minHeight,
            int maxHeight)
        {
            for (var i = 0; i < count; ++i)
                cloud.PutNextRectangle(SizeGenerator.GenerateSize(minWidth, maxWidth, minHeight, maxHeight));

            foreach (var rect in cloud.Rectangles)
                rect.IntersectsWith(cloud.Rectangles.Where(x => x != rect)).Should().BeFalse();
        }

        [Timeout(1000)]
        [TestCase(1000, 10, 10, 10, 10, TestName = "WhenAdd1000SameRectangles")]
        [TestCase(1000, 10, 40, 10, 20, TestName = "WhenAdd1000RandomRectangles")]
        public void PutNextRectangle_PlacedRectanglesHasNearlyCircleShape(int count, int minWidth, int maxWidth,
            int minHeight, int maxHeight)
        {
            for (var i = 0; i < count; ++i)
                cloud.PutNextRectangle(SizeGenerator.GenerateSize(minWidth, maxWidth, minHeight, maxHeight));

            var top = center.Y - cloud.Rectangles.Min(rect => rect.Top);
            var left = center.X - cloud.Rectangles.Min(rect => rect.Left);
            var right = cloud.Rectangles.Max(rect => rect.Right) - center.X;
            var bottom = cloud.Rectangles.Max(rect => rect.Bottom) - center.Y;
            var radius = Math.Max(Math.Max(top, bottom), Math.Max(left, right));
            var fault = radius / 3.0;

            foreach (var rect in cloud.Rectangles)
                GetDistanceFromPointToCenter(rect.Location).Should().BeLessThan(radius + fault);
        }

        [Timeout(1000)]
        [TestCase(1000, 15, 15, 10, 10, TestName = "WhenAdd1000SameRectangles")]
        [TestCase(1000, 15, 40, 10, 20, TestName = "WhenAdd1000RandomRectangles")]
        public void PutNextRectangle_RectanglesPlacedTightly(int count, int minWidth, int maxWidth, int minHeight,
            int maxHeight)
        {
            var allRectanglesArea = 0.0;
            for (var i = 0; i < count; ++i)
            {
                var rect = cloud.PutNextRectangle(SizeGenerator.GenerateSize(minWidth, maxWidth, minHeight, maxHeight));
                allRectanglesArea += rect.Width * rect.Height;
            }

            var radius = cloud.Rectangles.Max(x => GetDistanceFromPointToCenter(x.Location));
            var circleArea = Math.PI * radius * radius;
            var areaRatio = allRectanglesArea / circleArea;

            areaRatio.Should().BeGreaterOrEqualTo(MinDensity);
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed)
                return;
            var path = $"../../Images/{TestContext.CurrentContext.Test.FullName}.jpg";
            Console.WriteLine($"Tag cloud visualization saved to file {path}");
            CircularCloudVisualization.SaveImage(cloud.Rectangles, ImageWidth, ImageHeight).Save(path);
        }

        private double GetDistanceFromPointToCenter(Point point)
        {
            return Math.Sqrt(Math.Pow(point.X - center.X, 2) + Math.Pow(point.Y - center.Y, 2));
        }
    }
}