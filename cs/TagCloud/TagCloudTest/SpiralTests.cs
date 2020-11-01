using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagCloud;

namespace TagCloudTest
{
    [TestFixture]
    public class SpiralTests
    {
        private Spiral spiral;

        [SetUp]
        public void SetUp()
        {
            spiral = new Spiral(new Point(0, 0));
        }

        [Test]
        public void SpiralStartsAtCenter()
        {
            var startingPoint = new Point(1, 5);
            var shiftedSpiral = new Spiral(startingPoint);

            shiftedSpiral.CurrentPoint.Should().Be(startingPoint);
        }
    }
}