using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    public class CircularCloudLayouterTests
    {
        private static IEnumerable<TestCaseData> DensityTestData =>
            new[]
            {
                new TestCaseData(0.5, RectangleSizeProvider.GetRandomSizes(1000, 1000, 1000))
                    .SetName("1000 different sizes"),

                new TestCaseData(0.9, Enumerable.Repeat(new Size(4, 4), 1000))
                    .SetName("1000 identical squares")
            };

        [Test]
        public void PutNextRectangle_ReturnsRectangleWithSpecifiedSize()
        {
            var size = new Size(123, 456);

            var layouter = new CircularCloudLayouter(new Point(0, 0));
            var rectangle = layouter.PutNextRectangle(size);

            rectangle.Size.Should().BeEquivalentTo(size);
        }

        [Test]
        public void Layouter_DoesntOverlapAnyRectangles()
        {
            var rectanglesCount = 1000;
            var sizes = RectangleSizeProvider.GetRandomSizes(1000, rectanglesCount, 1000);

            var layouter = new CircularCloudLayouter(new Point(0, 0));
            foreach (var size in sizes)
                layouter.PutNextRectangle(size);

            for (var i = 0; i < rectanglesCount; i++)
            for (var j = i + 1; j < rectanglesCount; j++)
                layouter.Rectangles[i].IntersectsWith(layouter.Rectangles[j]).Should().Be(false);
        }

        [TestCaseSource(nameof(DensityTestData))]
        public void Layouter_LaysOutDenserThanThreshold(double density, IEnumerable<Size> sizes)
        {
            var desiredDensity = 0.5;
            var center = new Point(0, 0);

            var layouter = new CircularCloudLayouter(center);
            foreach (var size in sizes)
                layouter.PutNextRectangle(size);
            var radius = layouter.GetCoveringCircleRadius();
            var circleSquare = Math.PI * radius * radius;
            var rectanglesSquare = layouter.Rectangles.Sum(r => r.Height * r.Width);

            (rectanglesSquare / circleSquare).Should().BeGreaterThan(desiredDensity);
        }
    }
}