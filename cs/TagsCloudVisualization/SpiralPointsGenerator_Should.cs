using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class SpiralPointsGenerator_Should
    {
        private const int DistanceBetweenPoints = 1;
        private const double AngleIncrement = 1;
        private IEnumerable<Point> points;

        [SetUp]
        public void SetUp()
        {
            points = new SpiralPointsGenerator().GetPoints(DistanceBetweenPoints, AngleIncrement);
        }

        [TestCase(0, TestName = "DistanceIsZero")]
        [TestCase(-1, TestName = "DistanceIsNegative")]
        public void ThrowArgumentException_OnInvalidDistance(int distanceBetweenPoints)
        {
            Action action = () => new SpiralPointsGenerator().GetPoints(distanceBetweenPoints).First();
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void ReturnZeroPoint_OnFirstGenerating()
        {
            points.First().Should().Be(new Point());
        }

        [Test]
        public void ReturnNotZeroPoint_AfterFirstGenerating()
        {
            points.Take(2).Last().Should().NotBe(new Point());
        }

        [Test]
        public void GenerateNonRepeatingPoints()
        {
            var pointsToCheck = points.Take(1000).ToArray();
            for (var i = 0; i < pointsToCheck.Length; i++)
            {
                for (var j = i + 1; j < pointsToCheck.Length; j++)
                    pointsToCheck[i].Should().NotBe(pointsToCheck[j]);
            }
        }

        [Test]
        public void GeneratePointsWithIncreasingRadius()
        {
            double previousRadius = 0;
            var firstPoint = true;
            foreach (var point in points.Take(100))
            {
                var radius = Math.Sqrt(point.X * point.X + point.Y * point.Y);
                if (!firstPoint)
                    radius.Should().BeGreaterThan(previousRadius);
                firstPoint = false;
                previousRadius = radius;
            }
        }

        [Test]
        public void GeneratePointsWithIncreasingAngle()
        {
            double previousAngle = 0;
            var firstPoint = true;
            foreach (var point in points.Take(4))
            {
                var angle = Math.Atan2(point.Y, point.X);
                if (!firstPoint)
                    angle.Should().BeGreaterThan(previousAngle);
                firstPoint = false;
                previousAngle = angle;
            }
        }
    }
}