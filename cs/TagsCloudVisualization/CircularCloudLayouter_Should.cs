using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
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
                .Select((rectangle1, i) => Tuple.Create(rectangle1, rects.Skip(i + 1)));

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
            var distanceToFirst = GetDistance(relativeCenter, rects.First().Location);
            foreach (var rectangle in rects.Skip(1))
                GetDistance(relativeCenter, rectangle.Location).Should().BeGreaterOrEqualTo(distanceToFirst);
        }

        private IEnumerable<Rectangle> AssignLayouter(IEnumerable<Size> sizes) =>
            sizes.Select(size => layouter.PutNextRectangle(size));

        private double GetDistance(PointF point1, Point point2) =>
            Math.Sqrt((point1.X - point2.X) * (point1.X - point2.X) + (point1.Y - point2.Y) * (point1.Y - point2.Y));
    }
}