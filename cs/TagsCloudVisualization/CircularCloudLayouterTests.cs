using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter layouter;
        private Point center;

        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(center);
            center = Point.Empty;
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed)
                return;
            if (!Directory.Exists("Failed"))
                Directory.CreateDirectory("Failed");
            var filepath = $"{Environment.CurrentDirectory}\\Failed\\{TestContext.CurrentContext.Test.Name}.png";
            RectanglePainter.SaveToFile(filepath, layouter.Rectangles);
            Console.WriteLine($"Tag cloud visualization saved to file {filepath}");
        }

        [Test]
        public void SaveCenterAfterCreation()
        {
            layouter.Center.Should().Be(center);
        }

        [Test]
        public void NoRectanglesAfterCreation()
        {
            layouter.Rectangles.Should().BeEmpty();
        }

        [TestCase(0, 0)]
        [TestCase(-1, -1)]
        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        public void PutNextRectangle_ThrowArgumentException(int width, int height)
        {
            var size = new Size(width, height);
            Action act = () => layouter.PutNextRectangle(size);

            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void PutNextRectangle_CenterLocation_IfPutFirstRectangle()
        {
            var size = new Size(1, 1);

            layouter.PutNextRectangle(size).Location
                .Should().Be(center);
        }

        [Test]
        public void PutNextRectangle_DoesNotChangeSize()
        {
            var size = new Size(1, 1);

            layouter.PutNextRectangle(size).Size
                .Should().Be(size);
        }

        [TestCase(2)]
        [TestCase(5)]
        [TestCase(15)]
        public void SaveRectangleAfterPutNextRectangle(int count)
        {
            var rectSizes = Enumerable.Range(1, count)
                .Select(x => new Size(95 * x, 50 * x))
                .ToHashSet();

            foreach (var size in rectSizes)
                layouter.PutNextRectangle(size);

            layouter.Rectangles.Select(x => x.Size)
                .Should().HaveCount(rectSizes.Count).And
                .OnlyContain(x => rectSizes.Contains(x));
        }

        [TestCase(2)]
        [TestCase(3)]
        [TestCase(30)]
        public void RectanglesShouldNotBeIntersected(int count)
        {
            var rectSizes = Enumerable.Range(1, count)
                .Select(x => new Size(100 * x, 100 * x));
            var rects = new List<Rectangle>();

            foreach (var size in rectSizes)
                rects.Add(layouter.PutNextRectangle(size));

            Enumerable.Range(0, count - 1)
                .Select(idx => rects.Take(idx).IntersectsWith(rects[idx + 1]))
                .Any(x => x)
                .Should().BeFalse();
        }
    }
}