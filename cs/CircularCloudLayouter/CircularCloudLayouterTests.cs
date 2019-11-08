using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization

{
    [TestFixture]
    class ArchitectureOfTagsCloudShould
    {
        private CircularCloudLayouter basicLayouter;

        [SetUp]
        public void SetUp()
        {
            basicLayouter = new CircularCloudLayouter(new Point(100, 100));
        }

        [TestCase(0, 0)]
        public void ShouldHaveCorrectCenterCoordinates_When_Create(int x, int y)
        {
            var circularLayouter = new CircularCloudLayouter(new Point(x, y));
            circularLayouter.Center.Should().Be(new Point(x, y));

        }

        [Test]
        public void Layouter_Should_HaveRectanglesList_WhenCreated()
        {
            basicLayouter.Rectangles.Should().NotBeNull();
        }

        [TestCase(0, 10, TestName = "WhenWidthIsZero")]
        [TestCase(10, 0, TestName = "WhenHeightIsZero")]
        [TestCase(0, 0, TestName = "WhenWidthAndHeightIsZero")]
        [TestCase(-1, 10, TestName = "WhenWidthIsNegative")]
        [TestCase(10, -1, TestName = "WhenHeightIsNegative")]
        [TestCase(-2, -1, TestName = "WhenWidthAndHeightIsNegative")]
        public void ShouldThrowsArgumentException_When_PuttedRectangleHaveIncorrectDimension(int w, int h)
        {
            Action action = () => basicLayouter.PutNextRectangle(new Size(w, h));
            action.Should().Throw<ArgumentException>();
        }

        [TestCase(20, 20, -10, -10)]
        [TestCase(13, 13, -6, -6)]
        public void GetCenterOfFirstRectangle(int w, int h, int x, int y)
        {
            basicLayouter.PutNextRectangle(new Size(w, h)).Location.Should().Be(new Point(100 + x, 100 + y));
        }

        [Test]
        public void AllRectangles_Should_BeInRectanglesList()
        {
            Random random = new Random();
            for (int i = 0; i < 50; i++)
            {
                basicLayouter.PutNextRectangle(new Size(random.Next(1, 50), random.Next(1, 50)));
            }

            basicLayouter.Rectangles.Count.Should().Be(50);
        }

        [Test]
        [TestCaseSource(typeof(LayouterSource), "TestCases")]
        public void Rectangles_ShouldNot_Intersect(CircularCloudLayouter layouter)
        {
            layouter.Rectangles
                .Select(first =>
                    layouter.Rectangles.Any(second => second != first && first.IntersectsWith(second)))
                .Any(isIntersect => isIntersect).Should().BeFalse();
        }

        public static class LayouterSource
        {
            private static readonly CircularCloudLayouter LayouterWithSameSizeRectangles =
                LayouterWithTheSameSizeRectangles();
            private static readonly CircularCloudLayouter LayouterWithDifferentSizeRectangles =
                LayouterWithTheDifferentSizeRectangles();

            public static CircularCloudLayouter LayouterWithTheSameSizeRectangles()
            {
                var layouter = new CircularCloudLayouter(new Point(100, 100));
                for (int i = 0; i < 50; i++)
                    layouter.PutNextRectangle(new Size(5, 5));
                return layouter;
            }

            public static CircularCloudLayouter LayouterWithTheDifferentSizeRectangles()
            {
                var layouter = new CircularCloudLayouter(new Point(100, 100));
                Random random = new Random();
                for (int i = 0; i < 50; i++)
                    layouter.PutNextRectangle(new Size(random.Next(1, 10), random.Next(1, 10)));
                return layouter;
            }

            private static readonly CircularCloudLayouter[] TestCases =
                {LayouterWithSameSizeRectangles, LayouterWithDifferentSizeRectangles};
        }
    }
}
