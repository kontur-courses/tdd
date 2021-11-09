using NUnit.Framework;
using FluentAssertions;
using System.Drawing;
using TagsCloudVisualization;
using System;
using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Extensions;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    internal class CircularCloudLayouterTests
    {
        private CircularCloudLayouter layouter;
        private const int X = 500;
        private const int Y = 500;
        private const int Count = 1000;

        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(new Point(X, Y));
        }

        [TearDown]
        public void TearDown()
        {
            if (layouter.Rectangles.Count > 0 
                && TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var path = LayoutImageGenerator.CreateImage(layouter);
                Console.WriteLine($"Tag cloud visualization saved to file {path}");
            }
        }

        [Test]
        public void Should_HaveCenter_AfterCreation()
        {
            layouter.Center
                .Should().Be(new Point(X, Y));
        }

        [Test]
        public void Should_HaveNoRectangles_AfterCreation()
        {
            layouter.Rectangles.Should().BeEmpty();
        }

        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        [TestCase(-1, -1)]
        [TestCase(0, -0)]
        [TestCase(10, 0)]
        [TestCase(-10, 10)]
        public void Should_Throw_When_TryingToPutIncorrectSize(int width, int height)
        {
            FluentActions.Invoking(
                () => layouter.PutNextRectangle(new Size(width, height)))
                .Should().Throw<ArgumentException>();
        }

        [Test]
        public void Should_ReturnRectangleWithSameSize_AfterPutNextRectangle()
        {
            var sizeToAdd = new Size(X, Y);
            layouter.PutNextRectangle(sizeToAdd).Size
                .Should().Be(sizeToAdd);
        }

        [Test]
        public void Should_ContainSameNumberOfRectanglesAsWerePut()
        {
            layouter.PutManyRectangles(Count);
            layouter.Rectangles.Count.Should().Be(Count);
        }

        [Test]
        public void Should_ContainAllAddedRectangles()
        {
            var sizes = new List<Size>();

            for (var i = 1; i <= 5; i++)
            {
                var size = new Size(X, Y);
                sizes.Add(size);
                layouter.PutNextRectangle(size);
            }

            layouter.Rectangles
                .Select(rect => rect.Size)
                .Should()
                .BeEquivalentTo(sizes);
        }

        [Test]
        public void Should_NotContain_TwoEqualRectangles()
        {
            layouter.PutManyRectangles(Count);
            var rectangles = layouter.Rectangles.ToArray();

            for (var i = 0; i < rectangles.Length; i++)
                for (var j = 0; j < rectangles.Length; j++)
                {
                    if (i != j)
                        rectangles[i].Should().NotBe(rectangles[j]);
                }
        }

        [Test]
        public void Should_NotContain_IntersectedRectangles()
        {
            layouter.PutManyRectangles(Count);

            layouter.Rectangles
                .Should()
                .OnlyContain(rect =>
                    layouter.Rectangles
                    .All(other => !other.IntersectsWith(rect)
                    || other == rect));
        }

        [Test]
        public void Should_PutManyRectangles_Fast()
        {
            Action action = () => layouter.PutManyRectangles(1000);
            action.ExecutionTime().Should().BeLessThan(500.Milliseconds());
        }

        [Test]
        public void Should_CreateTightLayout()
        {
            layouter.PutManyRectangles(Count);
            var radius = layouter.CalculateLayoutRadius();
            var circleArea = Math.PI * radius * radius;
            var layoutArea = layouter.Rectangles
                .Select(rectangle => rectangle.Height * rectangle.Width)
                .Sum();

            circleArea.Should().BeInRange(layoutArea, 1.25 * layoutArea);
        }
    }
}
