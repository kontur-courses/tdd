using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualization_Tests
{
    [TestFixture]
    public class CircularCloudVisualizer_Should
    {
        [Test]
        public void DrawRectangles_BeCorrectSize()
        {
            var layout = new CircularCloudLayouter(new Point(0,0));
            var visualizer = new CircularCloudVisualizer();
            var rectangles = new List<Rectangle>();
            rectangles.Add(layout.PutNextRectangle(new Size(200, 100)));
            rectangles.Add(layout.PutNextRectangle(new Size(200, 100)));
            visualizer.DrawCloud(rectangles, layout.Radius).Width.Should().Be(layout.Radius*2);
            visualizer.DrawCloud(rectangles, layout.Radius).Height.Should().Be(layout.Radius*2); 
        }
    }
}
