using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.PointGenerator;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class Spiral_Tests
    {
        [Test]
        public void GetCoordinates_ReturnCoordinatesThatRadiusShouldIncrease()
        {
            var spiral = new Spiral(1f, 0.2f);
            var points = spiral.GetPoints(new PointF()).Take(100).ToArray();
            for (var i = 1; i < points.Length; i++)
            {
                var previusRadius = GetDistance(new PointF(), points[i - 1]);
                var currentRadius = GetDistance(new PointF(), points[i]);

                (previusRadius < currentRadius).Should().BeTrue();
            }
        }

        private double GetDistance(PointF a, PointF b)
        {
            return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }
    }
}