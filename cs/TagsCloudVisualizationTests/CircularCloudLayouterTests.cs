using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter defaultLayouter;

        [SetUp]
        public void SetUp()
        {
            defaultLayouter = new CircularCloudLayouter(new Point());
        }

        [Test]
        public void HaveNoRectangles_AfterCreating()
        {
            new CircularCloudLayouter(new Point())
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
            var size = new Size(10, 9);
            yield return new TestCaseData(new Point(0, 0), size) {ExpectedResult = new Point(-5, -4)};
            yield return new TestCaseData(new Point(10, 10), size) {ExpectedResult = new Point(5, 6)};
            yield return new TestCaseData(new Point(-10, -10), size) {ExpectedResult = new Point(-15, -14)};
        }

        [Test]
        public void PutNextRectangle_RandomRectangles_NotIntersect()
        {
            var random = new Random();

            for (var i = 0; i < 100; i++)
            {
                var size = new Size(random.Next(1, 100), random.Next(1, 100));
                TestContext.Out.WriteLine(size);

                defaultLayouter.PutNextRectangle(size);

                AssertHaveNoIntersection(defaultLayouter.Rectangles);
            }
        }

        [Test]
        public void PutNextRectangle_SecondRectangle_NotIntersectFirst()
        {
            defaultLayouter.PutNextRectangle(new Size(1, 1));
            defaultLayouter.PutNextRectangle(new Size(10, 10));
            defaultLayouter.Rectangles[1].IntersectsWith(defaultLayouter.Rectangles[0]).Should().BeFalse();
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