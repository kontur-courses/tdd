using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagCloud;

namespace TagCloudUnitTests
{

    [TestFixture]
    public class ArchimedeanSpiral_Should
    {
        private ArchimedeanSpiral _archimedeanSpiral;

        [SetUp]
        public void Setup()
        {
            _archimedeanSpiral = new ArchimedeanSpiral(new Point(0, 0));
        }

        [Test]
        public void ReturnsCentralPoint_WhenAngleEqualsZero()
        {
            _archimedeanSpiral.GetNextPoint().Should().BeEquivalentTo(new Point(0, 0));
        }

        [Test]
        public void ReturnsCorrectPoint_WhenAngleEqualsHalfPi()
        {
            for (int i = 0; i < 90; i++)
                _archimedeanSpiral.GetNextPoint();

            _archimedeanSpiral.GetNextPoint().Should().BeEquivalentTo(new Point(0, (int)(Math.PI / 2)));
        }

        [Test]
        public void ReturnsCorrectPoint_WhenAngleEqualsPi()
        {
            for (int i = 0; i < 180; i++)
                _archimedeanSpiral.GetNextPoint();

            _archimedeanSpiral.GetNextPoint().Should().BeEquivalentTo(new Point((int)(-Math.PI), 0));
        }
    }
}
