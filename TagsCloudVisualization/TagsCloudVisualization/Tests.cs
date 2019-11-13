using NUnit.Framework;
using System;
using FluentAssertions;
using System.Drawing;
using System.Linq;

namespace Tests
{
    [TestFixture]
    class CircularCloudLayouterShould
    {
        [Test]
        public void Create_IfCorrectCentrePoint()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(2, 3));
            circularCloudLayouter.GetType().Name.Should().Be(nameof(CircularCloudLayouter));
        }

        [Test]
        [TestCase(-1, 3)]
        [TestCase(1, -3)]
        [TestCase(-3, -1)]
        public void ThrowArgumentExceptionInCreating_IfIncorrectCentrePoint(int x, int y)
        {
            Assert.Throws<ArgumentException>(() => new CircularCloudLayouter(new Point(x, y)));
        }

        [Test]
        public void PutAndReturnOneRectangle_IfAllPlaneIsFree()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(50, 50));

            var rectangle = circularCloudLayouter.PutNextRectangle(new Size(10, 10));

            rectangle.Should().NotBe(null);
        }

        [Test]
        public void PutOneRectangleInCenter_IfAllPlaneIsFree()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(50, 50));

            var rectangle = circularCloudLayouter.PutNextRectangle(new Size(7, 10));

            rectangle.Location.Should().BeEquivalentTo(new Point(47, 55));
        }

        [Test]
        public void ShiftFirstRectangleToRight_IfDoesNotFitInCenter()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(20, 50));

            var rectangle = circularCloudLayouter.PutNextRectangle(new Size(46, 10));

            rectangle.Location.Should().BeEquivalentTo(new Point(0, 55));
        }

        [Test]
        public void ShiftFirstRectangleToUp_IfDoesNotFitInCenter()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(50, 20));

            var rectangle = circularCloudLayouter.PutNextRectangle(new Size(10, 46));

            rectangle.Location.Should().BeEquivalentTo(new Point(45, 46));
        }

        [Test]
        public void PutTwoRectangles_OnFreePlane()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(50, 50));

            var rectangle1 = circularCloudLayouter.PutNextRectangle(new Size(10, 20));
            var rectangle2 = circularCloudLayouter.PutNextRectangle(new Size(13, 6));

            rectangle1.Should().NotBe(null);
            rectangle2.Should().NotBe(null);
        }

        [Test]
        public void PutTwoRectangles_AndTheyAreNotIntersected()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(50, 50));

            var rectangle1 = circularCloudLayouter.PutNextRectangle(new Size(10, 20));
            var rectangle2 = circularCloudLayouter.PutNextRectangle(new Size(50, 50));

            rectangle1.IntersectsWith(rectangle2).Should().Be(false);
        }

        [Test]
        public void PutThreeRectangles_AndTheyAreNotIntersected()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(50, 50));

            var rectangle1 = circularCloudLayouter.PutNextRectangle(new Size(10, 20));
            var rectangle2 = circularCloudLayouter.PutNextRectangle(new Size(50, 50));
            var rectangle3 = circularCloudLayouter.PutNextRectangle(new Size(20, 7));

            rectangle1.IntersectsWith(rectangle2).Should().Be(false);
            rectangle1.IntersectsWith(rectangle3).Should().Be(false);
            rectangle2.IntersectsWith(rectangle3).Should().Be(false);
        }

        [Test]
        public void PutManyRectangles_AndTheyAreNotIntersected()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(100, 100));
            var rectanglesCount = 3;
            var randomRange = 200;
            var random = new Random(randomRange);
            var sizes = new Size[rectanglesCount];
            for (var i = 0; i < rectanglesCount; i++)
                sizes[i] = new Size(1 + random.Next(randomRange), 1 + random.Next(randomRange));
            var rectangles = new Rectangle[rectanglesCount];

            for (var i = 0; i < rectanglesCount; i++)
                rectangles[i] = circularCloudLayouter.PutNextRectangle(sizes[i]);

            circularCloudLayouter.RectangleCount.Should().Be(rectanglesCount);
            var b = rectangles
                .SelectMany(r1 => rectangles.Select((r2) => r1.IntersectsWith(r2)));
            rectangles
                .SelectMany(r1 => rectangles.Select((r2) => r1 != r2 && r1.IntersectsWith(r2)))
                .Any(x => x)
                .Should()
                .BeFalse();
        }
    }
}