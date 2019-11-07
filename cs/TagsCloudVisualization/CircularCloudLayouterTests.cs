using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterConstructor_Should
    {
        [TestCase(0, 10, TestName = "Center x is zero")]
        [TestCase(10, 0, TestName = "Center y is zero")]
        [TestCase(-1, 10, TestName = "Center x is negative")]
        [TestCase(10, -1, TestName = "Center y is negative")]
        public void ThrowArgumentException_When(int x, int y)
        {
            Action action = () => new CircularCloudLayouter(new Point(x, y));

            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void CreateNewInstance_WhenCenterCoordinatesArePositive()
        {
            Action action = () => new CircularCloudLayouter(new Point(150, 250));

            action.Should().NotThrow<Exception>();
        }
    }

    [TestFixture]
    public class CircularCloudLayouterPutNextRectangle_Should
    {
        private CircularCloudLayouter circularCloudLayouter;
        private Point layouterCenter;

        [SetUp]
        public void Init()
        {
            layouterCenter = new Point(500, 500);
            circularCloudLayouter = new CircularCloudLayouter(layouterCenter);
        }

        [TestCase(0, 10, TestName = "Width x is zero")]
        [TestCase(10, 0, TestName = "Height y is zero")]
        [TestCase(-1, 10, TestName = "Width x is negative")]
        [TestCase(10, -1, TestName = "Height y is negative")]
        public void ThrowArgumentException_When(int width, int height)
        {
            Action action = () => circularCloudLayouter.PutNextRectangle(new Size(width, height));

            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void NotChangeRectangleSize()
        {
            var addedRectangle = circularCloudLayouter.PutNextRectangle(new Size(100, 200));

            addedRectangle.Size.Should().BeEquivalentTo(new Size(100, 200));
        }

        [Test]
        public void AddFirstRectangleInTheCloudCenter()
        {
            var addedRectangle = circularCloudLayouter.PutNextRectangle(new Size(100, 200));

            addedRectangle.Location.Should().BeEquivalentTo(layouterCenter);
        }

        [Test]
        public void AddNextRectangle_That_DoesntIntersectWithFirst()
        {
            var firstRectangle = circularCloudLayouter.PutNextRectangle(new Size(100, 200));
            var secondRectangle = circularCloudLayouter.PutNextRectangle(new Size(50, 100));

            secondRectangle.IntersectsWith(firstRectangle).Should().BeFalse();
        }

        [Test]
        public void AddMultipleRectangles_That_DontIntersectWithEachOther()
        {
            var checkList = new List<Rectangle>
            {
                circularCloudLayouter.PutNextRectangle(new Size(100, 200)),
                circularCloudLayouter.PutNextRectangle(new Size(130, 250)),
                circularCloudLayouter.PutNextRectangle(new Size(210, 160)),
                circularCloudLayouter.PutNextRectangle(new Size(120, 115))
            };

            checkList.Any(r1 => checkList.Any(r2 => r1.IntersectsWith(r2) && r1 != r2)).Should().BeFalse();
        }

        [Test]
        public void PlaceTwoRectanglesCloseToEachOther()
        {
            var acceptableXAxisShift = 50;
            var firstRectangle = circularCloudLayouter.PutNextRectangle(new Size(100, 100));
            var secondRectangle = circularCloudLayouter.PutNextRectangle(new Size(20, 102));

            secondRectangle.Y.Should().Be(firstRectangle.Top);
            secondRectangle.X.Should().BeInRange(firstRectangle.Left - acceptableXAxisShift,
                firstRectangle.Right + acceptableXAxisShift);
        }

        [Test]
        public void AddMultipleRectangles_That_FormACircleLikeShape()
        {
            var acceptableRatio = 60;
            var furthestDistance = 0d;
            var rectanglesSquare = 0d;

            var checkList = new List<Rectangle>
            {
                circularCloudLayouter.PutNextRectangle(new Size(100, 200)),
                circularCloudLayouter.PutNextRectangle(new Size(130, 250)),
                circularCloudLayouter.PutNextRectangle(new Size(210, 160)),
                circularCloudLayouter.PutNextRectangle(new Size(120, 115))
            };
            foreach (var rectangle in checkList)
            {
                var distance = GetDistanceBetweenRectangleAndPoint(rectangle, layouterCenter);
                if (distance > furthestDistance)
                    furthestDistance = distance;
                rectanglesSquare += rectangle.Width * rectangle.Height;
            }

            var circleSquare = furthestDistance * furthestDistance * Math.PI;
            var squareRatio = rectanglesSquare / circleSquare * 100;
            squareRatio.Should().Be(acceptableRatio);
        }

        private static double GetDistanceBetweenRectangleAndPoint(Rectangle rectangle, Point point)
        {
            var rectangleCentre = new Point(rectangle.Location.X + rectangle.Width / 2,
                rectangle.Location.Y + rectangle.Height / 2);

            return Math.Sqrt(Math.Pow(rectangleCentre.X - point.X, 2) + Math.Pow(rectangleCentre.Y - point.Y, 2));
        }
    }
}