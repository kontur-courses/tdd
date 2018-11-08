using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        [Test]
        public void Cloud_HaveCenter_ItWasCreatedWith()
        {
            var cloud = new CircularCloudLayouter(new Point(-5, 6));
            cloud.Center.Should().Be(new Point(-5, 6));
        }

        [Test]
        public void Cloud_HaveNoRectangle_BeforeAnyWasPut()
        {
            var cloud = new CircularCloudLayouter(new Point());
            cloud.Rectangles.Should().BeEmpty();
        }

        [TestCase(-1, 1, TestName = "Negative width")]
        [TestCase(1, -1, TestName = "Negative height")]
        [TestCase(0, 1, TestName = "Zero width")]
        [TestCase(1, 0, TestName = "Zero height")]
        public void Constructor_ThrowsException_OnWrongArguments(int width, int height)
        {
            var cloud = new CircularCloudLayouter(new Point());
            Action act = () => cloud.PutNextRectangle(new Size(width, height));
            act.ShouldThrow<ArgumentException>();
        }

        [TestCase(4, 2, TestName = "With even size")]
        [TestCase(1, 3, TestName = "With odd size")]
        public void PutNextRectangle_ReturnsCenterRectangle_OnFirstPut(int width, int height)
        {
            var center = new Point(1, -4);
            var cloud = new CircularCloudLayouter(center);
            var rectangleSize = new Size(width, height);
            var expectedLocation = new Point(center.X - width / 2, center.Y - height / 2);
            cloud.PutNextRectangle(rectangleSize).Location.Should().Be(expectedLocation);
        }

        [Test]
        public void TwoPuttedRectangles_HaveDifferentLocations()
        {
            var center = new Point(0, 0);
            var cloud = new CircularCloudLayouter(center);
            var rect1 = cloud.PutNextRectangle(new Size(4, 5));
            var rect2 = cloud.PutNextRectangle(new Size(8, 3));
            rect1.Location.Should().NotBe(rect2.Location);
        }

        [Test]
        public void TwoPuttedRectangles_DoNotIntersect()
        {
            var center = new Point(0, 0);
            var cloud = new CircularCloudLayouter(center);
            var rect1 = cloud.PutNextRectangle(new Size(4, 5));
            var rect2 = cloud.PutNextRectangle(new Size(8, 3));
            rect1.IntersectsWith(rect2).Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_ReturnsNotIntersectingRectangles_On100RandomSizes()
        {
            var center = new Point(0, 0);
            var cloud = new CircularCloudLayouter(center);
            var random = new Random();
            for (var i = 0; i < 100; i++)
            {
                var width = random.Next(1, 11);
                var height = random.Next(1, 11);
                var rectangle = cloud.PutNextRectangle(new Size(width, height));
                rectangle.IntersectsWithAny(cloud.Rectangles.Where(rect=>!rect.Equals(rectangle))).Should().BeFalse();
            }
        }

        [Test]
        public void PutNextRectangle_PutsRectanglesInCircleShape()
        {
            var center = new Point(0, 0);
            var random = new Random();
            var cloud = new CircularCloudLayouter(center);
            var maxDistanceToRectangle = 0.0;
            var actualSumArea = 0;
            for (var i = 0; i < 100; i++)
            {
                var rectangle = cloud.PutNextRectangle(new Size(random.Next(1,11), random.Next(1, 11)));
                maxDistanceToRectangle = Math.Max(maxDistanceToRectangle, rectangle.CountMaxDistanceTo(center));
                actualSumArea += rectangle.Width * rectangle.Height;
            }

            var circleArea = maxDistanceToRectangle*maxDistanceToRectangle * Math.PI;
            var ratio = actualSumArea / circleArea;
            ratio.Should().BeGreaterThan(0.5);

        } 

    }
}