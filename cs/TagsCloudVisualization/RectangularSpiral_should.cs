using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;

namespace TagsCloudVisualization
{
    [TestFixture]
    class RectangularSpiral_should
    {
        RectangularSpiral rectangularSpiral;

        [SetUp]
        public void SetUp() => rectangularSpiral = new RectangularSpiral();

        [Test]
        public void GetPoints_ShouldBeReturnFirstPosition_InZeroZero()
        {
            rectangularSpiral.GetPoints().First().Should().Be(new Point(0, 0));
        }

        [Test]
        public void GetPoints_ShouldBeReturn_SecondPointOnUpDirection()
        {
            var upDirection = new Point(0, 1);
            var firstSpiralPoint = rectangularSpiral.GetPoints().First();
            var expectedSecondPosition = 
                new Point(
                    firstSpiralPoint.X + upDirection.X,
                    firstSpiralPoint.Y + upDirection.Y
                );

            rectangularSpiral.GetPoints().Take(2).Last().Should().Be(expectedSecondPosition);
        }

        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        public void GetPoints_ShouldtBeReturnPoints_NotRepeated(int countPoints)
        {
            var spiralPoints = rectangularSpiral.GetPoints().Take(countPoints).ToList();

            var points = new HashSet<Point>(spiralPoints);

            points.Count.Should().Be(countPoints);
        }

        [Test]
        public void GetPoints_ShouldtBeReturnPoints_OnСlockwiseDirections()
        {
            var expectedDirections = new List<Point>
            {
                new Point(0,1),
                new Point(1,0),
                new Point(0,-1),
                new Point(-1, 0),
            };

            var actualDirections = new List<Point>();

            Point previousPoint = Point.Empty;
            Point curentPoint = Point.Empty;
            foreach (var point in rectangularSpiral.GetPoints())
            {
                if(actualDirections.Count == 4) break;
                if(previousPoint == curentPoint)
                {
                    curentPoint = point;
                    continue;
                }

                previousPoint = curentPoint;
                curentPoint = point;

                var curentDirection = new Point(curentPoint.X - previousPoint.X, curentPoint.Y - previousPoint.Y);
                if (!actualDirections.Contains(curentDirection)) actualDirections.Add(curentDirection);
            }

            actualDirections.Should().BeEquivalentTo(expectedDirections);
        }
    }
}
