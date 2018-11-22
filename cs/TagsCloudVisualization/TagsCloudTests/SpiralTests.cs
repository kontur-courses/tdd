using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudTests
{
    [TestFixture]
    public class SpiralTests
    {
        private Spiral spiral;
        private Point center;
        private IEnumerator<Point> spiralEnumerator;
        
        [SetUp]
        public void SetUp()
        {
            center = new Point(0, 0);
            spiral = new Spiral(center);
            spiralEnumerator = spiral.GetNextPoint().GetEnumerator();
            spiralEnumerator.MoveNext();
        }
        
        [Test]
        public void GetNextPoint_ReturnPointWithCoordinatesOfCenterSpiral_ForFirstCall()
        {
            var l = spiralEnumerator.Current;
            l.Should().BeEquivalentTo(center);
            
        }

        [Test]
        public void GetNextPoint_DistanceBetweenEveryNextPointAndCenter_ShouldBeLargerThanPrevious()
        {
            var maximumRadiusDifference = 2;
            var pointsToCheck = spiral.GetNextPoint().Take(100).ToArray();
            var previousPoint = pointsToCheck.First();

            foreach (var point in pointsToCheck.Skip(1))
            {
                var currentDistanceToCenter = CalculateDistanceToCenter(point);
                var previousDistanceToCenter = CalculateDistanceToCenter(previousPoint);
                var radiusDifference = currentDistanceToCenter - previousDistanceToCenter;
                radiusDifference.Should().BeLessThan(maximumRadiusDifference);
                previousPoint = point;
            }
        }

        private double CalculateDistanceToCenter(Point point)
        {
            return TagCloudTests.CalculateDistanceBetweenPoints(point, center);
        }
    }
}