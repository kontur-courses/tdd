using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudVisualizer_Should
    {
        private Point center;
        private CircularCloudLayouter layout;
        private CircularCloudVisualizer visualizer;

        [SetUp]
        public void SetUp()
        {
            center = new Point(0, 0);
            var layoutSize = new Size(2000, 2000);
            layout = new CircularCloudLayouter(center, layoutSize);    
            visualizer = new CircularCloudVisualizer(layout);
        }

        [Test]
        public void CreateNewBitmap_CreateEmptyBitmap_CorrectSizes()
        {
            visualizer.DrawRectangles(new List<Rectangle>()).Size.Should().Be(layout.LayoutSize);
        }

    }
}