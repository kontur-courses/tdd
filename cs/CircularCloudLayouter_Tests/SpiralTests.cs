using System;
using System.Drawing;
using System.Linq;
using CloudLayouter;
using FluentAssertions;
using NUnit.Framework;

namespace CircularCloudLayouter_Tests
{
    [TestFixture]
    public class SpiralTests
    {
        private static readonly Point Center = new(400, 400);
        private static readonly double Step = 15;
        private Spiral spiral;
        
        [SetUp]
        public void SetUp()
        {
            spiral = new Spiral(Center, Math.PI / 180, Step);
        }

        [Test]
        public void Enumerator_FirstCall_returnsCenter()
        {
            spiral.First().Should().Be(Center);
        }

        [TestCase(10, TestName = "Small number of iterations")]
        [TestCase(1000, TestName = "Big number of iterations")]
        [TestCase(90, TestName = "90 degrees")]
        [TestCase(180, TestName = "180 degrees")]
        [TestCase(360, TestName = "0 degrees")]
        public void Enumerator_NumberOfIterations_AngleChanges(int index)
        {
            var angles = GenerateNumberOfPoints(361 + index)
                .Select(GetAngleFromXAxis)
                .ToArray();
            
            var angle = angles[index];
            var opposite = angles[180 + index];
            var same = angles[360 + index];
            
            Math.Abs(opposite - angle).Should().BeInRange(Math.PI - 0.2, Math.PI + 0.2);
            angle.Should().BeInRange(same - 0.2, same + 0.2);
        }
        
        private double GetAngleFromXAxis(Point point)
        {
            return Math.Atan2(point.X - Center.X, point.Y - Center.Y);
        }

        [TestCase(10, TestName = "Small number of iterations")]
        [TestCase(90, TestName = "90 degrees")]
        [TestCase(180, TestName = "180 degrees")]
        [TestCase(278, TestName = "Angle in between quadrant points")]
        [TestCase(360, TestName = "0 degrees")]
        [TestCase(100000, TestName = "Big number of iterations")]
        public void Enumerator_NumberOfIterations_LoopsAreEquidistant(int index)
        {
            var points = GenerateNumberOfPoints(361 + index);

            var innerLoop = points[index];
            var outerLoop = points[index + 360];
            var distance = Step * 2 * Math.PI;

            innerLoop.GetDistanceTo(outerLoop)
                .Should()
                .BeInRange(distance - 1, distance + 1);
        }

        [Test]
        public void Enumerator_Iterating_SpiralRadiusGrows()
        {
            var point = GenerateNumberOfPoints(10001);

            var radius1 = Center.GetDistanceTo(point[30]);
            var radius2 = Center.GetDistanceTo(point[1000]);
            var radius3 = Center.GetDistanceTo(point[9959]);

            radius1.Should().BeLessThan(radius2);
            radius2.Should().BeLessThan(radius3);
        }

        private Point[] GenerateNumberOfPoints(int number)
        {
            return spiral.Take(number).ToArray();
        }
    }
}