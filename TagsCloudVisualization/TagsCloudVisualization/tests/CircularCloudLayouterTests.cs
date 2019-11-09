using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization.tests
{
    [TestFixture]
    class CircularCloudLayouterTests
    {
        private CircularCloudLayouter _circularCloudLayouter;
        private List<Rectangle>_rectangles = new List<Rectangle>();
        private Point _center;
        private readonly Size[] _sizesForTesting = {
            new Size(2, 4),
            new Size(3, 4),
            new Size(2, 2),
            new Size(3, 3),
            new Size(1, 1),
            new Size(2, 2),
            new Size(3, 2)
        };
        private readonly Point[] _expectedLocations = {
            new Point(300, 300),
            new Point(297, 300),
            new Point(298, 298),
            new Point(300, 297),
            new Point(302, 300),
            new Point(298, 296),
            new Point(295, 298)
        };

        [SetUp]
        public void SetUp()
        {
            _center = new Point(300, 300);
            _circularCloudLayouter = new CircularCloudLayouter(_center);
        }

        [Test]
        public void Should_LocateTopLeftCornerOfFirstRectangleInCenter()
        {
            var size = new Size(2, 4);
            _circularCloudLayouter.PutNextRectangle(size).Location.Should().Be(new Point(300, 300));
        }

        [Test]
        public void Should_ReturnPositionClosestToCenter()
        {
            _sizesForTesting
                .Take(4)
                .ToList()
                .Select(size => _circularCloudLayouter.PutNextRectangle(size).Location)
                .Should()
                .BeEquivalentTo(_expectedLocations.Take((4)));
        }

        [Test]
        public void Should_ThrowException_When_SideLessOrEqualZero()
        {
            Func<Rectangle> act = () => _circularCloudLayouter.PutNextRectangle(new Size(-1 ,0));
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void CloudShouldBeDensity()
        {
            _rectangles = _sizesForTesting.Select(size => _circularCloudLayouter.PutNextRectangle(size)).ToList();
            double allRectanglesSquare = _rectangles.Select(rectangle => rectangle.Width*rectangle.Height).Sum();

            var xMinRadius = Math.Abs(_rectangles.Select(rectangle => rectangle.GetCorners()
                                                            .Select(point => point.X).Min()).Min() - _center.X);
            var xMaxRadius = Math.Abs(_rectangles.Select(rectangle => rectangle.GetCorners()
                                                            .Select(point => point.X).Max()).Max() - _center.X);
            var yMinRadius = Math.Abs(_rectangles.Select(rectangle => rectangle.GetCorners()
                                                            .Select(point => point.Y).Min()).Min() - _center.Y);
            var yMaxRadius = Math.Abs(_rectangles.Select(rectangle => rectangle.GetCorners()
                                                            .Select(point => point.Y).Max()).Max() - _center.Y);

            var radius = (xMinRadius + xMaxRadius + yMinRadius + yMaxRadius) / 4;
            var circleSquare = Math.PI * radius * radius;
            allRectanglesSquare.Should().BeGreaterOrEqualTo(80 * circleSquare / 100);
        }

        [Test]
        public void FigureFormShouldBeCircle()
        {

        }

        [Test]
        public void RectanglesShouldNotIntersectWithEachOther()
        {
            _rectangles = _sizesForTesting
                .Select(size => _circularCloudLayouter.PutNextRectangle(size))
                .ToList();

            _rectangles
                .ForEach(rec1 => _rectangles
                                .Where(rec2 => rec2 != rec1)
                                .ToList()
                                .ForEach(rec2 => rec2.IntersectsWith(rec1).Should().BeFalse()));
        }

        [Test]
        public void Should_ReturnPositionClosestToCenter_When_CanNotBeConnectedWithCenter()
        {
            _sizesForTesting
                .Select(size => _circularCloudLayouter.PutNextRectangle(size).Location)
                .Should()
                .BeEquivalentTo(_expectedLocations);
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var image = new DebugVisualization().DrawRectangles(_rectangles);
                image.Save("debug.png");
                Console.WriteLine($"Tag cloud visualization saved to file {Path.GetFullPath("debug.png")}");
            }
            _rectangles.Clear();
        }
    }
}
