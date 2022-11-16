using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using TagsCloudVisualization.TagCloud;

namespace TagCloudTests
{
    [TestFixture]
    internal class SpiralTests
    {
        public static Point Center { get; private set; }
        private static ISpiral spiral;

        [SetUp]
        public void SetUP()
        {
            Center = new(1920 / 2, 1080 / 2);
        }

        [TestCase(0, 1.1, TestName = "{m}_withZeroDelta")]
        [TestCase(1.1, 0, TestName = "{m}_Zero density")]
        public void Spiral_Constructor_ShouldThrowArgumentException(double delta, double density)
        {
            Action act = () => new Spiral(Center, delta, density);
            act.Should().Throw<ArgumentException>();
        }

        [TestCase(-5, 7, TestName = "{m}_withNegativeDelta")]
        [TestCase(4, -3, TestName = "{m}_withNegativeDensity")]
        [TestCase(-8, -2, TestName = "{m}_withNegativeParams")]
        [TestCase(6, 9, TestName = "{m}_withPositiveParams")]
        public void Spiral_Constructor_ShouldNotThrowArgumentException(double delta, double density)
        {
            Action act = () => new Spiral(Center, delta, density);
            act.Should().NotThrow();
        }

        [Test]
        public void Spiral_GetNextPoint_OnCorrectInput()
        {
            var expectedPoints = new List<Point>
            {
                new Point(960, 540),
                new Point(961, 542),
                new Point(958, 544),
                new Point(954, 541),
                new Point(955, 534)
            };

            spiral = new Spiral(Center, 1, 2);

            foreach (var expectedPoint in expectedPoints)
            {
                spiral.GetNextPoint().Should().Be(expectedPoint);
            }
        }
    }
}
