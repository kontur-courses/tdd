using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;
using TagsCloudVisualization.Layouters.RectangleLayouters;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class Spiral_Should
    {
        [Test]
        [TestCaseSource(nameof(CenterGenerator))]
        public void StartPoint_EqualsCenter_WithCustomCenter(Point center)
        {
            var spiral = new Spiral(center);
            
            spiral.CurrentPoint.X.Should().BeApproximately(center.X, 1e-9);
            spiral.CurrentPoint.Y.Should().BeApproximately(center.Y, 1e-9);
        }

        [Test]
        [TestCaseSource(nameof(CenterGenerator))]
        public void IncreaseSize_ChangingPoint_WithDefaultValues(Point center)
        {
            var spiral = new Spiral(center);
            
            spiral.IncreaseSize();

            spiral.CurrentPoint.X.Should().BeApproximately(Math.Cos(0.1) * 0.1 + center.X, 1e-9);
            spiral.CurrentPoint.Y.Should().BeApproximately(Math.Sin(0.1) * 0.1 + center.Y, 1e-9);
        }

        [Test]
        [TestCaseSource(nameof(CenterGenerator))]
        public void IncreaseSize_ChangingPoint_WithCustomValues(Point center)
        {
            var spiral = new Spiral(center);
            
            spiral.IncreaseSize(1, 1);
            
            spiral.CurrentPoint.X.Should().BeApproximately(Math.Cos(1) * 1 + center.X, 1e-9);
            spiral.CurrentPoint.Y.Should().BeApproximately(Math.Sin(1) * 1 + center.Y, 1e-9);
        }

        [Test]
        [TestCaseSource(nameof(CenterGenerator))]
        public void IncreaseSize_ChangingPointRepeatedly_WithDefaultValues(Point center)
        {
            var spiral = new Spiral(center);
            
            spiral.IncreaseSize();
            spiral.IncreaseSize();
            spiral.IncreaseSize();
            
            spiral.CurrentPoint.X.Should().BeApproximately(Math.Cos(0.3) * 0.3 + center.X, 1e-9);
            spiral.CurrentPoint.Y.Should().BeApproximately(Math.Sin(0.3) * 0.3 + center.Y, 1e-9);
        }
        
        [Test]
        [TestCaseSource(nameof(CenterGenerator))]
        public void IncreaseSize_ChangingPointRepeatedly_WithCustomValues(Point center)
        {
            var spiral = new Spiral(center);
            
            spiral.IncreaseSize(1, 2);
            spiral.IncreaseSize(3, 4);
            spiral.IncreaseSize(5, 6);
            
            spiral.CurrentPoint.X.Should().BeApproximately(Math.Cos(12) * 9 + center.X, 1e-9);
            spiral.CurrentPoint.Y.Should().BeApproximately(Math.Sin(12) * 9 + center.Y, 1e-9);
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