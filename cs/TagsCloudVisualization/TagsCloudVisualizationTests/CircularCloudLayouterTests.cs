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

        private static IEnumerable<TestCaseData> DensityTestData =>
            new[]
            {
                new TestCaseData(0.5, RectangleSizeProvider.GetRandomSizes(1000, 1000, 100))
                    .SetName("1000 different sizes"),

                new TestCaseData(0.7, Enumerable.Repeat(new Size(4, 4), 1000))
                    .SetName("1000 identical squares"),

                new TestCaseData(0.25, RectangleSizeProvider.GetRandomWordLikeSizes(999, 100, 10,
                        50))
                    .SetName("100 word-like rectangles"),

                new TestCaseData(0.3, RectangleSizeProvider.GetRandomWordLikeSizes(999, 100, 10,
                        50, canBeVertical: false))
                    .SetName("100 horizontal word-like rectangles")
            };

        [SetUp]
        public void SetUp()
        {
            layouterUnderTesting = null;
        }

        [TestCase(0, 10)]
        [TestCase(10, 0)]
        [TestCase(-10, 10)]
        [TestCase(10, -10)]
        public void PutNextRectange_ThrowsArgOutOfRangeException_WhenInvalidDimension(int width, int height)
        {
            var size = new Size(width, height);

            var layouter = new CircularCloudLayouter(new Point(0, 0));
            Action act = () => layouter.PutNextRectangle(size);

            act.Should().Throw<ArgumentOutOfRangeException>();
        }

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
        public void Layouter_LaysOutDenserThanThreshold(double desiredDensity, IEnumerable<Size> sizes)
        {
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

            if (currentContext.Result.Outcome.Status is TestStatus.Failed)
            {
                var visualizer = LayoutVisualizer.FromCircularCloudLayouter(layouterUnderTesting);
                var fileName = Path.ChangeExtension(currentContext.Test.Name, "png");
                var visualizationPath = Path.Join(currentContext.WorkDirectory, fileName);
                visualizer.SaveAs(visualizationPath);
                Console.WriteLine($"Tag cloud visualization saved to file \"{visualizationPath}\"");
            }
        }
    }
}