using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    public class CircularCloudLayouter_Should
    {
        private static IEnumerable<TestCaseData> DensityTestData
        {
            get
            {
                yield return new TestCaseData(0.5, GetRandomSizes(1000, 1000, 1000))
                    .SetName("1000 different sizes");

                yield return new TestCaseData(0.9, Enumerable.Repeat(new Size(4, 4), 1000))
                    .SetName("1000 identical squares");
            }
        }

        [Test]
        public void ReturnRectangleWithSpecifiedSize()
        {
            var size = new Size(123, 456);

            var layouter = new CircularCloudLayouter(new Point(0, 0));
            var rectangle = layouter.PutNextRectangle(size);

            rectangle.Size.Should().BeEquivalentTo(size);
        }

        [Test]
        public void NotOverlapAnyRectangles()
        {
            var rectanglesCount = 1000;
            var sizes = GetRandomSizes(1000, rectanglesCount, 1000);

            var layouter = new CircularCloudLayouter(new Point(0, 0));
            foreach (var size in sizes)
                layouter.PutNextRectangle(size);

            for (var i = 0; i < rectanglesCount; i++)
            for (var j = i + 1; j < rectanglesCount; j++)
                Assert.IsFalse(layouter.Rectangles[i].IntersectsWith(layouter.Rectangles[j]));
        }

        [TestCaseSource(nameof(DensityTestData))]
        public void LayOutDenserThanThreshold(double density, IEnumerable<Size> sizes)
        {
            var desiredDensity = 0.5;
            var center = new Point(0, 0);

            var layouter = new CircularCloudLayouter(center);
            foreach (var size in sizes)
                layouter.PutNextRectangle(size);
            var radius = layouter.GetRadius();
            var circleSquare = Math.PI * radius * radius;
            var rectanglesSquare = layouter.Rectangles.Sum(r => r.Height * r.Width);

            (rectanglesSquare / circleSquare).Should().BeGreaterThan(desiredDensity);
        }

        private static List<Size> GetRandomSizes(int seed, int rectanglesCount, int maxDimensionSize)
        {
            var sizesGenerator = new Random(seed);
            var sizes = new List<Size>();
            for (var i = 0; i < rectanglesCount; i++)
                sizes.Add(new Size(sizesGenerator.Next(maxDimensionSize), sizesGenerator.Next(maxDimensionSize)));
            return sizes;
        }
    }


    internal static class Extensions
    {
        public static IEnumerable<Point> GetVertices(this Rectangle rectangle)
        {
            var upperLeft = rectangle.Location;
            return Enumerable.Empty<Point>()
                .Append(upperLeft)
                .Append(upperLeft + new Size(0, rectangle.Height))
                .Append(upperLeft + new Size(rectangle.Width, 0))
                .Append(upperLeft + new Size(rectangle.Width, rectangle.Height));
        }

        public static double GetDistanceTo(this Point thisPoint, Point other)
        {
            return Math.Sqrt((thisPoint.X - other.X) * (thisPoint.X - other.X) -
                             (thisPoint.Y - other.Y) * (thisPoint.Y - other.Y));
        }

        public static double GetRadius(this CircularCloudLayouter layouter)
        {
            return layouter.Rectangles.SelectMany(r => r.GetVertices()).Select(p => layouter.Center.GetDistanceTo(p))
                .Max();
        }
    }
}