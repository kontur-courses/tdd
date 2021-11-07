using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class Spiral_Tests
    {
        private Spiral spiral;
        private List<Point> points;
        private Point center;

        [SetUp]
        public void Setup()
        {
            center = Point.Empty;
            spiral = new Spiral(center);
            points = new List<Point>();
        }

        [Test]
        public void Ctor_ThrowsException_WhenDensityIsNegative()
        {
            Assert.Throws<ArgumentException>(() => spiral = new Spiral(Point.Empty, -1));
        }
        
        [Test]
        public void GetNextPoint_ReturnsSpiralCenter_OnFirstCall()
        {
            var point = spiral.GetNextPoint();
            point.Should().Be(center);
        }

        [Test]
        public void GetNextPoint_EveryNextPointShouldBeUnique()
        {
            for (var i = 0; i < 250; i++)
            {
                points.Add(spiral.GetNextPoint());
            }

            points.Should().OnlyHaveUniqueItems();
        }
    }
}