using System.Drawing;
using FluentAssertions;
using HomeExerciseTDD;
using NUnit.Framework;

namespace TestProject1
{
    [TestFixture]
    public class SpiralTests
    {
        [Test]
        public void SpiralGetNextPoint_ReturnCenterPoint_WhenFirstCall()
        {
            var center = new Point(22, 22);
            Spiral spiral = new Spiral(center);

            var nextPoint = spiral.GetNextPoint();

            nextPoint.Should().BeEquivalentTo(center);
        }
        [Test]
        public void SpiralGetNextPoint_ReturnOtherPoint_WhenNotFirstCall()
        {
            var center = new Point(22, 22);
            Spiral spiral = new Spiral(center);

            var nextPoint = spiral.GetNextPoint();
            nextPoint = spiral.GetNextPoint();

            nextPoint.Should().NotBeEquivalentTo(spiral.GetNextPoint());
        }
    }
}