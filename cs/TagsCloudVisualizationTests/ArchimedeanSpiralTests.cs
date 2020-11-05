using System;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    public class ArchimedeanSpiralTests
    {
        private Point Center { get; set; }
        private ArchimedeanSpiral Sut { get; set; }

        [SetUp]
        public void SetUp()
        {
            Center = new Point(500, 500);
            Sut = new ArchimedeanSpiral(Center, 2);
        }

        [Test]
        public void GetNextPoint_ReturnStartPoint_OnFirstRequest()
        {
            Sut.GetNextPoint().Should().Be(Center);
        }

        [Test]
        public void GetNextPoint_ReturnDifferentPoints_OnMultipleRequest()
        {
            var firstPoint = Sut.GetNextPoint();
            var secondPoint = Sut.GetNextPoint();

            secondPoint.Should().NotBe(firstPoint);
        }

        [Test]
        public void GetNextPoint_ReturnPoint_WhenCalled()
        {
            Sut.GetNextPoint().Should().BeOfType(typeof(Point));
        }

        [TestCase(0 ,TestName = "distanceBetweenLoops is zero")]
        [TestCase(-1 ,TestName = "distanceBetweenLoops is negative")]
        public void ThrowException_When(double distanceBetweenLoops)
        {
            Func<ArchimedeanSpiral> sut  = () => new ArchimedeanSpiral(Center, distanceBetweenLoops);

            sut.Should().Throw<ArgumentException>();
        }
    }
}