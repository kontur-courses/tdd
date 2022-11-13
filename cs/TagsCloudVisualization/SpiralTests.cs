using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class SpiralTests
    {
        private Point center;
        private double radius;
        private double angle;

        [SetUp]
        public void SetUp()
        {
            center = new Point(0, 0);
            radius = 0;
            angle = 0;
        }

        [Test]
        public void GetPoints_ShouldReturnFirstPointInCenter()
        {
            var spiral = new Spiral(center, 1, 1);
            var points = spiral.GetPoints().Take(5).ToList();
            points.First().Should().BeEquivalentTo(center);
        }

        [TestCase(1, 1, TestName = "Positive angleOffset and radiusOffset")]
        [TestCase(-1, -1, TestName = "Negative angleOffset and radiusOffset")]
        public void GetPoints_ShouldReturnCorrectPoints(double radiusOffset, double angleOffset)
        {
            var spiral = new Spiral(center, angleOffset, radiusOffset);
            var points = spiral.GetPoints().Skip(1).Take(10).ToList();
            foreach (var point in points)
            {
                angle += angleOffset;
                radius += radiusOffset;
                var x = center.X + (int) Math.Round(radius * Math.Cos(angle));
                var y = center.Y + (int) Math.Round(radius * Math.Sin(angle));
                var correctPoint = new Point(x, y);
                point.Should().BeEquivalentTo(correctPoint);
            }
        }
    }
}