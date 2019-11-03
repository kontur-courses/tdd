using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class RoundSpiralPositionGenerator_Should
    {
        [Test]
        public void FirstStep_Should_EqualCenterSpiral()
        {
            var center = new Point(800, 600);
            var spiralGenerator = new RoundSpiralPositionGenerator(center);
            spiralGenerator.Next().Should().BeEquivalentTo(center);
        }
        
        [Test]
        public void SecondStep_Should_NotEqualCenterSpiral()
        {
            var center = new Point(800, 600);
            var spiralGenerator = new RoundSpiralPositionGenerator(center);
            spiralGenerator.Next();
            spiralGenerator.Next().Should().NotBeEquivalentTo(center);
        }

        [Test]
        public void AfterManyStep_Should_DeltaBetweenCenterAndCurrentStepMoreThanRadiusBetweenTurns()
        {
            var center = new Point(800, 600);
            var spiralGenerator = new RoundSpiralPositionGenerator(center);
            for (var i = 0; i < 100; i++)
            {
                spiralGenerator.Next();
            }

            var currentPoint = spiralGenerator.Next();
            Math.Abs(Math.Sqrt(Math.Pow(currentPoint.X- center.X, 2) + Math.Pow(currentPoint.Y - center.Y, 2))).Should().BeGreaterThan((int)spiralGenerator.RadiusBetweenTurns);
        }
    }
}
