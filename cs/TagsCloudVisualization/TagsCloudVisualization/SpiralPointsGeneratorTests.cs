using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class SpiralPointsGeneratorTests
    {
        [Test]
        public void Constructor_Throws_WhenCenterCoordinatesAreNegative()
        {
            Action act = () => new SpiralPointsGenerator(new Point(-1, -1), 0, 0, 1, 1);

            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Constructor_Throws_WhenStartRadiusIsNegative()
        {
            Action act = () => new SpiralPointsGenerator(new Point(1, 1), -1, 0, 1, 1);

            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Constructor_Throws_WhenAngleDeltaIsZero()
        {
            Action act = () => new SpiralPointsGenerator(new Point(1, 1), 1, 0, 0, 1);

            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Constructor_Throws_WhenRadiusDeltaIsZero()
        {
            Action act = () => new SpiralPointsGenerator(new Point(1, 1), 1, 0, 1, 0);

            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Constructor_NotThrows_WhenParamsAreValid()
        {
            Action act = () => new SpiralPointsGenerator(new Point(1, 1), 1, 0, 1, 1);

            act.Should().NotThrow<ArgumentException>();
        }

        [Test]
        public void GetSpiralPoints_ReturnsCenter_WhenItIsCalledFirstTime()
        {
            var center = new Point(1, 1);
            var generator = new SpiralPointsGenerator(center);

            generator.GetSpiralPoints().FirstOrDefault().Should().Be(center);
        }

        [Test]
        public void GetSpiralPoints_IsLazy()
        {
            var generator = new SpiralPointsGenerator();

            generator.ExecutionTimeOf(g => g.GetSpiralPoints().Count()).Should().BeGreaterThan(1000.Milliseconds());
        }
    }
}