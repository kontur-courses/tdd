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

        [TestCaseSource(nameof(_getPointsShouldReturnCorrectPointsCases))]
        public void GetPoints_ShouldReturnCorrectPoints(double radiusOffset, double angleOffset,
            List<Point> expectedPoints)
        {
            var spiral = new Spiral(center, angleOffset, radiusOffset);
            var points = spiral.GetPoints().Take(10).ToList();
            for (var i = 0; i < 10; i++)
                points[i].Should().BeEquivalentTo(expectedPoints[i]);
        }

        private static object[] _getPointsShouldReturnCorrectPointsCases =
        {
            new object[]
            {
                1, 1, new List<Point>
                {
                    new Point(0, 0), new Point(1, 1), new Point(-1, 2), new Point(-3, 0),
                    new Point(-3, -3), new Point(1, -5), new Point(6, -2), new Point(5, 5),
                    new Point(-1, 8), new Point(-8, 4), new Point(-8, -5)
                }
            },
            new object[]
            {
                -1, -1, new List<Point>
                {
                    new Point(0, 0), new Point(-1, 1), new Point(1, 2),
                    new Point(3, 0), new Point(3, -3), new Point(-1, -5), new Point(-6, -2),
                    new Point(-5, 5), new Point(1, 8), new Point(8, 4), new Point(8, -5)
                }
            }
        };
    }
}