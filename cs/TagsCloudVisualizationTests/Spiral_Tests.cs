using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    public class ArchimedeanSpiral_Tests
    {
        private ArchimedeanSpiral spiral;
        private List<Point> points;
        private Point center;

        [SetUp]
        public void Setup()
        {
            center = Point.Empty;
            spiral = new ArchimedeanSpiral(center);
            points = new List<Point>();
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