using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;
using TagsCloudVisualization.Layouters;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class Spiral_Should
    {
        private const double Epsilon = 1e-9;
        private const double DefaultRadiusIncreaseValue = 0.1;
        private const double DefaultAngleIncreaseValue = 0.1;

        [Test]
        [TestCaseSource(nameof(CenterGenerator))]
        public void StartPoint_EqualsCenter_WithCustomCenter(Point center)
        {
            var spiral = new Spiral(center);
            
            spiral.CurrentPoint.X.Should().BeApproximately(center.X, Epsilon);
            spiral.CurrentPoint.Y.Should().BeApproximately(center.Y, Epsilon);
        }

        [Test]
        [TestCaseSource(nameof(CenterGenerator))]
        public void IncreaseSize_ChangingPoint_WithDefaultValues(Point center)
        {
            var spiral = new Spiral(center);
            
            spiral.IncreaseSize();
            
            var currentRadius = DefaultRadiusIncreaseValue;
            var currentAngle = DefaultAngleIncreaseValue;

            var expectedX = CountX(currentRadius, currentAngle, center.X);
            var expectedY = CountY(currentRadius, currentAngle, center.Y);

            spiral.CurrentPoint.X.Should().BeApproximately(expectedX, Epsilon);
            spiral.CurrentPoint.Y.Should().BeApproximately(expectedY, Epsilon);
        }

        [Test]
        [TestCaseSource(nameof(CenterGenerator))]
        public void IncreaseSize_ChangingPoint_WithCustomValues(Point center)
        {
            var spiral = new Spiral(center);
            
            spiral.IncreaseSize(1, 1);
            
            var currentRadius = 1;
            var currentAngle = 1;

            var expectedX = CountX(currentRadius, currentAngle, center.X);
            var expectedY = CountY(currentRadius, currentAngle, center.Y);
            
            spiral.CurrentPoint.X.Should().BeApproximately(expectedX, Epsilon);
            spiral.CurrentPoint.Y.Should().BeApproximately(expectedY, Epsilon);
        }

        [Test]
        [TestCaseSource(nameof(CenterGenerator))]
        public void IncreaseSize_ChangingPointRepeatedly_WithDefaultValues(Point center)
        {
            var spiral = new Spiral(center);
            
            spiral.IncreaseSize();
            spiral.IncreaseSize();
            spiral.IncreaseSize();

            var currentRadius = DefaultRadiusIncreaseValue * 3;
            var currentAngle = DefaultAngleIncreaseValue * 3;

            var expectedX = CountX(currentRadius, currentAngle, center.X);
            var expectedY = CountY(currentRadius, currentAngle, center.Y);
            
            spiral.CurrentPoint.X.Should().BeApproximately(expectedX, Epsilon);
            spiral.CurrentPoint.Y.Should().BeApproximately(expectedY, Epsilon);
        }
        
        [Test]
        [TestCaseSource(nameof(CenterGenerator))]
        public void IncreaseSize_ChangingPointRepeatedly_WithCustomValues(Point center)
        {
            var spiral = new Spiral(center);
            
            spiral.IncreaseSize(1, 2);
            spiral.IncreaseSize(3, 4);
            spiral.IncreaseSize(5, 6);

            var currentRadius = 1 + 3 + 5;
            var currentAngle = 2 + 4 + 6;

            var expectedX = CountX(currentRadius, currentAngle, center.X);
            var expectedY = CountY(currentRadius, currentAngle, center.Y);
            
            spiral.CurrentPoint.X.Should().BeApproximately(expectedX, Epsilon);
            spiral.CurrentPoint.Y.Should().BeApproximately(expectedY, Epsilon);
        }

        private double CountX(double radius, double angle, double currentX)
        {
            return Math.Cos(angle) * radius + currentX;
        }

        private double CountY(double radius, double angle, double currentY)
        {
            return Math.Sin(angle) * radius + currentY;
        }

        private static IEnumerable<Point> CenterGenerator()
        {
            yield return new Point(0, 0);
            yield return new Point(-10, -10);
            yield return new Point(10, 10);
            yield return new Point(-10, 10);
            yield return new Point(10, -10);
            yield return new Point(0, 10);
            yield return new Point(0, -10);
            yield return new Point(10, 0);
            yield return new Point(-10, 0);
        }
    }
}