using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class ArchimedesSpiralTests
    {
        public const double Epsilon = 1e-2;
        private IEnumerator<PointF> generator;

        [TestCase(7, 0, 0, 0, 0, 0, TestName = "SpiralStartsAtCenter")]
        [TestCase(7, 5, 5, 0, 5, 5, TestName = "SpiralStartsAtCenterWhenCenterIsNotZero")]
        [TestCase(7, 0, 0, 5, 2.76f, 1.29f, TestName = "RandomPoint")]
        [TestCase(7, 5, 1, 5, 7.76f, 2.29f, TestName = "RandomPointWhenCenterIsNotZero")]
        [TestCase(3, 0, 0, 5, 1.18f, 0.55f, TestName = "RandomPointOnRandomThickness")]
        public void IEnumerator_YieldsCorrectValues(float a, float centerX, float centerY,
            int elementIndex, float expectedValueX, float expectedValueY)
        {
            generator = new ArchimedesSpiral(a, new PointF(centerX, centerY)).GetEnumerator();
            for (var i = 0; i <= elementIndex; i++)
                generator.MoveNext();
            generator.Current.X.Should().BeApproximately(expectedValueX, (float) Epsilon);
            generator.Current.Y.Should().BeApproximately(expectedValueY, (float) Epsilon);
        }

        [TestCase(7, 0, 0, 20, TestName = "RandomPointOnSpiral")]
        [TestCase(2, 0, 0, 16, TestName = "RandomPointOnDifferentThickness")]
        [TestCase(2, 16, -2, 16, TestName = "RandomPointWhenCenterIsNotZero")]
        public void IEnumerator_YieldsSequenceInCorrectOrder(float a, float centerX, float centerY, int elementIndex)
        {
            generator = new ArchimedesSpiral(a, new PointF(centerX, centerY)).GetEnumerator();
            for (var i = 0; i <= elementIndex; i++)
                generator.MoveNext();
            var firstPoint = generator.Current;
            generator.MoveNext();
            float theta, r;
            (r, theta) = PointConverter.TransformCartesianToPolar(firstPoint.X - centerX, firstPoint.Y - centerY);
            theta += ArchimedesSpiral.DeltaAngle;
            r = a * theta;
            var nextX = (float) (r * Math.Cos(theta)) + centerX;
            var nextY = (float) (r * Math.Sin(theta)) + centerY;
            generator.Current.X.Should().BeApproximately(nextX, (float)Epsilon);
            generator.Current.Y.Should().BeApproximately(nextY, (float)Epsilon);
        }

        [TearDown]
        public void Dispose()
        {
            generator?.Dispose();
        }
    }
}