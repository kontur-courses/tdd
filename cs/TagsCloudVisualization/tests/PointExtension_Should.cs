using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class PointExtension_Should
    {
        [TestCase(0, 1, 1, 1, 1)]
        [TestCase(0, -1, -1, -1, 1)]
        [TestCase(0, 0, 0, 0, 0)]
        [TestCase(0, 0, 10, 10, 14.1421)]
        [TestCase(123, 321, 154, 169, 155.1289)]
        [TestCase(-321, -25, 1054, 1069, 1757.1172)]
        [TestCase(-321, -25, -1054, -1069, 1275.6272)]
        public void DistanceBetweenPoints_Should_BeEqualDistance(int x1, int y1, int x2, int y2, double expectedDistance)
        {
            var firstPoint = new Point(x1, y1);
            var secondPoint = new Point(x2, y2);
            firstPoint.GetDistance(secondPoint).Should().BeInRange(expectedDistance-0.0001, expectedDistance+0.0001);
        }
    }
}
