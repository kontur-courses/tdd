using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Test
    {
        private CircularCloudLayouter circularCloudLayouter;

        [SetUp]
        public void SetUp()
        {
            circularCloudLayouter = new CircularCloudLayouter(new Point(0, 0));
        }

        [Test]
        public void PutNextRectangle_Should_PutRectangleInTheCenter_When_FirstRectangle()
        {
            var rectangle = circularCloudLayouter.PutNextRectangle(new Size(30, 20));
            rectangle.Location.Should().Be(new Point(-15, -10));
        }

        [Test]
        public void PutNextRectangle_Should_NotIntersectRectangles_When_TwoRectangles()
        {
            var firstRectangle = circularCloudLayouter.PutNextRectangle(new Size(30, 20));
            var secondRectangle = circularCloudLayouter.PutNextRectangle(new Size(30, 20));
            secondRectangle.IntersectsWith(firstRectangle).Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_Should_NotIntersectRectangles_When_TwoRectanglesWithDifferentSizes()
        {
            var firstRectangle = circularCloudLayouter.PutNextRectangle(new Size(30, 20));
            var secondRectangle = circularCloudLayouter.PutNextRectangle(new Size(40, 80));
            secondRectangle.IntersectsWith(firstRectangle).Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_Should_ThrowArgumentException_When_SizeIsEmpty()
        {
            Following.Code(() => circularCloudLayouter.PutNextRectangle(Size.Empty)).ShouldThrow<ArgumentException>();
        }
    }

    public static class Following
    {
        public static Action Code(Action action)
        {
            return action;
        }
    }
}