using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
    [TestFixture]
    class CircularCloudLayouterTests
    {
        private Point center;
        private CircularCloudLayouter layout;
        private Size defaultSize;

        [SetUp]
        public void SetUp()
        {
            center = new Point(0,0);
            layout = new CircularCloudLayouter(center);
            defaultSize = new Size(100, 50);
        }

        [Test]
        public void CircularCloudLayouter_CreateEmptyLayout_EmptyLayout()
        {
            layout.Center.Should().Be(center);
            layout.Rectangles.Count.Should().Be(0);
        }

        [Test]
        public void PutNextRectangle_PutOneRectangle_LeftTopRectangleCornerShouldBeCenterLayoutPoint()
        {
            var rectangle = layout.PutNextRectangle(defaultSize);
            rectangle.ShouldBeEquivalentTo(new Rectangle(center, defaultSize));
        }

        [Test]
        public void PutNextRectangle_PutOneRectangle_AddToListOfRectangles()
        {
            layout.PutNextRectangle(defaultSize);
            layout.Rectangles.Count.Should().Be(1);
        }

        [Test]
        public void PutNextRectangle_PutTwoRectangles_RectanglesDoNotIntersect()
        {
            var firstRectangle = layout.PutNextRectangle(defaultSize);
            var secondRectangle = layout.PutNextRectangle(new Size(80, 40));
            firstRectangle.IntersectsWith(secondRectangle).Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_PutMultipleRectangles_RectanglesDoNotIntersect()
        {
            var random = new Random();
            var rectangles = new List<Rectangle>();

            for (var i = 0; i < 1000; i++)
            {
                var randomSize = new Size(random.Next(200), random.Next(100));
                var newRectangle = layout.PutNextRectangle(randomSize);
                rectangles.ForEach(rect => rect.IntersectsWith(newRectangle).Should().BeFalse());
                rectangles.Add(newRectangle);
            }
        }

    }
}
