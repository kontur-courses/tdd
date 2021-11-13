using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class SpiralPointsCreator_Should
    {
        [Test]
        public void BeCreatedWithSetCenter()
        {
            var center = new Point(5, 3);
            var creator = new SpiralPointsCreator(center);

            creator.Center.Should().Be(center);
        }

        [TestCase(0, 0, TestName = "zero center is allowed")]
        [TestCase(-1, 0, TestName = "negative x centre coordinate is allowed")]
        [TestCase(0, -1, TestName = "negative y centre coordinate is allowed")]
        [TestCase(-1, -1, TestName = "negative both coordinates is allowed")]
        [TestCase(1, 1, TestName = "positive both coordinates is allowed")]
        public void CreateFirstPointInCenter(int xCenter, int yCenter)
        {
            var creator = new SpiralPointsCreator(new Point(xCenter, yCenter));
            creator.GetNextPoint().Should().Be(new Point(xCenter, yCenter));
        }

        [Test]
        public void CreateDifferentPoints()
        {
            var pointsCount = 100;
            var creator = new SpiralPointsCreator(new Point(0, 0));
            var points = new List<Point>();
            for (var i = 1; i <= pointsCount; i++) points.Add(creator.GetNextPoint());
            points.Count.Should().Be(points.Distinct().Count());
        }

        [Test]
        public void CreateExpectedNumberOfPoints()
        {
            var pointsCount = 100;
            var creator = new SpiralPointsCreator(new Point(0, 0));
            var points = new List<Point>();
            for (var i = 1; i <= pointsCount; i++) points.Add(creator.GetNextPoint());

            points.Count.Should().Be(pointsCount);
        }
    }
}