using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Geom;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class SpiralShould
    {
        [Test]
        public void ReturnCenterAsFirstLocation()
        {
            var s = new Spiral(new Point(0, 0));
            
            s.GetNextLocation().Should().Be(new PointF(0, 0));
        }
    }
}