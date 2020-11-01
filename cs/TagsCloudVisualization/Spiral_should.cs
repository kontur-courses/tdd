using System;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class Spiral_should
    {
        [Test]
        public void Spiral_ShouldStartInCenter()
        {
            var center = new Point(10,10);
            var spiral = new Spiral(center, 1, 1);
            spiral.GetNextPoint().Should().BeEquivalentTo(center);
        }
        
        [Test]
        public void Spiral_ShouldIncreaseDistanceFromCenter_AfterSomeSteps()
        {
            var center = new Point(0,0);
            var spiral = new Spiral(center, 100, 0.05 * Math.PI);
            
            var prevDistance = getDistance(spiral.GetNextPoint(), center);
            
            for (var i = 0; i < 100; i++)
            {
                var newDistance = getDistance(spiral.GetNextPoint(), center);
                newDistance.Should().BeGreaterThan(prevDistance);
                prevDistance = newDistance;
            }
        }

        private double getDistance(Point first, Point second)
        {
            return Math.Sqrt(Math.Pow(first.X - second.X, 2)
                             + Math.Pow(first.Y - second.Y, 2));
        }
        
        [Test]
        public void Spiral_ShouldReturnCorrectPoints_AfterSomeSteps()
        {
            var center = new Point(0,0);
            var spiral = new Spiral(center, 4, 0.5 * Math.PI);
            spiral.GetNextPoint().Should().BeEquivalentTo(new Point(0,0));
            spiral.GetNextPoint().Should().BeEquivalentTo(new Point(0,1));
            spiral.GetNextPoint().Should().BeEquivalentTo(new Point(-2,0));
            spiral.GetNextPoint().Should().BeEquivalentTo(new Point(0,-3));
            spiral.GetNextPoint().Should().BeEquivalentTo(new Point(4,0));
        }
    }
}