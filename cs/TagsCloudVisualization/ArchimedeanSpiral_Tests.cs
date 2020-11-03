using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    internal class ArchimedeanSpiral_Tests
    {
        private ArchimedeanSpiral spiral;

        [SetUp]
        public void SetUp()
        {
            spiral = new ArchimedeanSpiral(new Point(), 0.1);
        }

        [Test]
        public void InitializeSpiral_ThrowException_WhenNotPositiveSpiralParameter()
        {
            Assert.Throws<ArgumentException>(() => spiral = new ArchimedeanSpiral(new Point(), -1));
        }

        [TestCase(0, 0, TestName = "WhenCenterHasZeroCoordinates")]
        [TestCase(-5, -1, TestName = "WhenCenterHasNegativeCoordinates")]
        [TestCase(10, 5, TestName = "WhenCenterHasPositiveCoordinates")]
        [TestCase(-10, 5, TestName = "WhenCenterHasPositiveAndNegativeCoordinates")]
        public void GetNextPoint_FirstPointEqualsCenter(int centerX, int centerY)
        {
            var center = new Point(centerX, centerY);
            spiral = new ArchimedeanSpiral(center, 0.1);
            spiral.GetNextPoint().Should().Be(center);
        }

        [TestCase(10, TestName = "WhenGet10Point")]
        [TestCase(100, TestName = "WhenGet100Point")]
        [TestCase(1000, TestName = "WhenGet1000Point")]
        public void GetNextPoint_PointsDoesNotEquals(int count)
        {
            var points = new List<Point>();
            for (var i = 0; i < count; i++)
                points.Add(spiral.GetNextPoint());

            foreach (var point in points)
                points.Where(x => x != point).Any(x => x == point).Should().BeFalse();
        }
    }
}