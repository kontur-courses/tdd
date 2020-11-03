using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter _circularCloudLayouter;
        private Point _center;

        [SetUp] 
        public void Initialize()
        {
            _center = new Point(50, 50);
            _circularCloudLayouter = new CircularCloudLayouter(_center);
        }

        [TestCase(-10, 10)]
        [TestCase(10, -10)]
        [TestCase(-10, -10)]
        public void CircularCloudLayouter_ThrowsException(int x, int y)
        {
            Action create = () => new CircularCloudLayouter(new Point(x, y));
            create.Should().Throw<ArgumentException>();
        }

        [TestCase(-10, 10)]
        [TestCase(10, -10)]
        [TestCase(-10, -10)]
        [TestCase(10, 0)]
        [TestCase(0, 10)]
        [TestCase(0, 0)]
        public void PutNextRectangle_ThrowsException(int width, int height)
        {
            Action create = () => _circularCloudLayouter.PutNextRectangle(new Size(width, height));
            create.Should().Throw<ArgumentException>();
        }

        [TestCase(4, 4)]
        [TestCase(7, 5)]
        public void PutNextRectangle_PlaceFirstRectangleInCenter(int width, int height)
        {
            _circularCloudLayouter.PutNextRectangle(new Size(width, height));
            _circularCloudLayouter.GetRectangles()[0].Location.Should()
                .Be(new Point(_center.X - width/2, _center.Y - height/2));
        }


        [Test]
        public void CircularCloudLayouter_DoesNotContainsAnyRectangles_AfterCreating()
        {
            _circularCloudLayouter.GetRectangles().Should().BeEmpty();
        }

        [Test]
        public void CircularCloudLayouter_ContainsManyRectangles_AfterAdding()
        {
            for (var i = 0; i < 10; i++)
                _circularCloudLayouter.PutNextRectangle(new Size(10, 10));
            _circularCloudLayouter.GetRectangles().Should().HaveCount(10);
        }

        [Test]
        public void CircularCloudLayouter_ContainsCorrectRectangle_AfterAdding()
        {
            _circularCloudLayouter.PutNextRectangle(new Size(10, 10));
            _circularCloudLayouter.GetRectangles()[0].Size.Should().Be(new Size(10, 10));
        }

        [Test]
        public void Rectangles_DoNotIntersectEachOther()
        {
            for (var i = 0; i < 10; i++)
                _circularCloudLayouter.PutNextRectangle(new Size(5, 2));

            var rectangles = _circularCloudLayouter.GetRectangles();
            for (var j = 0; j < 9; j++)
            for (var i = j + 1; i < 10; i++)
            {
                rectangles[j].IntersectsWith(rectangles[i]).Should().BeFalse();
            }
        }

        [TestCase(3, 3)]
        [TestCase(5, 3)]
        public void Cloud_IsCloseToCircle(int width, int height)
        {
            for (var i = 0; i < 20; i++)
                _circularCloudLayouter.PutNextRectangle(new Size(width, height));

            var rectangles = _circularCloudLayouter.GetRectangles();
            var leftRadius = rectangles.Select(r => Math.Abs(r.Left)).Max();
            var rightRadius = rectangles.Select(r => Math.Abs(r.Right)).Max();
            var topRadius = rectangles.Select(r => Math.Abs(r.Left)).Max();
            var bottomRadius = rectangles.Select(r => Math.Abs(r.Right)).Max();

            leftRadius.Should().BeCloseTo(rightRadius, (uint)rightRadius / 10);
            topRadius.Should().BeCloseTo(bottomRadius, (uint)bottomRadius / 10);
        }
    }
}
