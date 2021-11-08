using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter defaultLayouter;

        [SetUp]
        public void SetUp()
        {
            defaultLayouter = new CircularCloudLayouter();
        }

        [Test]
        public void Constructor_ThrowsException_WithNullSpiralPath()
        {
            Assert.Throws<ArgumentException>(() =>
                new CircularCloudLayouter(new Point(), null));
        }

        [Test]
        public void HaveNoRectangles_AfterCreating()
        {
            defaultLayouter
                .Rectangles.Count
                .Should().Be(0);
        }

        [TestCaseSource(nameof(PutNextRectangleFirstRectanglePlacedInCenterTestCases))]
        public Point PutNextRectangle_FirstRectangle_PlacedInCenter(Point center, Size rectangle)
        {
            var layouter = new CircularCloudLayouter(center);
            layouter.PutNextRectangle(rectangle);
            return layouter.Rectangles[0].Location;
        }

        public static IEnumerable<TestCaseData> PutNextRectangleFirstRectanglePlacedInCenterTestCases()
        {
            var size = new Size(3, 4);
            yield return new TestCaseData(new Point(0, 0), size) {ExpectedResult = new Point(-1, -2)};
            yield return new TestCaseData(new Point(3, 4), size) {ExpectedResult = new Point(2, 2)};
            yield return new TestCaseData(new Point(-3, -4), size) {ExpectedResult = new Point(-4, -6)};
        }

        [Test]
        public void PutNextRectangle_SecondRectangle_NotIntersectFirst()
        {
            defaultLayouter.PutNextRectangle(new Size(1, 1));
            defaultLayouter.PutNextRectangle(new Size(10, 10));
            defaultLayouter.Rectangles[1].IntersectsWith(defaultLayouter.Rectangles[0]).Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_RandomRectangles_NotIntersect()
        {
            LayouterBitmapSaver.CreateRandomRectangles(100).ForEach(rectangle =>
            {
                TestContext.WriteLine(rectangle);
                defaultLayouter.PutNextRectangle(rectangle);
                AssertHaveNoIntersection(defaultLayouter.Rectangles);
            });
        }

        [TestCase(0, 0)]
        [TestCase(1, 0)]
        [TestCase(0, 1)]
        [TestCase(-1, -1)]
        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        public void PutNextRectangle_ThrowsException_WithNotPositiveSize(int width, int height)
        {
            Assert.Throws<ArgumentException>(() =>
                defaultLayouter.PutNextRectangle(new Size(width, height)));
        }

        [Test]
        public void PutNextRectangle_WorksFastEnough()
        {
            var rectangles = LayouterBitmapSaver.CreateRandomRectangles(1000);

            Action act = () => rectangles.ForEach(defaultLayouter.PutNextRectangle);

            GC.Collect();
            act.ExecutionTime().Should().BeLessThan(5.Seconds());
        }

        [Test]
        public void PutNextRectangle_RandomRectangles_DensityHighEnough()
        {
            LayouterBitmapSaver.CreateRandomRectangles(1000).ForEach(defaultLayouter.PutNextRectangle);
            var density = CalculateDensity(defaultLayouter.Rectangles);
            density.Should().BeGreaterThan(0.65);
            TestContext.WriteLine($"Density is: {density}");
        }

        private static double CalculateDensity(IReadOnlyCollection<Rectangle> rectangles)
        {
            var topLeft = PointHelper.GetTopLeftAge(
                rectangles.Select(rectangle => new Point(rectangle.X, rectangle.Y)));

            var bottomRight = PointHelper.GetBottomRightAge(
                rectangles.Select(rectangle => new Point(rectangle.Right, rectangle.Bottom)));

            var sideLength = Math.Max(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);
            var radius = sideLength / 2;

            var circleArea = Math.PI * radius * radius;
            var rectanglesArea = rectangles
                .Select(rectangle => rectangle.Width * rectangle.Height)
                .Sum();

            var density = rectanglesArea / circleArea;
            return density;
        }

        private static void AssertHaveNoIntersection(IReadOnlyList<Rectangle> rectangles)
        {
            for (var i = 0; i < rectangles.Count; i++)
                for (var j = i + 1; j < rectangles.Count; j++)
                    Assert.False(rectangles[i].IntersectsWith(rectangles[j]),
                        $"{rectangles[i]} intersects with {rectangles[j]}");
        }
    }
}