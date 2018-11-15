using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_should
    {
        private CircularCloudLayouter circularCloudLayouter;

        [SetUp]
        public void SetUp()
        {
            circularCloudLayouter = new CircularCloudLayouter(new Point(0, 0));
        }

        [TestCase(0, 0, 100, 100, TestName = "center equal (0,0)")]
        [TestCase(-10, 40, 100, 100, TestName = "center not equal (0,0)")]
        public void PutNextRectangle_InCenter_WhenRectangleFirstAnd(int centerX, int centerY, 
            int rectangleWidth, int rectangleHeight)
        {
            var center = new Point(centerX, centerY);
            circularCloudLayouter = new CircularCloudLayouter(center);

            var rectangleSize = new Size(rectangleWidth, rectangleHeight);
            var leftTopLocation = new Point(
                (int)Math.Ceiling(center.X - rectangleSize.Width / 2d),
                (int)Math.Ceiling(center.Y - rectangleSize.Height / 2d)
            );
            var expectedLocation = new Rectangle(leftTopLocation, rectangleSize);

            var rectanglLocation = circularCloudLayouter.PutNextRectangle(rectangleSize);

            rectanglLocation.Should().Be(expectedLocation);
        }

        [Test]
        public void PutNextRectangle_WithoutIntersectionsWithPastRectangles()
        {
            var firstRectangleSize = new Size(100, 100);
            var secondRectangleSize = new Size(100, 100);

            var firstRectangleLocation = circularCloudLayouter
                .PutNextRectangle(firstRectangleSize);
            var secondRectangleLocation = circularCloudLayouter
                .PutNextRectangle(secondRectangleSize);

            firstRectangleLocation
                .IntersectsWith(secondRectangleLocation)
                .Should().BeFalse();
        }
    }
}
