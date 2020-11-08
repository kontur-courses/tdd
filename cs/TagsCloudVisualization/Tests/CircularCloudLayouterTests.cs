using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization.Tests
{
    class CircularCloudLayouterTests
    {
        private CircularCloudLayouter _layourter;
        private Point _canvasCenter;

        [SetUp]
        public void SetUp()
        {
            _canvasCenter = Point.Empty;
            _layourter = new CircularCloudLayouter(_canvasCenter);
        }

        [Test]
        public void PutNextRectangle_ShouldPutCorrect_WhenPutFirstRectangleWithEvenSize()
        {
            var expectedRectangle = new Rectangle(new Point(-5, -5), new Size(10, 10));
            _layourter.PutNextRectangle(new Size(10, 10)).Should().BeEquivalentTo(expectedRectangle);
        }

        [Test]
        public void PutNextRectangle_ShouldPutCorrect_WhenPutFirstRectangleWithOddSize()
        {
            var expectedRectangle = new Rectangle(new Point(-4, -4), new Size(9, 9));
            _layourter.PutNextRectangle(new Size(9, 9)).Should().BeEquivalentTo(expectedRectangle);
        }

        [Test]
        public void PutNextRectangle_ShouldNotIntersect_WhenPutRectangles()
        {
            var random = new Random();
            var rectangles = new List<Rectangle>();
            for (var i = 0; i < 50; i++)
                rectangles.Add(_layourter.PutNextRectangle(new Size(random.Next(1, 5), random.Next(1, 5))));

            ContainsAnyIntersections().Should().BeFalse();

            bool ContainsAnyIntersections()
            {
                for (var i = 0; i < rectangles.Count; i++)
                {
                    if (rectangles[i].IntersectsWith(rectangles.Take(i).Skip(1)))
                        return true;
                }

                return false;
            }
        }

        [Test]
        public void PutNextRectangle_AllRectanglesShouldLieInsideCircle_WhenPutRectangles()
        {
            const int radius = 8;
            var rectangles = new List<Rectangle>();
            for(var i = 0; i < 20; i++)
                rectangles.Add(_layourter.PutNextRectangle(new Size(2, 2)));
            rectangles.Any(x => GetDistance(x.GetMiddlePoint(), _canvasCenter) >= radius).Should().BeFalse();

            static double GetDistance(Point p1, Point p2)
            {
                return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
            }
        }

        [Test]
        public void PutNextRectangle_AllRectanglesShouldMoveToCanvasCenter_WhenPutRectangles()
        {
            var rectangles = new List<Rectangle>();
            for (var i = 0; i < 20; i++)
            {
                rectangles.Add(_layourter.PutNextRectangle(new Size(2,4)));
            }

            rectangles.Any(x => !CanMove(x, rectangles, _canvasCenter, 1)).Should().BeTrue();

            static bool CanMove(Rectangle rectangle, IEnumerable<Rectangle> rectangles, Point toPoint, int axisPoint)
            {
                return !rectangle.MoveOneStepTowardsPoint(toPoint, axisPoint).IntersectsWith(rectangles);
            }
        }

        [TestCase(-1, 5)]
        [TestCase(5, -1)]
        [TestCase(5, 0)]
        [TestCase(0, 5)]
        public void PutNextRectangle_ShouldThrowException_WhenSizeIncorrect(int width, int height)
        {
            var rectangleSize = new Size(width, height);
            Action act = () => _layourter.PutNextRectangle(rectangleSize);
            act.Should().Throw<ArgumentException>().WithMessage("Width and height of the rectangle must be positive");
        }
    }
}
