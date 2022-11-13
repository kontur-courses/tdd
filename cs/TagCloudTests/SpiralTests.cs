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
        public static Point _center;
        private static ISpiral spiral;

        [SetUp]
        public void SetUP()
        {
            _center = new(1920 / 2, 1080 / 2);
        }

        [TestCase(0, 1.1, TestName = "{m}_Zero delta")]
        [TestCase(1.1, 0, TestName = "{m}_Zero density")]
        public void Spiral_Constructor_ShouldThrowArgumentException(double delta, double density)
        {
           Action act = () => new SpiralBeta(_center, delta, density);
           act.Should().Throw<ArgumentException>();
        }

        [TestCase(-1, 1, TestName = "{m}_")]
        [TestCase(1, -1, TestName = "{m}_")]
        [TestCase(-1, -1, TestName = "{m}_")]
        [TestCase(1, 1, TestName = "{m}_")]
        public void Spiral_Constructor_ShouldNotThrowArgumentException(double delta, double density)
        {
            Action act = () => new SpiralBeta(_center, delta, density);
            act.Should().NotThrow();
        }

        [Test]
        public void Spiral_GetNextPoint_OnCorrectInput()
        {
            spiral.GetNextPoint().Should().BeEquivalentTo(_center);
            spiral.GetNextPoint().Should().BeEquivalentTo(new Point(961, 541));
            spiral.GetNextPoint().Should().BeEquivalentTo(new Point(959, 543));
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
