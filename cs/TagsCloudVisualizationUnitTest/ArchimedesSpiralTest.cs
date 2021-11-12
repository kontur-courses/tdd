using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationUnitTest
{
    class ArchimedesSpiralTest
    {
        [Test]
        public void CalculateFirstPoint_ShouldBe_In_Centre()
        {
            var centre = new Point(100, 100);
            var spiral = new ArchimedesSpiral(centre);

            spiral.CalculatePoint().Should().Be(centre);
        }
    }
}