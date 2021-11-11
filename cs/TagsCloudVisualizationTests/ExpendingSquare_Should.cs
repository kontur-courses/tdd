using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class ExpendingSquare_Should
    {
        [TestCase(10, 10, TestName = "Center in first circle quarter")]
        [TestCase(-10, 10, TestName = "Center in second circle quarter")]
        [TestCase(-10, -10, TestName = "Center in third circle quarter")]
        [TestCase(10, -10, TestName = "Center in fourth circle quarter")]
        [TestCase(0, 0, TestName = "Center in (0, 0)")]
        public void NotThrowExceptionsInConstructor_When(int x, int y)
        {
            var center = new Point(x, y);
            Action action = () => new ExpandingSquare(center);
            action.Should().NotThrow();
        }

        [TestCase(0, 0, TestName = "Center in (0, 0)")]
        [TestCase(5, 6, TestName = "Center is not in (0, 0)")]
        public void ReturnCenterPointOnFirstIteration_When(int x, int y)
        {
            var center = new Point(x, y);
            var square = new ExpandingSquare(center);

            square.First().Should().BeEquivalentTo(center);
        }

        [Test]
        public void ReturnPointsInTopRightBottomLeftOrder()
        {
            var startPoint = new Point(5, 6);
            var square = new ExpandingSquare(startPoint);
            var expectedPoints = new List<Point>
            {
                startPoint,
                startPoint + new Size(-1, -1),
                startPoint + new Size(0, -1),
                startPoint + new Size(1, -1),
                startPoint + new Size(1, 0),
                startPoint + new Size(1, 1),
                startPoint + new Size(0, 1),
                startPoint + new Size(-1, 1),
                startPoint + new Size(-1, 0),
            };

            var actualPoints = square.Take(expectedPoints.Count).ToList();
            actualPoints.Should().BeEquivalentTo(expectedPoints);
        }

        [Test]
        public void NotAffectOnOtherIntegerSquare()
        {
            var firstSquareCenter = new Point(10, 20);
            var firstSquare = new ExpandingSquare(firstSquareCenter);

            var secondSquareCenter = new Point(-10, -20);
            var secondSquare = new ExpandingSquare(secondSquareCenter);

            var takenPointsCount = 20;

            var firstSquarePoints = firstSquare.Take(takenPointsCount);
            var secondSquarePoints = secondSquare.Take(takenPointsCount);

            firstSquarePoints.Intersect(secondSquarePoints).Should().BeEmpty();
        }


        //А такой тест не нарушает паттерн AAA?
        [Test]
        public void ReturnValuesWithIncreasingChebyshevDistance()
        {
            var startPoint = new Point(5, 6);
            var square = new ExpandingSquare(startPoint);

            var maxDistance = 0;
            var pointsCount = 500;

            foreach (var point in square.Take(pointsCount))
            {
                var distance = Math.Max(
                    Math.Abs(point.X - startPoint.X),
                    Math.Abs(point.Y - startPoint.Y));
                distance.Should().BeGreaterOrEqualTo(maxDistance);

                maxDistance = distance;
            }
        }

        [Test]
        [Timeout(1000)]
        public void ReturnPointsWithGoodTimePerformance()
        {
            var square = new ExpandingSquare(new Point(10, 20));
            var takenPointsCount = 10000;

            var difficultListToCreate = square.Take(takenPointsCount).ToList();
        }
    }
}