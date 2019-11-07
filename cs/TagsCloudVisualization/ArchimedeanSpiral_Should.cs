using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class ArchimedeanSpiral_Should
    {
        [TestCase(0, 0)]
        [TestCase(500, 400)]
        public void ReturnCenterPointFirst_WhenAisZero(int centerX, int centerY)
        {
            var center = new Point(centerX, centerY);
            var archimedeanSpiral = new ArchimedeanSpiral(center);
            archimedeanSpiral.GetNextPoint().Should().BeEquivalentTo(center);
        }

        [TestCase(5000, 5000, 2, 2, Math.PI * 2, TestName = "onFullCircle")]
        [TestCase(5000, 5000, 2, 2, Math.PI, TestName = "onHalfCircle")]
        [TestCase(5000, 5000, 2, 2, -Math.PI, TestName = "onOppositeDirection")]
        public void ReturnPointWithGreaterRadius_OnNextStep(int centerX, int centerY, double a, double b, double step)
        {
            var center = new Point(centerX, centerY);
            var archimedeanSpiral = new ArchimedeanSpiral(center, step, a, b);
            var firstPoint = archimedeanSpiral.GetNextPoint();
            var secondPoint = archimedeanSpiral.GetNextPoint();
            var firstRadius = GetDistance(center, firstPoint);
            var secondRadius = GetDistance(center, secondPoint);
            secondRadius.Should().BeGreaterThan(firstRadius);
        }

        private static double GetDistance(Point firstPoint, Point secondPoint)
        {
            return Math.Sqrt((firstPoint.X - secondPoint.X) * (firstPoint.X - secondPoint.X) +
                             (firstPoint.Y - secondPoint.Y) * (firstPoint.Y - secondPoint.Y));
        }
    }
}