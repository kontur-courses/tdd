using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Drawing;
using FluentAssertions;

namespace TagsCloudVisualization
{
    [TestFixture]
    class CircularCloudLayouterTests
    {
        CircularCloudLayouter CircularCloudLayouter;

        [SetUp]
        public void SetUp()
        {
            var center = new Point(1, 3);
            CircularCloudLayouter = new CircularCloudLayouter(center);
        }

        public List<(Size size, Point location)> SizeGenerator() => new List<(Size size, Point location)>
        {
            (new Size(2, 4), new Point(1, 3)),
            (new Size(3, 4), new Point(-2, 3)),
            (new Size(2, 2), new Point(-1, 1)),
            (new Size(3, 3), new Point(1, 0)),
            (new Size(1, 1), new Point(3, 3)),
            (new Size(2, 2), new Point(-1, -1)),
            (new Size(3, 2), new Point(-4, 1)),
        };

        [Test]
        public void Should_ReturnCenterPoint_When_FirstRectangleAdded()
        {
            var size = new Size(2, 4);
            CircularCloudLayouter.PutNextRectangle(size).Location.Should().Be(new Point(1, 3));
        }

        [Test]
        public void Should_ReturnPositionClosestToCenter()
        {
            var sizesWithLocations = SizeGenerator();
            sizesWithLocations
                .Take(4)
                .ToList()
                .ForEach(b => CircularCloudLayouter.PutNextRectangle(b.size).Location
                                                                              .Should()
                                                                              .Be(b.location));
        }

        [TestCase(4)]
        [TestCase(10)]
        [TestCase(15)]
        public void RectanglesShouldNotIntersectWithEachOther(int rectanglesCount)
        {
            var random = new Random();
            var rectangles = Enumerable
                .Range(0, rectanglesCount)
                .Select(r => CircularCloudLayouter.PutNextRectangle(new Size(random.Next(1, 100), random.Next(1, 100))))
                .ToList();

            rectangles
                .ForEach(rec1 => rectangles
                                .Where(rec2 => rec2 != rec1)
                                .ToList()
                                .ForEach(rec2 => rec2.IntersectsWith(rec1).Should().BeFalse()));
        }

        [Test]
        public void Should_ReturnPositionClosestToCenter_When_CanNotBeConnectedWithCenter()
        {
            var sizesWithLocations = SizeGenerator();
            sizesWithLocations
                .ForEach(b => CircularCloudLayouter.PutNextRectangle(b.size).Location
                                                                              .Should()
                                                                              .Be(b.location));
        }
    }

}
