using System;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class Spiral_Should
    {
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetNextPoint_ShouldContainPointsFromSpiral_OnCorrespondingInput(int expectedPointShift)
        {
            const int parameter = 2;
            const float stepInRadians = (float)(Math.PI / 6);
            var expectedPoint = GeometryUtils.ConvertPolarToIntegerCartesian(
                parameter * stepInRadians * expectedPointShift, 
                stepInRadians * expectedPointShift);
            var spiral = new Spiral(parameter, 30);

            var pointFromSpiral = spiral.GetNextPoint().Take(expectedPointShift + 1).Last();

            pointFromSpiral.Should().Be(expectedPoint);
        }
    }
}