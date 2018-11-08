using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.CloudLayouts;
using TagsCloudVisualization.CloudVisualizers;

namespace TagsCloudVisualizationTest
{
    [TestFixture]
    public class CloudVisualizer_should
    {
        [Test]
        public void Constructor_ShouldFail_OnNullCloudVisualizer()
        {
            Assert.Throws<ArgumentNullException>(() => new CloudVisualizer(Pens.Black, null));
        }
        
        [Test]
        public void GenerateImage_ShouldReturnImage_WithSameSize()
        {
            var expected = new Size(1000, 1000);       
            
            var cloudVisualizer = new CloudVisualizer(Pens.Aqua, new CircularCloudLayout(Point.Empty));
            var image = cloudVisualizer.GenerateImage(expected);

            image.Size.Should().Be(expected);
        }
    }
}