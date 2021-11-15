using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class ExpandingSquare_Should
    {
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
            actualPoints.Should().BeEquivalentTo(
                expectedPoints,
                config => config.WithStrictOrdering());
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
        
        [Test]
        public void ReturnValuesWithIncreasingChebyshevDistance()
        {
            var startPoint = new Point(5, 6);
            var square = new ExpandingSquare(startPoint);

            var pointsCount = 500;
            using (new AssertionScope())
            {
                DistanceShouldExpire(square.Take(pointsCount), startPoint);
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

        private void DistanceShouldExpire(IEnumerable<Point> points, Point startPoint)
        {
            var maxDistance = 0;
            foreach (var point in points)
            {
                var distance = Math.Max(
                    Math.Abs(point.X - startPoint.X),
                    Math.Abs(point.Y - startPoint.Y));
                distance.Should().BeGreaterOrEqualTo(maxDistance);

                maxDistance = distance;
            }
        }
    }
}