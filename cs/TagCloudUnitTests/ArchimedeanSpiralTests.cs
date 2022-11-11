using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagCloud;

namespace TagCloudUnitTests
{

    [TestFixture]
    public class ArchimedeanSpiralTests
    {

        [TestCase(-1)]
        [TestCase(0)]
        public void Constructor_ThrowsArgumentException_WhenScaleFactorHasInvalidValue(double scaleFactor)
        {
            Action action = () => new ArchimedeanSpiral(new Point(0, 0), scaleFactor);

            action.Should().Throw<ArgumentException>();
        }

        [TestCaseSource(nameof(TestsParameters))]
        public void GetNextPoint_ReturnsCorrectPoint_WhenSpiralTurnedOnAngle(Point centralPoint, double scaleFactor, double angleOnRadians, Point point)
        {
            var archimedeanSpiral = new ArchimedeanSpiral(centralPoint, scaleFactor);

            TurnSpiralOnAngle(archimedeanSpiral, angleOnRadians);

            archimedeanSpiral.GetNextPoint().Should().BeEquivalentTo(point);
        }

        private void TurnSpiralOnAngle(ArchimedeanSpiral archimedeanSpiral, double angleOnRadians)
        {
            int stepsCount = (int)(Math.Abs(angleOnRadians) * 180 / Math.PI);

            for (int i = 0; i < stepsCount; i++)
                archimedeanSpiral.GetNextPoint();
        }

        private static readonly object[] TestsParameters =
        {
            new object[] { new Point(0, 0), 1.0, 0, new Point(0, 0) },
            new object[]
            {
                new Point(0, 0), 1.0, Math.PI / 4, new Point((int)(Math.PI / 4 * Math.Sqrt(2) / 2), (int)(Math.PI / 4 * Math.Sqrt(2) / 2))
            },
            new object[] { new Point(0, 0), 1.0, Math.PI / 2, new Point(0, (int)(Math.PI / 2)) },
            new object[] { new Point(0, 0), 1.0, Math.PI, new Point((int)(-Math.PI), 0) },
            new object[] { new Point(0, 0), 2.0, 0, new Point(0, 0) },
            new object[]
            {
                new Point(0, 0), 2.0, Math.PI / 4, new Point((int)(2 * Math.PI / 4 * Math.Sqrt(2) / 2), (int)(2 * Math.PI / 4 * Math.Sqrt(2) / 2))
            },
            new object[] { new Point(0, 0), 2.0, Math.PI / 2, new Point(0, (int)(2 * Math.PI / 2)) },
            new object[] { new Point(0, 0), 2.0, Math.PI, new Point((int)(-2 * Math.PI), 0) },
            new object[] { new Point(1, 1), 1.0, 0, new Point(1, 1) },
            new object[]
            {
                new Point(1, 1), 1.0, Math.PI / 4, new Point(1 + (int)(Math.PI / 4 * Math.Sqrt(2) / 2),1 +  (int)(Math.PI / 4 * Math.Sqrt(2) / 2))
            },
            new object[] { new Point(1, 1), 1.0, Math.PI / 2, new Point(1, 1 + (int)(Math.PI / 2)) },
            new object[] { new Point(1, 1), 1.0, Math.PI, new Point(1+(int)( - Math.PI), 1) },
            new object[] { new Point(1, 1), 2.0, 0, new Point(1, 1) },
            new object[]
            {
                new Point(1, 1), 2.0, Math.PI / 4, new Point(1 + (int)(2 * Math.PI / 4 * Math.Sqrt(2) / 2), 1 + (int)(2 * Math.PI / 4 * Math.Sqrt(2) / 2))
            },
            new object[] { new Point(1, 1), 2.0, Math.PI / 2, new Point(1, 1 + (int)(2 * Math.PI / 2)) },
            new object[] { new Point(1, 1), 2.0, Math.PI, new Point(1 + (int)( - 2 * Math.PI), 1) },
        };
    }
}
