using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;


namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class ArchimedeanSpiral_Should
    {
        private readonly Point center = new Point(0, 0);
        private readonly double stepLength = 0.1;
        private readonly double angleShiftForEachPoint = 2 * Math.PI / 1000;

        [TestCase(0, 0.1, TestName = "{m}ZeroStepLength")]
        [TestCase(1, 0, TestName = "{m}ZeroAngleShift")]
        [TestCase(-1, 0.1, TestName = "{m}NegativeStepLength")]
        [TestCase(1, -0.1, TestName = "{m}NegativeAngleShift")]
        public void ThrowArgumentException_On(double stepLength,
            double angleShiftForEachPoint)
        {
            Action act = () => new ArchimedeanSpiral(center, 
                stepLength, angleShiftForEachPoint);
            act.Should().Throw<ArgumentException>();
        }

        [TestCase(0, 0)]
        [TestCase(100, -100)]
        public void ReturnCenter_AsFirstPoint(int x, int y)
        {
            var center = new Point(x, y);
            var spiral = new ArchimedeanSpiral(center, stepLength, angleShiftForEachPoint);

            spiral.CalculateNextPoint().Should().BeEquivalentTo(center, 
                "first point must be the center of the spiral");
        }

        [TestCase(5, 2 * Math.PI)]
        [TestCase(0.1, 2 * Math.PI/ 1000)]
        public void PutPointsOnStepLengthDistance_AfterFullCircle(
            double stepLength, double angleShiftForEachPoint)
        {
            var spiral = new ArchimedeanSpiral(center, stepLength, angleShiftForEachPoint);
            var firstPoint = spiral.CalculateNextPoint();

            var amountOfSubsteps = 2 * Math.PI / angleShiftForEachPoint;
            Point secondPoint = new Point();
            while (amountOfSubsteps-- > 0)
                secondPoint = spiral.CalculateNextPoint();

            firstPoint.GetSquaredDistanceTo(secondPoint)
                .Should().BeApproximately(stepLength*stepLength, 0.5,
                "two points differed by 2Pi angle must be " +
                "distanced by length of spiral step");
        }
    }
}
