using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    public class SpiralTests
    {
        private Spiral systemUnderTest;

        [SetUp]
        public void SetUp()
        {
            systemUnderTest = new Spiral(new Point(0, 0));
        }

        [Test]
        public void Constructor_ThrowArgumentException_WhenSpiralParamLessOrEqualZero()
        {
            Action action = () => new Spiral(new Point(), -1);

            action.Should().Throw<ArgumentException>().WithMessage("Spiral param must be greater than zero");
        }

        [Test]
        public void GetNextPoint_ReturnsCenter_WhenFirstCall()
        {
            var center = systemUnderTest.Center;

            var point = systemUnderTest.GetNextPoint();

            point.Should().BeEquivalentTo(center);
        }

        [Test]
        public void GetNextPoint_ShouldIncreaseDistance_WhenManyCalls()
        {
            const int pointsCount = 1000;
            var point = systemUnderTest.GetNextPoint();

            for (var i = 1; i < pointsCount / 2; i++) 
                point = systemUnderTest.GetNextPoint();
            var halfDistance = point.GetDistance(systemUnderTest.Center);
            for (var i = pointsCount / 2; i < pointsCount; i++) 
                point = systemUnderTest.GetNextPoint();

            var fullDistance = point.GetDistance(systemUnderTest.Center);
            fullDistance.Should().BeGreaterThan(halfDistance);
        }

        [Test]
        public void GetNextPoint_ShouldGenerateUniquePoints_WhenManyCalls()
        {
            const int pointsCount = 1000;
            var points = new List<Point>();

            for (var i = 0; i < pointsCount; i++)
                points.Add(systemUnderTest.GetNextPoint());

            var hasDuplicates = points.GroupBy(x => x).Any(x => x.Count() > 1);
            hasDuplicates.Should().BeFalse();
        }
    }
}