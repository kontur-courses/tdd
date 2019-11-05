using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    [TestFixture]
    class PointExtentionTest
    {
        [Test]
        public void DistanceShouldBeZero_When_TheSamePoints()
        {
            var point = new Point(1, 3);
            point.DistanceTo(point).Should().Be(0);
        }

        [TestCase(1, 0, 1, 0, 0)]
        [TestCase(1, 0, 1, 1, 1)]
        [TestCase(6, 0, 0, 8, 10)]
        public void DistanceTo_Should_ReturnRightDistance(int x1, int y1, int x2, int y2, double distance)
        {
            new Point(x1, y1).DistanceTo(new Point(x2, y2)).Should().Be(distance);
        }
    }
}
