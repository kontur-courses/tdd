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
            
            s.GetNextLocation().Should().Be(new PointF(0, 0));
        }
    }
}