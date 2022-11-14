using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    public class CircularCloudLayouterTests
    {
        private static CircularCloudLayouter layouterUnderTesting;

        [SetUp]
        public void SetUp()
        {
            layouterUnderTesting = null;
        }
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

            layouterUnderTesting = new CircularCloudLayouter(new Point(0, 0));
            var rectangle = layouterUnderTesting.PutNextRectangle(size);

            rectangle.Size.Should().BeEquivalentTo(size);
        }

        [Test]
        public void Layouter_DoesntOverlapAnyRectangles()
        {
            var rectanglesCount = 1000;
            var sizes = RectangleSizeProvider.GetRandomSizes(1000, rectanglesCount, 1000);

            layouterUnderTesting = new CircularCloudLayouter(new Point(0, 0));
            foreach (var size in sizes)
                layouterUnderTesting.PutNextRectangle(size);

            for (var i = 0; i < rectanglesCount; i++)
            for (var j = i + 1; j < rectanglesCount; j++)
                layouterUnderTesting.Rectangles[i]
                    .IntersectsWith(layouterUnderTesting.Rectangles[j])
                    .Should().Be(false);
        }

        [TestCaseSource(nameof(DensityTestData))]
        public void Layouter_LaysOutDenserThanThreshold(double density, IEnumerable<Size> sizes)
        {
            var desiredDensity = 0.5;
            var center = new Point(0, 0);

            layouterUnderTesting = new CircularCloudLayouter(center);
            foreach (var size in sizes)
                layouterUnderTesting.PutNextRectangle(size);
            var radius = layouterUnderTesting.GetCoveringCircleRadius();
            var circleSquare = Math.PI * radius * radius;
            var rectanglesSquare = layouterUnderTesting.Rectangles.Sum(r => r.Height * r.Width);

            (rectanglesSquare / circleSquare).Should().BeGreaterThan(desiredDensity);
        }
        

        [TearDown]
        public void TearDown()
        {
            var currentContext = TestContext.CurrentContext;
            var testNotPassed = currentContext.Result.Assertions
                .Any(a => a.Status is AssertionStatus.Error or AssertionStatus.Failed);

            if (testNotPassed)
            {
                var visualizer = LayoutVisualizer.FromCircularCloudLayouter(layouterUnderTesting);
                var fileName = Path.ChangeExtension(currentContext.Test.Name, "png");
                var visualizationPath = Path.Join(currentContext.WorkDirectory, fileName) ;
                visualizer.SaveAs(visualizationPath);
                Console.WriteLine($"Tag cloud visualization saved to file \"{visualizationPath}\"");
            }
        }
    }
}