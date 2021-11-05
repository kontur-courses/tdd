using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class SpiralPointsCreator_Should
    {
        [Test]
        public void BeCreatedWithSetCenter()
        {
            var creator = new SpiralPointsCreator(new Point(0, 0));

            creator.Center.Should().Be(new Point(0, 0));
        }

        [TestCase(0,0, TestName = "zero center")]
        [TestCase(-1, 0, TestName = "negative x centre coordinate")]
        [TestCase(0, -1, TestName = "negative y centre coordinate")]
        [TestCase(-1, -1, TestName = "negative both coordinates")]
        [TestCase(1, 1, TestName = "positive both coordinates")]
        public void CreateFirstPointInCenter(int xCenter, int yCenter)
        {
            var creator = new SpiralPointsCreator(new Point(xCenter, yCenter));
            creator.GetNextPoint().Should().Be(new Point(xCenter, yCenter));
        }

        [TestCase(10)]
        [TestCase(100)]
        public void CreateDifferentPoints(int number)
        {
            var creator = new SpiralPointsCreator(new Point(0, 0));
            var points = new List<Point>();
            for (int i = 1; i <= number; i++)
            {
               points.Add(creator.GetNextPoint()); 
            }
            points.Count.Should().Be(points.Distinct().Count());
        }

        [TestCase(1)]
        [TestCase(10)]
        [TestCase(50)]
        [TestCase(100)]
        public void CreateExpectedNumberOfPoints(int number)
        {
            var creator = new SpiralPointsCreator(new Point(0, 0));
            var points = new List<Point>();
            for (int i = 1; i <= number; i++)
            {
               points.Add(creator.GetNextPoint());
            }

            points.Count.Should().Be(number);


        }
    }
}