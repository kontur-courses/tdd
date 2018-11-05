using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;


namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private Point center;
        private CircularCloudLayouter layouter;
        private readonly Size defaultSize = new Size(2, 4);

        [SetUp]
        public void SetUp()
        {
            center = new Point(0, 1);
            layouter = new CircularCloudLayouter(center);
        }

        [Test]
        public void HaveCenter_AfterCreation()
        {
            layouter.Center.Should().Be(center);
        }

        [Test]
        public void NotChangeCenter_AfterPuttingNewRectangle()
        {
            layouter.PutNextRectangle(defaultSize);
            layouter.Center.Should().Be(center);
        }

        [Test]
        public void ReturnSameSizeRectangleAsGiven_WhenPuttingNewRectangle()
        {
            var rect = layouter.PutNextRectangle(defaultSize);
            rect.Size.Should().Be(defaultSize);
        }

        [Test]
        public void PutFirstRectangleInCenter()
        {
            var rect = layouter.PutNextRectangle(defaultSize);
            rect.Location.Should().Be(center);
        }

        [Test]
        public void NotHaveIntersectedRectangles()
        {
            var rects = AssignLayouter(Enumerable.Repeat(defaultSize, 10)).ToArray();

            var rectPairs = rects
                .Select((rectangle, i) => (rectangle, rects.Skip(i + 1)));

            foreach (var rectPair in rectPairs)
                rectPair.Item1.IntersectsWithAnyFrom(rectPair.Item2).Should().BeFalse();
        }

        [Test]
        public void PlaceFirstRectInARelativeCenter()
        {
            var rects = AssignLayouter(Enumerable.Repeat(defaultSize, 10)).ToArray();
            var middleX = (float)rects.Sum(rect => rect.X) / rects.Length;
            var middleY = (float)rects.Sum(rect => rect.Y) / rects.Length;
            var relativeCenter = new PointF(middleX, middleY);
            var distanceToFirst = rects.First().Location.DistanceTo(relativeCenter);
            foreach (var rectangle in rects.Skip(1))
                rectangle.Location.DistanceTo(relativeCenter).Should().BeGreaterOrEqualTo(distanceToFirst);
        }

        private IEnumerable<Rectangle> AssignLayouter(IEnumerable<Size> sizes) =>
            sizes.Select(size => layouter.PutNextRectangle(size));
    }
}