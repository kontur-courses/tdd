using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class Spiral_should
    {
        [Test]
        public void return_center_as_first_location()
        {
            var s = new Spiral(new Point(0, 0));
            
            s.GetNextLocation().Should().Be(new Point(0, 0));
        }

        [Test]
        public void return_locations_as_spiral()
        {
            var s = new Spiral(new Point(10, 10));
            var expectedResult = new[]
            {
                new Point(10, 10),
                new Point(10, 9),
                new Point(11, 9),
                new Point(11, 10),
                new Point(11, 11),
                new Point(10, 11),
                new Point(9, 11),
                new Point(9, 10),
                new Point(9, 9),
                new Point(9, 8)
            };

            var actualResult = new List<Point>();
            for (var i = 0; i < expectedResult.Length; i++)
                actualResult.Add(s.GetNextLocation());

            actualResult.Should().BeEquivalentTo(expectedResult);
        }
    }
}