using System.Collections.Generic;
using System.Drawing;
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
            center = new Point(20, 20);
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
            var previousPoint = spiralEnumerator.Current;
            for (int i = 0; i < 500; i++)
            {
                spiralEnumerator.MoveNext();
                var nextPoint = spiralEnumerator.Current;
                CalculateDistanceToCenter(nextPoint).Should().BeGreaterOrEqualTo(CalculateDistanceToCenter(previousPoint));
                previousPoint = nextPoint;
            }
        }

        private double CalculateDistanceToCenter(Point point)
        {
            return TagCloudTests.CalculateDistanceBetweenPoints(point, center);
        }
    }
}