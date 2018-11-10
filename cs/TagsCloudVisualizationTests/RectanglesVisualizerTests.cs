using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    class RectanglesVisualizerTests
    {
        [Test]
        public void Should_CreateBitmap_WhenNoRectangles()
        {
            var visualizer = new RectanglesVisualizer(new Rectangle[0]);
            var bitmap = visualizer.RenderToBitmap();
            bitmap.Should().NotBeNull();
        }
    }
}
