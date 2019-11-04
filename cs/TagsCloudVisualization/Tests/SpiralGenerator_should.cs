using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Tests
{
    class SpiralGenerator_should
    {
        [TestCase(-1, -1)]
        [TestCase(1, -1)]
        [TestCase(-1, 1)]
        public void Constructor_WhenPointWithNegativeCord_ThrowArgumentException(int x, int y)
        {
            var center = new Point(x, y);
            Action action = () =>
            {
                // ReSharper disable once ObjectCreationAsStatement
                new SpiralGenerator(center);
            };
            action.ShouldThrow<ArgumentException>();
        }

        private static Point center;
        private static SpiralGenerator generator;

        [SetUp]
        public void SetUp()
        {
            center = new Point(5, 5);
            generator = new SpiralGenerator(center);
        }


        [Test]
        public void GetNextPoint_WhenGetOnePoint_ReturnCenterPoint()
        {
            generator.GetNextPoint().Should().Be(generator.Center);
        }

        [Test]
        public void GetNextPoint_WhenGetTwoPoint_ReturnDifferentPoints()
        {
            var first = generator.GetNextPoint();
            generator.GetNextPoint().Should().NotBe(first);
        }

        [Test]
        public void GetNextPoint_WhenGetAnyPoint_ReturnDifferentPoints()
        {
            var hash = new HashSet<Point>();
            for (var i = 0; i < 20; i++)
            {
                var point = generator.GetNextPoint();
                hash.Should().NotContain(point);
                hash.Add(point);
            }
        }
    }
}
