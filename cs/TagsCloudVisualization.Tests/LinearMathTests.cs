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

            AssertPoint(new Point(70, 70), cartesianPoint);
        }

        [Test]
        public void CenterWith_Size100x100AndZeroPoint_ShouldBeHalfSide()
        {
            var center = new Point(0, 0).CenterWith(new Size(100, 100));

            center.Should().Be(new Point(50, 50));
        }

        private void AssertPoint(Point expected, Point actual)
        {
            actual.X.Should().Be(expected.X);
            actual.Y.Should().Be(expected.Y);
        }
    }
}
