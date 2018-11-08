using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Sequences;

namespace TagsCloudVisualizationTest
{
    [TestFixture]
    public class Spiral_Should
    {
        [Test]
        public void GetPoints_FirstPoint_ShouldBeZero()
        {
            var spiral = new Spiral();
            var expected = new Point();
            
            var actual = spiral
                .GetPoints()
                .First();
            
            actual.Should().Be(expected);
        }

        [Test]
        public void GetPoints_ShouldReturnNotOnlyZeroPoints()
        {
            var spiral = new Spiral();
            
            var actual = spiral
                .GetPoints()
                .Take(100)
                .Where(p => p != Point.Empty);
            
            actual.Any().Should().BeTrue();
        }
        
        [Test]
        public void Constructor_ShouldFail_WhenZeroStep()
        {
            Assert.Throws<ArgumentException>(() => new Spiral(0));
        }
    }
}