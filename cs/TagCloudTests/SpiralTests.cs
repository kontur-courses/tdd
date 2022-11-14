using FluentAssertions;
using NUnit.Framework;
using System;
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
           Action act = () => new SpiralBeta(Center, delta, density);
           act.Should().Throw<ArgumentException>();
        }

        [TestCase(-5, 7, TestName = "{m}_withNegativeDelta")]
        [TestCase(4, -3, TestName = "{m}_withNegativeDensity")]
        [TestCase(-8, -2, TestName = "{m}_withNegativeParams")]
        [TestCase(6, 9, TestName = "{m}_withPositiveParams")]
        public void Spiral_Constructor_ShouldNotThrowArgumentException(double delta, double density)
        {
            Action act = () => new SpiralBeta(Center, delta, density);
            act.Should().NotThrow();
        }

        [Test]
        public void Spiral_GetNextPoint_OnCorrectInput()
        {
            var expectedPoint1 = new Point(960, 540);
            var expectedPoint2 = new Point(961, 542);
            var expectedPoint3 = new Point(958, 544);
            var expectedPoint4 = new Point(954, 541);
            var expectedPoint5 = new Point(955, 543);

            spiral = new SpiralBeta(Center, 1, 2);

            spiral.GetNextPoint().Should().BeEquivalentTo(expectedPoint1);
            spiral.GetNextPoint().Should().BeEquivalentTo(expectedPoint2);
            spiral.GetNextPoint().Should().BeEquivalentTo(expectedPoint3);
            spiral.GetNextPoint().Should().BeEquivalentTo(expectedPoint4);
            spiral.GetNextPoint().Should().BeEquivalentTo(expectedPoint5);
        }
    }

    public class SpiralBeta : ISpiral
    {
        public SpiralBeta(Point center, double delta, double density)
        {
            throw  new NotImplementedException();
        }

        public Point GetNextPoint()
        {
            throw new NotImplementedException();
        }
    }
}
