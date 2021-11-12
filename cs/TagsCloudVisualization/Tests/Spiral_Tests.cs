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
            var center = new PointF();
            var spiral = new Spiral(0.2f, 1);
            var points = spiral.GetPoints(new PointF(), new Size()).Take(100).ToArray();
            for (var i = 1; i < points.Length; i++)
            {
                var previousRadius = center.DistanceTo(points[i - 1]);
                var currentRadius = center.DistanceTo(points[i]);

                (previousRadius <= currentRadius).Should().BeTrue();
            }
        }
    }
}