using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    class VisualizatorTests
    {
        [Test]
        public void Should_CreateBitmap_SpecifiedSize()
        {
            var visualizator = new Visualizator(new List<Rectangle>());
            var size = new Size(200, 100);
            var bitmap = visualizator.CreateBitmap(size);
            bitmap.Size.Should().BeEquivalentTo(size);
        }
    }
}
