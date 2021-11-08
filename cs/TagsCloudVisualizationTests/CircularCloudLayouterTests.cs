using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
            CreateRandomRectangles(100).ForEach(rectangle =>
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
            var rectangles = CreateRandomRectangles(700);

            Action act = () => rectangles.ForEach(defaultLayouter.PutNextRectangle);

            GC.Collect();
            act.ExecutionTime().Should().BeLessThan(5.Seconds());
        }

        [Test]
        public void PutNextRectangle_PlaceRectangles_TightEnough()
        {
            CreateRandomRectangles(1000).ForEach(defaultLayouter.PutNextRectangle);


            var ranges = defaultLayouter.Rectangles
                .Select(rectangle => Math.Sqrt(Math.Pow(rectangle.X, 2) + Math.Pow(rectangle.Y, 2)))
                .ToList();

            ranges.Should().OnlyContain(range => range < 1300);
        }

        [Test]
        [Explicit]
        public void PutNextRectangle_Squares_SaveToBitmap()
        {
            var square = new Size(10, 10);
            var layouter = new CircularCloudLayouter(new Point(1000, 1000));

            Enumerable.Range(0, 1000).ToList().ForEach(_ => layouter.PutNextRectangle(square));

            SaveRectanglesToBitmap(layouter);
        }

        [Test]
        [Explicit]
        public void PutNextRectangle_RandomRectangles_SaveToBitmap()
        {
            var layouter = new CircularCloudLayouter(new Point(2500, 2500));

            CreateRandomRectangles(1000).ForEach(rectangle => layouter.PutNextRectangle(rectangle));

            SaveRectanglesToBitmap(layouter);
        }

        private static void SaveRectanglesToBitmap(CircularCloudLayouter layouter)
        {
            var visualizer = new RectangleVisualizer(layouter.Rectangles);
            var savePath = Path.Combine(Directory.GetCurrentDirectory(), "CircularCloudLayouter.Rectangles.bmp");
            new VisualOutput(visualizer).SaveToBitmap(savePath);
            TestContext.WriteLine($"Saved to '{savePath}'");
        }

        private static void AssertHaveNoIntersection(IReadOnlyList<Rectangle> rectangles)
        {
            for (var i = 0; i < rectangles.Count; i++)
                for (var j = i + 1; j < rectangles.Count; j++)
                    Assert.False(rectangles[i].IntersectsWith(rectangles[j]),
                        $"{rectangles[i]} intersects with {rectangles[j]}");
        }

        private static List<Size> CreateRandomRectangles(int count)
        {
            var random = new Random();
            return Enumerable.Range(0, count)
                .Select(_ => new Size(random.Next(10, 100), random.Next(10, 100)))
                .ToList();
        }
    }
}