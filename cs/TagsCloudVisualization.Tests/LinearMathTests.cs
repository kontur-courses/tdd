using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class LinearMathTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void DistanceBetween_TwoPoints_ShouldBeReturnCorrectDistance()
        {
            var distance = LinearMath.DistanceBetween(new Point(0, 0), new Point(100, 120));

            distance.Should().BeApproximately(156.2, 1);
        }

        [Test]
        public void GetDiagonal_Size10x10_ShouldBeReturnCorrectDiagonal()
        {
            var size = new Size(10, 10);

            var diagonal = LinearMath.GetDiagonal(size);

            diagonal.Should().BeApproximately(14.14, 1);
        }

        [Test]
        public void PolarToCartesian_PolarCoordinatesCenterIsZero_ShouldBeCorrectConvert()
        {
            var center = new Point(0, 0);
            var angle = Math.PI / 4;
            var radius = 100;

            var cartesianPoint = LinearMath.PolarToCartesian(center, radius, angle);

            cartesianPoint.Should().Be(new Point(70, 70));
        }
    }
}
