using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    [TestFixture]
    internal class TagsCloudTests
    {
        private CircularCloudLayouter layouter;

        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(new Point(500, 500));
        }

        [TearDown]
        public void CreateImageOnFail()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var path = $"{Directory.GetCurrentDirectory()}\\TestOutput\\{TestContext.CurrentContext.Test.Name}.png";
                var bmp = TagCloudVisualizer.Visualize(layouter, new Size(1000, 1000));
                bmp.Save(path, ImageFormat.Png);
                TestContext.WriteLine($"Tag cloud visualization saved to file {path}");
            }
        }

        [TestCase(1, 0)]
        [TestCase(0, 1)]
        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        public void PutNextRectangle_ThrowsArgumentException_IfRectangleSizeHasNonPositiveParameter(int width,
            int height)
        {
            Func<Rectangle> act = () => layouter.PutNextRectangle(new Size(width, height));
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void PutNextRectange_ReturnsRectangleWithPositionShiftedByOffsets()
        {
            var recSize = new Size(10, 10);
            var expectedShiftedCenter =
                new Point(layouter.Center.X - recSize.Width / 2, layouter.Center.Y - recSize.Height / 2);
            var rect = layouter.PutNextRectangle(recSize);
            new Point(rect.X, rect.Y).Should().Be(expectedShiftedCenter);
        }

        [TestCase(1, 1)]
        [TestCase(42, 19)]
        [TestCase(1000, 1000)]
        public void PutNextRectangle_ReturnsRectangleWithCorrectSize(int width, int height)
        {
            var rectSize = new Size(width, height);
            layouter.PutNextRectangle(rectSize).Size.Should().Be(rectSize);
        }

        [TestCase(0, 0)]
        [TestCase(42, 38)]
        [TestCase(-10000, -500)]
        public void PutNextRectangle_FirstRectangleIsOnCenter_WhenLayouterHasCustomCenterPoint(int x, int y)
        {
            var center = new Point(x, y);
            var customLayouter = new CircularCloudLayouter(center);
            var rect = customLayouter.PutNextRectangle(new Size(10, 10));
            rect.X.Should().Be(center.X - rect.Width / 2);
            rect.Y.Should().Be(center.Y - rect.Height / 2);
        }

        [TestCase(2)]
        [TestCase(10)]
        [TestCase(100)]
        public void PutNextRectangles_ShouldNotReturnCrossingRectangles_AfterPassingSetOfRectangleSizes(
            int rectanglesAmount)
        {
            var rectangles = new List<Rectangle>();
            for (var i = 1; i < rectanglesAmount; i++)
                rectangles.Add(layouter.PutNextRectangle(new Size(i, i)));

            for (var i = 0; i < rectangles.Count; i++)
            for (var j = 0; j < i; j++)
                rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_ReturnsRectangleCloseToCenter_IfSmallRectangleIsAfterLargeRectangle()
        {
            var centerRect = layouter.PutNextRectangle(new Size(5, 5));
            layouter.PutNextRectangle(new Size(100, 100));
            var smallRect = layouter.PutNextRectangle(new Size(5, 5));

            var distance = Math.Sqrt(Math.Pow(smallRect.X - centerRect.X, 2) + Math.Pow(smallRect.Y - centerRect.Y, 2));

            distance.Should().BeLessThan(10);
        }

        [TestCase(100)]
        [TestCase(500)]
        [TestCase(1000)]
        public void PutNextRectangle_IsTimePermissible_OnBigNumberOfRectangles(int rectanglesAmount)
        {
            var rnd = new Random();
            Action action = () =>
            {
                for (var i = 0; i < rectanglesAmount; i++)
                    layouter.PutNextRectangle(new Size(5 + rnd.Next(40), 5 + rnd.Next(40)));
            };
            action.ExecutionTime().Should().BeLessThan((10 * rectanglesAmount).Milliseconds());
        }
    }
}