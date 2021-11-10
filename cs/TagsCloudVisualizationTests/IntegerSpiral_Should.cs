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
    public class IntegerSpiral_Should
    {
        [TestCase(10, 10, TestName = "Center in first circle quarter")]
        [TestCase(-10, 10, TestName = "Center in second circle quarter")]
        [TestCase(-10, -10, TestName = "Center in third circle quarter")]
        [TestCase(10, -10, TestName = "Center in fourth circle quarter")]
        [TestCase(0, 0, TestName = "Center in (0, 0)")]
        public void NotThrowExceptionsInConstructor_When(int x, int y)
        {
            var center = new Point(x, y);
            Action action = () => new IntegerSpiral(center);
            action.Should().NotThrow();
        }

        [TestCase(0, 0, TestName = "Center in (0, 0)")]
        [TestCase(5, 6, TestName = "Center is not in (0, 0)")]
        public void ReturnCenterPointOnFirstIteration_When(int x, int y)
        {
            var center = new Point(x, y);
            var spiral = new IntegerSpiral(center);

            spiral.First().Should().BeEquivalentTo(center);
        }

        [Test]
        public void ReturnPointsInTopRightBottomLeftOrder()
        {
            var startPoint = new Point(5, 6);
            var spiral = new IntegerSpiral(startPoint);
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

            var actualPoints = spiral.Take(expectedPoints.Count).ToList();
            actualPoints.Should().BeEquivalentTo(expectedPoints);
        }

        [Test]
        public void NotAffectOnOtherIntegerSpiral()
        {
            var firstSpiralCenter = new Point(10, 20);
            var firstSpiral = new IntegerSpiral(firstSpiralCenter);

            var secondSpiralCenter = new Point(-10, -20);
            var secondSpiral = new IntegerSpiral(secondSpiralCenter);

            var takenPointsCount = 20;

            var firstSpiralPoints = firstSpiral.Take(takenPointsCount);
            var secondSpiralPoints = secondSpiral.Take(takenPointsCount);

            firstSpiralPoints.Intersect(secondSpiralPoints).Should().BeEmpty();
        }
        
        [Test]
        [Timeout(1000)]
        public void ReturnPointsWithGoodTimePerformance()
        {
            var spiral = new IntegerSpiral(new Point(10, 20));
            var takenPointsCount = 10000;

            var difficultListToCreate = spiral.Take(takenPointsCount).ToList();
        }
    }
}