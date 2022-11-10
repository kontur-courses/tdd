using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace TagCloud
{
    class SpiralPointGeneratorTests
    {
        [TestCase(0,0, TestName = "center in zero")]
        [TestCase(3, 5, TestName = "center non zero")]
        public void GetNextPoint_FirstPointCreatedInCenter_When(int centerX, int centerY)
        {
            var center = new Point(centerX, centerY);
            var spiralPointGenerator = new SpiralPointGenerator(center);

            var firstPoint = spiralPointGenerator.GetNextPoint();

            firstPoint.Should().BeEquivalentTo(center);
        }

        [TestCase(0, 0, TestName = "center in zero")]
        [TestCase(3, 5, TestName = "center non zero")]
        public void GetNextPoint_NewPointsAreMovingAwayFromCenter_When(int centerX, int centerY)
        {
            var center = new Point(centerX, centerY);
            var spiralPointGenerator = new SpiralPointGenerator(center);
            var firstPoint = spiralPointGenerator.GetNextPoint();
            var previousPoint = spiralPointGenerator.GetNextPoint();

            for (int i = 1; i < 1000; i++)
            {
                var currentPoint = spiralPointGenerator.GetNextPoint();
                if (i % 200 == 0)
                {
                    var currentDistanceToCenter = GetDistance(firstPoint, currentPoint);
                    var previousDistanceToCenter = GetDistance(firstPoint, previousPoint);
                    currentDistanceToCenter.Should().BeGreaterThan(previousDistanceToCenter);
                    previousPoint = currentPoint;
                }
            }
        }

        private double GetDistance(Point a, Point b)
        {
            var diff = Point.Subtract(a, new Size(b));
            return Math.Pow(diff.X * diff.X + diff.Y * diff.Y, 0.5);
        }
    }
}
