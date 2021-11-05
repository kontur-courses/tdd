using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;
using static TagsCloudVisualizationTests.Helper;

namespace TagsCloudVisualizationTests
{
    public class SpiralTests
    {
        private Spiral _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new Spiral(new Point(0, 0));
        }

        [Test]
        public void Constructor_ThrowArgumentException_WhenSpiralParamLessOrEqualZero()
        {
            Action action = () => new Spiral(new Point(), -1);

            action.Should().Throw<ArgumentException>().WithMessage("Spiral param must be great than zero");
        }

        [Test]
        public void GetNextPoint_ReturnsCenter_WhenFirstCall()
        {
            var center = _sut.Center;

            var point = _sut.GetNextPoint();

            point.Should().BeEquivalentTo(center);
        }
        
        [TestCase(1000)]
        public void GetNextPoint_ShouldIncreaseDistance_WhenManyCalls(int pointsCount = 100)
        {
            var point = _sut.GetNextPoint();

            for (var i = 1; i < pointsCount / 2; i++) point = _sut.GetNextPoint();
            var halfDistance = point.GetDistance(_sut.Center);
            for (var i = pointsCount / 2; i < pointsCount; i++) point = _sut.GetNextPoint();
            var fullDistance = point.GetDistance(_sut.Center);

            fullDistance.Should().BeGreaterThan(halfDistance);
        }
    }
}