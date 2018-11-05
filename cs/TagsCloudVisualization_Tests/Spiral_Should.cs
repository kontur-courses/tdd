using System;
using NUnit.Framework;
using System.Drawing;
using TagsCloudVisualization;
using FluentAssertions;

namespace TagsCloudVisualization_Tests
{
    [TestFixture]
    public class Spiral_Should
    {
        [Test]
        public void GetNextPoint_IncreasesAngle()
        {
            var center = new Point(0, 0);
            var spiral = new Spiral(1, 0);
            spiral.GetNextPoint(center);
            spiral.Angle.Should().Be(1);
        }

        [Test]
        public void GetNextPoint_GetsNextPointRight()
        {
            var center = new Point(0, 0);
            var spiral = new Spiral(1, Math.PI / 2);
            var point = spiral.GetNextPoint(center);
            point.Should().BeEquivalentTo(new Point(0, 1));
        }
    }
}