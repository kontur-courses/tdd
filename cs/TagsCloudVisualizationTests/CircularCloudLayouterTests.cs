using NUnit.Framework;
using FluentAssertions;
using System.Drawing;
using TagsCloudVisualization;
using System;
using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    class CircularCloudLayouterTests
    {
        private CircularCloudLayouter layouter;
        private const int X = 500;
        private const int Y = 500;
        private const int COUNT = 100;

        [SetUp]
        public void SetUp()
        {
            layouter = CircularCloudLayouter
                .Generate(new Point(X, Y), COUNT);
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var path = LayoutImageGenerator.CreateImage(layouter);
                Console.WriteLine($"Tag cloud visualization saved to file {path}");
            }
        }

        [Test]
        public void Should_HaveCenter_AfterCreation()
        {
            layouter.Center
                .Should().Be(new Point(500, 500));
        }

        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        public void Should_Throw_When_TryingToPutNegativeSize(int width, int height)
        {
            FluentActions.Invoking(
                () => layouter.PutNextRectangle(new Size(width, height)))
                .Should().Throw<ArgumentException>();
        }

        [Test]
        public void Should_ReturnRectangleWithSameSize_AfterPutNextRectangle()
        {
            layouter.PutNextRectangle(new Size(50, 50))
                .Size
                .Should()
                .BeEquivalentTo(new Size(50, 50));
        }

        [Test]
        public void Should_ContainAllAddedRectangles()
        {
            var sizes = new List<Size>();
            layouter = new CircularCloudLayouter(new Point(X, Y));

            for (var i = 1; i <= 5; i++)
            {
                var size = new Size(i * 10, i * 5);
                sizes.Add(size);
                layouter.PutNextRectangle(size);
            }

            layouter.Rectangles
                .Select(rect => rect.Size)
                .Should()
                .BeEquivalentTo(sizes);
        }

        [Test]
        public void Should_NotContain_IntersectedRectangles()
        {
            layouter.Rectangles
                .Should()
                .OnlyContain(rect =>
                    layouter.Rectangles
                    .All(other => !other.IntersectsWith(rect)
                    || other == rect));
        }
    }
}
